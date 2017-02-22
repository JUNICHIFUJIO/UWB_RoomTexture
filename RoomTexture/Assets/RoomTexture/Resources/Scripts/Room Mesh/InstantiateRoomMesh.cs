using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using HoloToolkit.Unity;

namespace UWB_RoomTexture {

    public class InstantiateRoomMesh : MonoBehaviour {

        //public bool instantiated = false;

        public GameObject spatialMappingObj;
        //public GameObject room;
        //public string ShaderName = "RoomProjectionShader";

        // Use this for initialization
        void Start() {
            // Add the SpatialMapping prefab to Unity hierarchy
            //
            // Assumes the SpatialMapping prefab exists at
            // "Assets/HoloToolkit/Prefabs/SpatialMapping.prefab"

            spatialMappingObj = null;

            InstantiateSpatialMappingPrefab();

            //spatialMappingObj = Resources.Load<GameObject>(RoomTexture.SpatialMappingPrefabPath);

            /*
            spatialMappingObj = Resources.Load<GameObject>(
                Application.dataPath + Path.DirectorySeparatorChar
                + "HoloToolkit" + Path.DirectorySeparatorChar
                + "SpatialMapping" + Path.DirectorySeparatorChar
                + "Prefabs" + Path.DirectorySeparatorChar
                + "SpatialMapping.prefab");
                */

            // ERROR TESTING Unnecessary?
            //spatialMappingObj = GameObject.Instantiate<GameObject>(spatialMappingObj);


            InitializeRoom();
            //room = new GameObject();
            //GrabRoomMesh();
        }

        void InstantiateSpatialMappingPrefab()
        {
            spatialMappingObj = Instantiate<GameObject>(Resources.Load<GameObject>(Constants.Folders.SpatialMappingPrefabPath));
        }

        void InitializeRoom()
        {
            // if the spatial mapping object doesn't exist, create it
            if (spatialMappingObj == null)
                InstantiateSpatialMappingPrefab();

            // move the room mesh over to the spatialmappingobj
            ObjectSurfaceObserver surfaceObserver = spatialMappingObj.GetComponent<ObjectSurfaceObserver>();
            surfaceObserver.roomModel = Resources.Load<GameObject>(Constants.Folders.RoomMeshFolderPath + Constants.Names.RoomMeshName + Constants.Suffixes.RoomMeshSuffix);
            // set the material that the spatialmapping object will use to render it
            SpatialMappingManager mappingManager = spatialMappingObj.GetComponent<SpatialMappingManager>();
            mappingManager.SetSurfaceMaterial(CreateRoomMeshMaterial());
        }

        Material CreateRoomMeshMaterial()
        {
            Material roomCanvasMaterial = new Material(Shader.Find(Constants.Shaders.RoomBackground));
            roomCanvasMaterial.SetColor("_BaseColor", Color.black);

            return roomCanvasMaterial;
        }
        
        void AddProjectorChild(Projector projector)
        {
            //projector.transform.parent = room.transform;
            projector.transform.parent = spatialMappingObj.GetComponent<ObjectSurfaceObserver>().roomModel.transform;
        }
        
        //----------------------------------------------------
        /*
        void GrabRoomMesh()
        {
            // Grab the saved room mesh from SpatialMapping's
            // Object Surface Observer component
            room = spatialMappingObj.GetComponent<HoloToolkit.Unity.ObjectSurfaceObserver>().roomModel;
            SetRoomToBlack();
        }

        void SetRoomToBlack()
        {
            // Set the appropriate items for the room object
            // 1) Instantiate a pure black material to paint the room
            // This makes it so that projections projecting on it display easily
            // (Black has an RGB value of 0,0,0 and white has a value of 1,1,1)
            // If adding together projections, colors for each pixel must be
            // between these values
            Material blackMat = new Material(Shader.Find("RoomCanvasShader"));
            room.GetComponent<MeshRenderer>().material = blackMat;
        }
        */
    }
}