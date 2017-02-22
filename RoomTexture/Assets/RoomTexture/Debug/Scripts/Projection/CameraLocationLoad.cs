using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    namespace UWB_Debug
    {
        public class CameraLocationLoad : MonoBehaviour
        {
            public Matrix4x4 WorldToCamera = new Matrix4x4();
            public Matrix4x4 ProjectionMatrix = new Matrix4x4();
            public float NearClipPlane = 0.0f;
            public float FarClipPlane = 0.0f;
            public string Filename = "";
            public bool Load = false;

            private string defaultFilename = "Test";

            void OnValidate()
            {
                if(Load == true)
                {
                    if (Filename.Equals(string.Empty))
                        Filename = defaultFilename;
                    CameraLocation camLoc = CameraLocation.Load(Constants.Folders.CameraLocationFolderPath + Constants.Names.CameraLocationAutoName + Filename + Constants.Suffixes.FileSuffix_CameraLocation);
                    
                    // ERROR TESTING REMOVE
                    //CameraLocation camLoc = CameraLocation.Load(Constants.Folders.CameraLocationFolderPath + "Debug_CamLoc" + Constants.Suffixes.FileSuffix_CameraLocation);

                    WorldToCamera = camLoc.WorldToCameraTransform;
                    ProjectionMatrix = camLoc.ProjectionTransform;
                    NearClipPlane = camLoc.NearClipPlane;
                    FarClipPlane = camLoc.FarClipPlane;
                    
                    Load = false;
                }
            }
        }
    }
}