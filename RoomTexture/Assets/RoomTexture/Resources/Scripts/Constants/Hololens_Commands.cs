using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    namespace Constants
    {
        public class Commands : MonoBehaviour
        {
            public enum VoiceCommandsEnum
            {
                BeginRoomTexture,
                RoomScreenshot,
                EndRoomTexture
            };

            public enum GesturesEnum
            {
                RoomScreenshot
            };
            
            public static string Keyword_BeginRoomTexturing = "Start Room Texturing";
            public static string Keyword_RoomScreenshot = "Texture";
            public static string Keyword_EndRoomTexturing = "Stop Room Texturing";
        }
    }
}