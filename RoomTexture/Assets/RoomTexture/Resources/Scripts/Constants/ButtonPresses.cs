using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    namespace Constants
    {
        public class ButtonPresses : MonoBehaviour
        {
            public static KeyCode TextureCaptureFinished = KeyCode.T;
            public static KeyCode Holotoolkit_LoadRoomMesh = GameObject.Find(Constants.Names.Holotoolkit_RemoteMappingName).GetComponent<HoloToolkit.Unity.FileSurfaceObserver>().LoadFileKey;
        }
    }
}