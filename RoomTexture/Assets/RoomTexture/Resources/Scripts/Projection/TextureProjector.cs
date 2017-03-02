using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.WebCam;

namespace UWB_RoomTexture
{
    public class TextureProjector : MonoBehaviour
    {
        // This assumes that all information found on 
        // https://developer.microsoft.com/en-us/windows/holographic/locatable_camera
        // is correct.
        public static Projector GenerateProjector(string CameraLocationFilePath, string TextureFilePath)
        {
            Projector newProjector = null;

            CameraLocation camLoc = CameraLocation.Load(CameraLocationFilePath);

            if (camLoc.HasLocationData)
            {
                newProjector = new Projector();

                // Set the main aspects of the projector
                int horizontalFOV = CameraLocation.GetHorizontalFOV(Constants.Camera.CameraResolution()); // in degrees
                newProjector.aspectRatio = 16.0f / 9.0f;
                newProjector.fieldOfView = horizontalFOV / newProjector.aspectRatio;
                newProjector.orthographic = false;
                newProjector.nearClipPlane = camLoc.NearClipPlane;
                newProjector.farClipPlane = camLoc.FarClipPlane;

                // Set the position, rotation, and scale of the projector
                // This information was found on a forum post at http://answers.unity3d.com/questions/402280/how-to-decompose-a-trs-matrix.html
                newProjector.transform.position = camLoc.WorldToCameraTransform.GetColumn(3);
                newProjector.transform.rotation = Quaternion.LookRotation(
                    camLoc.WorldToCameraTransform.GetColumn(2),
                    camLoc.WorldToCameraTransform.GetColumn(1)
                    );
                newProjector.transform.localScale = new Vector3(
                    camLoc.WorldToCameraTransform.GetColumn(0).magnitude,
                    camLoc.WorldToCameraTransform.GetColumn(1).magnitude,
                    camLoc.WorldToCameraTransform.GetColumn(2).magnitude
                    );

                // Generate and set the material
                Texture2D tex = LoadTexture.Load(TextureFilePath);
                //Material mat = MaterialMaker.GenerateRoomMaterial(tex, RoomTexture.Projector_MaterialAutoName + camLoc.name);
                Material mat = MaterialMaker.GenerateRoomMaterial(tex);
#if UNITY_EDITOR
                // ERROR TESTING - DOESN'T EVEN APPLY TO FINAL BUILD
                SaveMaterial.Save(mat);
#endif
                
                //                AssetDatabase.CreateAsset(mat, RoomTexture.MaterialFolderPath + mat.name);
                //               AssetDatabase.Refresh();
                newProjector.material = mat;


                // Set the projector's name
                //newProjector.name = RoomTexture.Projector_AutoName + camLoc.name;
                newProjector.name = FileNameTranslator.ClippedTextureToProjector(tex.name);

                // Set it to ignore all layers except for the one it needs to project onto
                int layerID = LayerMask.NameToLayer(Constants.Names.LayerName);
                int ignoreLayer = 1 << layerID;
                ignoreLayer = ~ignoreLayer;
                newProjector.ignoreLayers = ignoreLayer;

                // ERROR TESTING REMOVE
                //AssetDatabase.CreateAsset(newProjector, Constants.Folders.ProjectorFolderPath + FileNameTranslator.ClippedTextureToProjector(tex.name));
                //AssetDatabase.SaveAssets();
            }

            return newProjector;
        }

        // This assumes that all information found on 
        // https://developer.microsoft.com/en-us/windows/holographic/locatable_camera
        // is correct.
        // 
        // This version of the method adds the projector to the container since you can't add manipulated components to a GameObject dynamically
        public static Projector GenerateProjector(GameObject container, string CameraLocationFilePath, string TextureFilePath)
        {
            Projector newProjector = null;

            CameraLocation camLoc = CameraLocation.Load(CameraLocationFilePath);

            if (camLoc.HasLocationData && container != null)
            {
                newProjector = container.AddComponent<Projector>();

                // Set the main aspects of the projector
                int horizontalFOV = CameraLocation.GetHorizontalFOV(Constants.Camera.CameraResolution()); // in degrees
                newProjector.aspectRatio = 16.0f / 9.0f;
                newProjector.fieldOfView = horizontalFOV / newProjector.aspectRatio;
                newProjector.orthographic = false;
                newProjector.nearClipPlane = camLoc.NearClipPlane;
                newProjector.farClipPlane = camLoc.FarClipPlane;

                CameraLocation.SetTransformValues(newProjector.transform, camLoc);
                // ERROR TESTING REMOVE
                /*
                // Set the position, rotation, and scale of the projector
                // This information was found on a forum post at http://answers.unity3d.com/questions/402280/how-to-decompose-a-trs-matrix.html
                newProjector.transform.position = camLoc.WorldToCameraTransform.GetColumn(3);
                newProjector.transform.rotation = Quaternion.LookRotation(
                    camLoc.WorldToCameraTransform.GetColumn(2),
                    camLoc.WorldToCameraTransform.GetColumn(1)
                    );
                newProjector.transform.localScale = new Vector3(
                    camLoc.WorldToCameraTransform.GetColumn(0).magnitude,
                    camLoc.WorldToCameraTransform.GetColumn(1).magnitude,
                    camLoc.WorldToCameraTransform.GetColumn(2).magnitude
                    );
                    */

                // Set the name
                newProjector.name = Constants.Names.Projector_AutoName + camLoc.name;

                // Generate and set the material
                Texture2D tex = LoadTexture.Load(TextureFilePath);
                //Material mat = MaterialMaker.GenerateRoomMaterial(tex, RoomTexture.Projector_MaterialAutoName + camLoc.name);
                Material mat = MaterialMaker.GenerateRoomMaterial(tex);
#if UNITY_EDITOR
                // ERROR TESTING - DOESN'T EVEN APPLY TO FINAL BUILD
                SaveMaterial.Save(mat);
#endif
                //                AssetDatabase.CreateAsset(mat, RoomTexture.MaterialFolderPath + mat.name);
                //               AssetDatabase.Refresh();
                newProjector.material = mat;

                // Set it to ignore all layers except for the one it needs to project onto
                int layerID = LayerMask.NameToLayer(Constants.Names.LayerName);
                int ignoreLayer = 1 << layerID;
                ignoreLayer = ~ignoreLayer;
                newProjector.ignoreLayers = ignoreLayer;

                // ERROR TESTING REMOVE
                //AssetDatabase.CreateAsset(newProjector, Constants.Folders.ProjectorFolderPath + FileNameTranslator.ClippedTextureToProjector(tex.name));
                //AssetDatabase.SaveAssets();
            }

            return newProjector;
        }

        public static void AddProjectorToParent(GameObject parent, GameObject projector)
        {
            projector.transform.parent = parent.transform;
            // ERROR TESTING - DO I NEED TO MANUALLY ADD THE ITEM TO THE SCENE'S OBJECT AND HAVE IT AS A CHILD?
            // IF SO, USE SceneAsset.Instantiate (?)
        }
    }
}