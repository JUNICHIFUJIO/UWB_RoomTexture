using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace UWB_RoomTexture
{
    public class LoadTexture : MonoBehaviour
    {
        public static Texture2D Load(string filepath)
        {
            Texture2D tex = null;

            if (Directory.Exists(Path.GetDirectoryName(filepath)))
            {
                // Check that it's a supported texture file
                string fileExtension = Path.GetExtension(filepath);
                if (fileExtension != Constants.Suffixes.FileSuffix_JPG
                    && fileExtension != Constants.Suffixes.FileSuffix_PNG)
                {
                    throw new System.Exception(Constants.ErrorStrings.UnsupportedTextureFileFormat);
                }

                // Convert the filepath into one that Unity recognizes via its Resources.Load method
                // i.e. strip everything before the "Resources" folder and remove the file extension
                string correctedFilepath = filepath.Remove(0, Constants.Folders.FolderRoot.Length);
                string[] correctedFilepathComponents = correctedFilepath.Split('.');
                correctedFilepath = correctedFilepathComponents[0];

                // ERROR TESTING REMOVE
                //TextureImporter importer = new TextureImporter();
                //TextureImporterSettings settings = new TextureImporterSettings();
                //settings.wrapMode = TextureWrapMode.Clamp;
                //settings.readable = true;
                //importer.SetTextureSettings(settings);
                //AssetDatabase.WriteImportSettingsIfDirty(Constants.Folders.TextureFolderPath);
                //AssetDatabase.WriteImportSettingsIfDirty(Constants.Folders.RoomTextureFolderPath);
                //AssetDatabase.WriteImportSettingsIfDirty(Constants.Folders.ClippedRoomTextureFolderPath);
                
                tex = Resources.Load(correctedFilepath) as Texture2D;
                //AssetDatabase.ImportAsset(filepath, ImportAssetOptions.ForceUpdate);
                if (tex != null)
                {
                    tex.name = Path.GetFileNameWithoutExtension(filepath);
                }
            }
            else
            {
                throw new DirectoryNotFoundException();
            }

            return tex;
        }
    }
}