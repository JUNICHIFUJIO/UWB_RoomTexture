using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    namespace Constants
    {
        public class Shaders : MonoBehaviour
        {
            public static string RoomBackground = "RoomCanvasShader";
            public static string RoomTexture = "RoomProjectionShader_1";
            public static string RoomTexture_NonProjector = "RoomProjectionShader_NonProjector";
        }
    }
}