using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Platform-specific line spacing
//using System.Linq; // For grabbing first CameraResolution from the available ones

namespace UWB_RoomTexture
{
    namespace Constants
    {
        public class Camera : MonoBehaviour
        {
            public static TextureFormat Format = TextureFormat.RGBA32;

            // ERROR TESTING - LINQ LAMBDA EQUATION CAUSED FILE TO CRASH
            //            public static Resolution CameraResolution = UnityEngine.VR.WSA.WebCam.PhotoCapture.SupportedResolutions.OrderByDescending<Resolution, int>((res) => res.width * res.height).First();
            // ERROR TESTING - HOLDOVER THAT LED TO UNDERSTANDING OF LINQ CRASH
            //public static Resolution CameraResolution = new Resolution() { height = 12, refreshRate = 3, width = 3 };
            public static TextureWrapMode WrapMode = TextureWrapMode.Clamp;

            public static string CameraLocationAutoName = "CameraLocation_";
            public static string FileSuffix_CameraLocation = ".camloc";
            public static char CameraLocation_MatrixCellSeparator = ',';
            public static string CameraLocation_MatrixSeparator = Environment.NewLine + Environment.NewLine;

            // ERROR TESTING - USING ANY LINQ DIRECTIVES OR ACCESSING Hololens' camera resolution causes a crash
            public static Resolution CameraResolution()
            {
                Resolution camRes = new Resolution();
                camRes.width = 1280;
                camRes.height = 720;
                camRes.refreshRate = 30;

                return camRes;

                //IEnumerable resEnumerable = UnityEngine.VR.WSA.WebCam.PhotoCapture.SupportedResolutions;
                //return (UnityEngine.Resolution)resEnumerable.GetEnumerator().Current;

                // ERROR TESTING REMOVE
                //return UnityEngine.VR.WSA.WebCam.PhotoCapture.SupportedResolutions.First();
            }
        }
    }
}
