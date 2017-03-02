using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    namespace UWB_Debug
    {
        public class DebugText : MonoBehaviour
        {
            public static TextMesh textMesh = null;

            // Use this for initialization
            void Start()
            {
                textMesh = GameObject.Find("DebugTextObj").GetComponent<TextMesh>();
            }

            void OnEnable()
            {
                Application.logMessageReceived += DisplayDebugMessage;
            }

            void OnDisable()
            {
                Application.logMessageReceived -= DisplayDebugMessage;
            }

            public void DisplayDebugMessage(string message, string stackTrace, LogType type)
            {
                if(textMesh.text.Length > 300)
                {
                    textMesh.text = message + "\n";
                }
                else
                {
                    textMesh.text += message + "\n";
                }
            }
        }
    }
}