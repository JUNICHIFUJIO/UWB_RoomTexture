using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace UWB_RoomTexture
{
    namespace UWB_Debug
    {
        public class Debug_TextureCapture : MonoBehaviour
        {
            public bool Passed = false;
            public static int AutoTextureNumber = 0;
            public static string DebugMessage = "Tests have not run.";
            
            // Use this for initialization
            void Start()
            {
                string expectedFileName = Constants.Names.TextureAutoName + Constants.Names.TextureAutoSuffixInt;

                Test_GetNextAvailableAutoTextureNumber();
                if (!Test_BeginCapture())
                {
                    DebugMessage = "Capture failed to begin.";
                    Passed = false;
                }
                else if (!Test_EndCapture())
                {
                    DebugMessage = "Capture failed to end.";
                    Passed = false;
                }
                else if (!Test_Capture(expectedFileName))
                {
                    DebugMessage = "Capture failed (file not found). Expected filepath = " + expectedFileName;
                    Passed = false;
                }
                else
                {
                    Passed = true;
                }
            }

            public static void Test_GetNextAvailableAutoTextureNumber()
            {
                AutoTextureNumber = TextureCapture.GetNextAvailableAutoTextureNumber();
            }

            public static bool Test_BeginCapture()
            {
                bool passed = false;

                cleanUp();

                TextureCapture.BeginCapture();
                if (TextureCapture.photoCaptureObject != null)
                    passed = true;

                cleanUp();

                return passed;
            }

            public static bool Test_EndCapture()
            {
                bool passed = false;

                cleanUp();

                TextureCapture.BeginCapture();
                TextureCapture.EndCapture();
                if (TextureCapture.photoCaptureObject == null)
                {
                    passed = true;
                }

                cleanUp();

                return passed;
            }

            public static bool Test_Capture(string expectedFileName)
            {
                bool passed = false;

                cleanUp();

                TextureCapture.BeginCapture();
                TextureCapture.Capture();
                TextureCapture.EndCapture();

                if (Directory.Exists(expectedFileName))
                {
                    passed = true;
                }

                cleanUp();

                return passed;
            }

            private static void cleanUp()
            {
                if (TextureCapture.photoCaptureObject != null)
                {
                    TextureCapture.photoCaptureObject.Dispose();
                    TextureCapture.photoCaptureObject = null;
                }
            }
        }
    }
}