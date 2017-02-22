using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    public class DisplayManager : MonoBehaviour
    {
        public static Texture2D Texture = null;
        public static GameObject Container = null;
        public static Projector SampleProjector = null;

        public static void UpdateDisplay(Texture2D tex, CameraLocation camLoc)
        {
            Texture = tex;
            Texture.name = tex.name;

            Container = new GameObject();
            SampleProjector = new Projector();
            CameraLocation.SetTransformValues(SampleProjector.transform, camLoc);
            CameraLocation.SetTransformValues(Container.transform, camLoc);
            SampleProjector.transform.parent = Container.transform;
        }

        public static void DestroyDisplay()
        {
            Texture = null;
            Destroy(Container);
            Container = null;
        }
    }
}