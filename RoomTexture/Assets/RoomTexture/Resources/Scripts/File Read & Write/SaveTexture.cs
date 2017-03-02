using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UWB_RoomTexture
{
    public class SaveTexture : MonoBehaviour
    {
        /// <summary>
        /// Saves the passed in texture with the desired file type to the 
        /// folder path.
        /// 
        /// This ASSUMES that the path ends with the directory separator and 
        /// does NOT specify a file.
        /// </summary>
        /// <param name="tex">The texture to save.</param>
        /// <param name="fileType">The file format to save it in.</param>
        /// <param name="path">The folder path to save the texture file to.</param>
        public static void Save(Texture2D tex, Constants.Suffixes.ImageSuffixTypes imageSuffix, string path)
        {
            // Safeguard against non-alpha texture input (which is a 
            // workaround to make textures display properly.
            if(imageSuffix != Constants.Suffixes.ImageSuffixTypes.PNG)
            {
                throw new System.Exception(Constants.ErrorStrings.NonAlphaTextureFileTypeUsed);
            }

            tex.wrapMode = TextureWrapMode.Clamp;
            byte[] texBytes;
            string fileSuffix = "";

            // Store texture bytes in texBytes
            if(imageSuffix == Constants.Suffixes.ImageSuffixTypes.Jpeg)
            {
                texBytes = tex.EncodeToJPG();
                fileSuffix = Constants.Suffixes.FileSuffix_JPG;
            }
            else if (imageSuffix == Constants.Suffixes.ImageSuffixTypes.PNG)
            {
                texBytes = tex.EncodeToPNG();
                fileSuffix = Constants.Suffixes.FileSuffix_PNG;
            }
            else
            {
                throw new System.Exception(Constants.ErrorStrings.UnsupportedTextureFileFormat);
            }

            // ERROR TESTING REMOVE - ATTEMPTED TO AUTOMATE IMPORT SETTINGS - COULDN'T DO IT
            /*
            TextureImporter importer = new TextureImporter();
            TextureImporterSettings settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.isReadable = true;
            importer.SetTextureSettings(settings);
            AssetDatabase.WriteImportSettingsIfDirty(Constants.Folders.TextureFolderPath);
            AssetDatabase.WriteImportSettingsIfDirty(Constants.Folders.RoomTextureFolderPath);
            AssetDatabase.WriteImportSettingsIfDirty(Constants.Folders.ClippedRoomTextureFolderPath);
            */

            // Write texture data to file specified
            File.WriteAllBytes(path + tex.name + fileSuffix, texBytes);

            // ERROR TESTING REMOVE - ATTEMPTED TO AUTOMATE IMPORT SETTINGS - COULDN'T DO IT
            /*
            AssetImporter assetImporter = new AssetImporter();
            assetImporter.SaveAndReimport();
            */

#if UNITY_EDITOR
            // Immediately refresh / update files shown
            AssetDatabase.Refresh();
#endif
        }
    }
}