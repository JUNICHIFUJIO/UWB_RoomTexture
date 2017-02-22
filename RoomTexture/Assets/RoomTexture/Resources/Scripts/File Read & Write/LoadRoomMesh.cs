using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

// ERROR TESTING DELETE THIS
namespace UWB_RoomTexture
{
    public class LoadRoomMesh : MonoBehaviour
    {
        public static GameObject Load(string filepath)
        {
            if (Path.GetExtension(filepath).Equals(Constants.Suffixes.FileSuffix_Prefab))
                return Resources.Load(filepath) as GameObject;
            else
                throw new System.Exception(Constants.ErrorStrings.UnsupportedPrefabFileType);
        }
    }
}