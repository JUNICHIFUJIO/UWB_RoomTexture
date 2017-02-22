using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace UWB_RoomTexture
{
    namespace UWB_Debug
    {
        public class Debug_LoadMaterial : MonoBehaviour
        {
            public string Load;

            // Use this for initialization
            void Start()
            {
                string pass = Debug_RoomTexture.Debug_TestPassedString;
                string fail = Debug_RoomTexture.Debug_TestFailString;

                string filepath = Debug_RoomTexture.Debug_MaterialFolderPath + Debug_RoomTexture.Debug_MaterialTestName + Constants.Suffixes.FileSuffix_Material;
                Load = (Test_Load(filepath)) ? pass : fail;
            }

            public static bool Test_Load(string filepath)
            {
                bool passed = true;

                try {
                    Material testMat = LoadMaterial.Load(filepath);
                    if (testMat == null
                        || !testMat.name.Equals(Debug_RoomTexture.Debug_MaterialTestName))
                    {
                        passed = false;
                    }
                }
                catch(DirectoryNotFoundException e)
                {
                    passed = false;
                }

                if (!passed)
                    Debug.Log(Debug_RoomTexture.Debug_TestFailString);

                return passed;
            }
        }
    }
}