using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

namespace UWB_RoomTexture
{
    namespace Constants
    {
        public class Folders : MonoBehaviour
        {
            public static string FolderRoot =
                Application.dataPath + Path.AltDirectorySeparatorChar
                + "RoomTexture" + Path.AltDirectorySeparatorChar
                + "Resources" + Path.AltDirectorySeparatorChar;

            public static string AssetFolderRoot =
                "Assets" + Path.AltDirectorySeparatorChar
                + "Resources" + Path.AltDirectorySeparatorChar;

            public static string PrefabFolderPath =
                "_Prefabs" + Path.AltDirectorySeparatorChar;
            
            public static string CameraLocationFolderPath =
                FolderRoot
                + "CameraLocations" + Path.AltDirectorySeparatorChar;

            public static string MaterialFolderPath =
                "Materials" + Path.AltDirectorySeparatorChar;

            public static string RoomMeshFolderPath =
                "Meshes" + Path.AltDirectorySeparatorChar;

            public static string ShaderFolderPath =
                "Shaders" + Path.AltDirectorySeparatorChar;
            
            //public static string RoomTextureFolderPath_Load =
            //    "Textures" + Path.AltDirectorySeparatorChar
            //    + "Raw" + Path.AltDirectorySeparatorChar;

            public static string RoomTextureFolderPath =
                FolderRoot
                + "Textures" + Path.AltDirectorySeparatorChar
                + "Raw" + Path.AltDirectorySeparatorChar;

            public static string ClippedRoomTextureFolderPath_Load =
                "Textures" + Path.AltDirectorySeparatorChar
                + "Final" + Path.AltDirectorySeparatorChar;

            public static string ClippedRoomTextureFolderPath =
                FolderRoot
                + "Textures" + Path.AltDirectorySeparatorChar
                + "Final" + Path.AltDirectorySeparatorChar;
            
            public static string RoomMeshFolderPath_HoloToolkit =
#if !UNITY_EDITOR
            ApplicationData.Current.RoamingFolder.Path;
#else
            Application.persistentDataPath;
#endif

            // ERROR TESTING REMOVE
            //public static string ProjectorFolderPath =
            //    "Projectors" + Path.AltDirectorySeparatorChar;
            
            // ERROR TESTING REMOVE
            public static string SpatialMappingPrefabPath =
                FolderRoot
                + "HoloToolkit" + Path.DirectorySeparatorChar
                + "SpatialMapping" + Path.DirectorySeparatorChar
                + "Prefabs" + Path.DirectorySeparatorChar
                + "SpatialMapping.prefab";
        }
    }
}