// ERROR TESTING REMOVE
/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UWB_RoomTexture
{
    public class SaveRoomMesh : MonoBehaviour
    {
        public static void Save(GameObject roomMesh)
        {
            // ERROR TESTING - CREATEPREFAB CREATES PREFAB FROM A GAME OBJECT, IT DOESN'T ACTUALLY save it...?
            GameObject prefab = PrefabUtility.CreatePrefab(Constants.Folders.RoomMeshFolderPath + Constants.Names.RoomMeshFile, roomMesh);
            prefab.name = Constants.Names.RoomMeshFile;
            AssetDatabase.Refresh();
        }
    }
}
*/