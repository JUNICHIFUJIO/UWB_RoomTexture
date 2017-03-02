using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor; // For saving assets
#endif

namespace UWB_RoomTexture
{
    public class SaveMaterial : MonoBehaviour
    {
        public static void Save(Material material)
        {
            // Safeguard against bad input
            if (material != null)
            {
#if UNITY_EDITOR
                // ERROR TESTING - NOT USED IN FINAL VERSION - MIGHT AS WELL KEEP AROUND FOR FUTURE DESIRED USE?
                AssetDatabase.CreateAsset(
                    material, // The material to be saved

                    Constants.Folders.AssetFolderRoot
                    + Constants.Folders.MaterialFolderPath
                    + material.name
                    + Constants.Suffixes.FileSuffix_Material // Material filepath
                    );

                    // ERROR TESTING REMOVE - old parameters to material saving
                    //material, Constants.Folders.MaterialFolderPath + material.name + Constants.Suffixes.FileSuffix_Material);
                AssetDatabase.SaveAssets();
#endif
            }
        }
    }
}