using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace UWB_RoomTexture
{
    namespace UWB_Debug
    {
        public class Debug_LoadTexture : MonoBehaviour
        {
            public string Load_PNG;
            public string Load_IncorrectFormat;

            // Use this for initialization
            void Start()
            {
                string pass = Debug_RoomTexture.Debug_TestPassedString;
                string fail = Debug_RoomTexture.Debug_TestFailString;

                string filepath = Debug_RoomTexture.Debug_TextureFolderPath + Debug_RoomTexture.Debug_TextureTestName + Constants.Suffixes.FileSuffix_PNG;
                string filepath_Bad = Debug_RoomTexture.Debug_TextureFolderPath + Debug_RoomTexture.Debug_TextureTestName + Constants.Suffixes.FileSuffix_JPG;
                Load_PNG = (Test_Load(filepath)) ? pass : fail;
                Load_IncorrectFormat = (!Test_Load(filepath_Bad)) ? pass : fail;
            }

            public static bool Test_Load(string filepath)
            {
                bool passed = true;

                try
                {
                    Texture2D tex = LoadTexture.Load(filepath);

                    if (tex == null)
                        passed = false;
                }
                catch (DirectoryNotFoundException e)
                {
                    passed = false;
                    Debug.Log(Debug_RoomTexture.Debug_TestFailString);
                }
                catch (System.Exception e)
                {
                    passed = false;

                    if (e.Message != Constants.ErrorStrings.UnsupportedTextureFileFormat)
                    {
                        Debug.Log(Debug_RoomTexture.Debug_TestFailString);
                    }
                }

                return passed;
            }
        }
    }
}