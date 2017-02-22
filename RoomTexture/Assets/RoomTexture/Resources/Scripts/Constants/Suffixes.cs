using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    namespace Constants
    {
        public class Suffixes : MonoBehaviour
        {
            // ERROR TESTING remove?
            public static string RoomMeshSuffix = ".room";

            // FileSuffix
            //public static string FileSuffix = ".png";

            // FileSuffixTypes
            public enum ImageSuffixTypes
            {
                Jpeg,
                PNG
            };

            public static string FileSuffix_PNG = ".png";
            public static string FileSuffix_JPG = ".jpg";

            public static string FileSuffix_CameraLocation = ".camloc";

            public static string FileSuffix_Material = ".mat";
            public static string FileSuffix_Prefab = ".prefab";
        }
    }
}