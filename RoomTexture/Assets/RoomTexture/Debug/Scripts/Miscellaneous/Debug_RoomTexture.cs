using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace UWB_RoomTexture
{
    namespace UWB_Debug
    {
        public class Debug_RoomTexture : MonoBehaviour
        {
            public static bool Flag_Info = true;

            public static string Debug_TestPassedString = "Passed.";
            public static string Debug_TestFailString = "***TEST FAILED.***";

            public static string Debug_FolderRoot =
                Application.dataPath + Path.AltDirectorySeparatorChar
                + "Debug" + Path.AltDirectorySeparatorChar;

            public static string Debug_MaterialFolderPath =
                Debug_FolderRoot
                + "Materials" + Path.AltDirectorySeparatorChar;

            public static string Debug_MeshFolderPath =
                Debug_FolderRoot
                + "Meshes" + Path.AltDirectorySeparatorChar;

            public static string Debug_TextureFolderPath =
                Debug_FolderRoot
                + "Textures" + Path.AltDirectorySeparatorChar;

            public static string Debug_PrefabFolderPath =
                Debug_FolderRoot
                + "Prefabs" + Path.AltDirectorySeparatorChar;

            public static string Debug_MaterialTestName = "Test";
            public static string Debug_MeshTestName = "Test";
            public static string Debug_TextureTestName = "Test";
            public static string Debug_PrefabTestName = "Test";
        }
    }
}