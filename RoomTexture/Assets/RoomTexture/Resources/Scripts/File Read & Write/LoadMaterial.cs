using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace UWB_RoomTexture
{
    public class LoadMaterial : MonoBehaviour
    {

        public static Material Load(string filepath)
        {
            Material mat = null;

            if (Directory.Exists(Path.GetDirectoryName(filepath)))
            {
                // Check that it's a supported material file
                string fileExtension = Path.GetExtension(filepath);
                if (fileExtension != Constants.Suffixes.FileSuffix_Material)
                {
                    throw new System.Exception(Constants.ErrorStrings.UnsupportedMaterialFileFormat);
                }

                mat = Resources.Load(filepath) as Material;
                mat.name = Path.GetFileNameWithoutExtension(filepath);
            }
            else
            {
                throw new DirectoryNotFoundException();
            }

            return mat;
        }
    }
}