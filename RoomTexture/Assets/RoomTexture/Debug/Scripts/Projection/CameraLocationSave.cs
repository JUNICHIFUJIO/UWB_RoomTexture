using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

namespace UWB_RoomTexture
{
    namespace UWB_Debug
    {
        public class CameraLocationSave : MonoBehaviour
        {
            public Matrix4x4 WorldToCamera = new Matrix4x4();
            public Matrix4x4 ProjectionMatrix = new Matrix4x4();
            public float NearClipPlane = 0.0f;
            public float FarClipPlane = 0.0f;
            public bool CameraHasLocationData = false;
            public string Filename = "";
            public bool Save = false;

            private string defaultFilename = "Test";

            void OnValidate()
            {
                if (Save == true)
                {
                    CameraLocation camLoc = new CameraLocation();
                    camLoc.WorldToCameraTransform = WorldToCamera;
                    camLoc.ProjectionTransform = ProjectionMatrix;
                    camLoc.NearClipPlane = NearClipPlane;
                    camLoc.FarClipPlane = FarClipPlane;
                    camLoc.HasLocationData = CameraHasLocationData;

                    if (Filename.Equals(string.Empty))
                        Filename = defaultFilename;
                    CameraLocation.Save(camLoc, Constants.Names.CameraLocationAutoName + Filename);

                    //CameraLocation.Save(camLoc, "Debug_CamLoc");
                    // ERROR TESTING REMOVE
                    //File.WriteAllText(Constants.Folders.CameraLocationFolderPath + "Debug_CamLoc" + Constants.Suffixes.FileSuffix_CameraLocation, "This is a test");

                    Save = false;
                }
            }
        }
    }
}