using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    namespace Constants
    {
        public class DebugStrings : MonoBehaviour
        {
            public static bool DebugFlag = false;

            public static string PhotoCaptureModeInitFailed = "Photo Capture mode initialization failed.";
            public static string PhotoCaptured = "Photo capture mode ended. Processing data...";
            public static string PhotoCaptureModeEnded = "Photo capture mode ended successfully.";
        }
    }
}