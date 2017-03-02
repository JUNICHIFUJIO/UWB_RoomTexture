using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace UWB_RoomTexture
{
    namespace UWB_Debug
    {
        public class Debug_LoadMesh : MonoBehaviour
        {
            // ERROR TESTING REMOVE THIS IN FINAL DEBUG RUN
            /*
            public string Load;

            // Use this for initialization
            void Start()
            {
                string pass = Debug_RoomTexture.Debug_TestPassedString;
                string fail = Debug_RoomTexture.Debug_TestFailString;

                string filepath = Debug_RoomTexture.Debug_MeshFolderPath + Debug_RoomTexture.Debug_MeshTestName + Constants.Suffixes.RoomMeshSuffix;
                Load = (Test_Load(filepath)) ? pass : fail;
            }

            public static bool Test_Load(string filepath)
            {
                bool passed = true;

                try
                {
                    GameObject obj = LoadRoomMesh.Load(filepath);

                    if (obj == null)
                        passed = false;
                }
                catch (System.Exception e)
                {
                    passed = false;
                }

                if (!passed)
                    Debug.Log(Debug_RoomTexture.Debug_TestFailString);

                return passed;
            }
            */
        }
    }
}