using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UWB_RoomTexture {
    public class CentralProcessor : MonoBehaviour {

        public static GameObject RoomMesh = null;
        public float Trigger; // ERROR TESTING - WORKAROUND TO TRIGGER THE EVENT INSIDE OF THE UNITY EDITOR

        #region Event / Handlers
        public delegate void QueuedTextureRefreshEventHandler();
        public event QueuedTextureRefreshEventHandler OnTextureRefresh;
        #endregion

        // When anything in Unity's inspector changes, it'll trigger the texture refreshing
        // Need to switch this over to be outside of Unity's editor
        void OnValidate()
        {
            //OnTextureRefresh.Invoke();
            AttachPrefab();
            SetMaterials();
        }

        // This runs everything, and is set so that when the event is triggered, it cascades the wealth of commands it needs to
        public static GameObject GenerateTexturePrefab()
        {
            // Grab the Room Texture folder
            // Grab the CameraLocation folder
            // 

            // Get the textures out of the texture folder and make materials out of them
            // Get the camera locations and generate projectors
            // Get the room mesh from wherever it is stored
            // Instantiate the room mesh and save it to the scene
            // Instantiate the projectors and save them to the scene
            // Make the projectors children of the mesh
            // Make the mesh and all its children a prefab - save this prefab
            // Save the scene and asset database
            
            // Alpha clip adjust and SAVE the textures
            foreach(string rawTexFile in Directory.GetFiles(Constants.Folders.RoomTextureFolderPath))
            {
                Texture2D rawRoomTex = LoadTexture.Load(rawTexFile);
                AlphaClipTexture.AlphaClip(rawRoomTex);
            }

            // Create an empty GameObject to house all of the Texture Projectors
            GameObject RoomTextureObject = new GameObject();
            RoomTextureObject.name = Constants.Names.TexturePrefabName;
            LayerSwitcher.SetLayer(RoomTextureObject, Constants.Names.LayerName);

            // Load up all alpha clipped textures to create materials
            foreach (string clippedTexFile in Directory.GetFiles(Constants.Folders.ClippedRoomTextureFolderPath))
            {
                string clippedRoomTexName = Path.GetFileNameWithoutExtension(clippedTexFile);
                
                // Generate the projector and its GameObject container
                // Attach the container onto the overall RoomTextureObject as a child
                GameObject ProjectorChild = new GameObject();
                ProjectorChild.transform.parent = RoomTextureObject.transform;
                Projector textureProjector = TextureProjector.GenerateProjector(
                    ProjectorChild, // GameObject to house the projector
                    Constants.Folders.CameraLocationFolderPath + FileNameTranslator.ClippedTextureToCameraLocation(clippedRoomTexName), // Camera Location file path
                    clippedTexFile // Clipped texture file path
                    );
                ProjectorChild.name = textureProjector.name;
            }

#if UNITY_EDITOR
            // Create a prefab of all the projectors
            GameObject prefab = PrefabUtility.CreatePrefab(Constants.Folders.PrefabFolderPath + RoomTextureObject.name, RoomTextureObject);
            SaveTexturePrefab.Save();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets(); // ERROR TESTING - is this the correct order?
#else
            GameObject prefab = null;
#endif

            return prefab;
        }

        public static void AttachPrefab()
        {
            GameObject textureObject = GameObject.Find(Constants.Names.TexturePrefabName);
            if (textureObject != null)
            {
                GameObject.Destroy(textureObject);
                textureObject = null;
            }

            GameObject SpatialMapper = GameObject.Find(Constants.Names.Holotoolkit_SpatialMappingName);
            if (SpatialMapper != null)
            {
                LayerSwitcher.ChangeMeshLayer();
                Debug.Log("Layer mask adjusted for SpatialMapping room mesh.");
                AttachPrefabToObject(SpatialMapper);
            }

            if (RoomMesh != null)
            {
                LayerSwitcher.ChangeMeshLayer(RoomMesh);
                Debug.Log("Layer mask adjusted for " + RoomMesh.name + " mesh.");
                AttachPrefabToObject(RoomMesh);
                RoomMesh = null;
            }
            


            // 3 SCENARIOS FOR MESH EXISTENCE (ASIDE FROM NO MESH EXISTING)
            // If the Spatial Mapper exists in the scene, attach it to that
            // Assume the guys will hit 'L' to load up the saved mesh

            // If the room mesh exists in SRMesh, attach it to spatial mapper as well

            // If the room mesh exists as a separate mesh that user is manipulating inside of teh Unity editor
        }
        
        // Attach texture prefab to the mesh object
        public static void AttachPrefabToObject(GameObject obj)
        {
            if (obj != null) {
#if UNITY_EDITOR
                string prefabFilepath = Constants.Folders.PrefabFolderPath + Constants.Names.TexturePrefabName + Constants.Suffixes.FileSuffix_Prefab;
                if (Directory.Exists(prefabFilepath))
                {
                    GameObject texturePrefab = PrefabUtility.InstantiatePrefab(Resources.Load(prefabFilepath)) as GameObject;
                    texturePrefab.transform.parent = obj.transform;
                }
#else
                // ERROR TESTING REMOVE - How do I adjust for this in the final version?
                GameObject texturePrefab = null;
#endif
                //texturePrefab.transform.parent = obj.transform;
            }
        }

        public static void DetachPrefabFromObject()
        {
            GameObject textureObject = GameObject.Find(Constants.Names.TexturePrefabName);
            if (textureObject != null)
            {
                GameObject.Destroy(textureObject);
                textureObject = null;
            }
        }

        public static void SetMaterials()
        {
            Material RoomCanvasMaterial = null;
            
            if (Constants.Thresholds.LessThanTextureThreshold)
            {
                RoomCanvasMaterial = MaterialMaker.GenerateRoomMaterial_NonProjector();
            }
            else
            {
                RoomCanvasMaterial = MaterialMaker.GenerateRoomCanvasMaterial();
            }

            // Set the Holotoolkit generated canvas mesh
            SetMaterials(GameObject.Find(Constants.Names.Holotoolkit_RemoteMappingName), RoomCanvasMaterial);
            // Set the passed in canvas mesh
            if (RoomMesh != null)
            {
                SetMaterials(RoomMesh, RoomCanvasMaterial);
            }
        }

        private static void SetMaterials(GameObject obj, Material mat)
        {
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.material = mat;

            int numChildren = obj.transform.childCount;
            for(int i = 0; i < numChildren; i++)
            {
                SetMaterials(obj.transform.GetChild(i).gameObject, mat);
            }
        }
        
        public static void UpdateRenderMethodUsed(bool startup = false)
        {
            int numFiles = Directory.GetFiles(Constants.Folders.ClippedRoomTextureFolderPath).Length;
            bool updatedLessThanTextureThreshold = numFiles < Constants.Thresholds.TextureThreshold;
            if (updatedLessThanTextureThreshold != Constants.Thresholds.LessThanTextureThreshold || startup)
            {
                CentralProcessor.SetMaterials();
                AttachPrefab();
            }
            Constants.Thresholds.LessThanTextureThreshold = numFiles < Constants.Thresholds.TextureThreshold;
        }

        // Use this for initialization
        void Start() {
            //OnTextureRefresh += new QueuedTextureRefreshEventHandler(Process); // null should be object
        }
    }
}