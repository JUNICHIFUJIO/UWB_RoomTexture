using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    namespace UWB_Debug
    {
        public class RemoteMappingCheck : MonoBehaviour
        {
            public bool RoomLoaded = false;
            public bool LayersSet = false;
            public string LayerName = "";
            public GameObject MiscMesh = null;
            public bool MeshLayersSet = false;
            public string MeshLayer = "";

            private bool roomLoading = false;
            private string lastObjUsed = "";

            void Update()
            {
                if (!RoomLoaded 
                    && !roomLoading
                    && Input.GetKeyDown(Constants.ButtonPresses.Holotoolkit_LoadRoomMesh))
                {
                    roomLoading = true;

                    GameObject.Find("TextureManager").GetComponent<TextureManager>().Trigger_RoomMeshLoaded();
                    // ERROR TESTING REMOVE
                    //TextureManager.Trigger_RoomMeshLoaded();

                    GameObject remoteMapper = GameObject.Find(Constants.Names.Holotoolkit_RemoteMappingName);
                    if (remoteMapper != null)
                    {
                        RoomLoaded = true;
                        LayersSet = checkLayers(remoteMapper);
                        LayerName = getLayerName(remoteMapper);
                    }

                    roomLoading = false;
                }

                if (MiscMesh != null
                    && !lastObjUsed.Equals(MiscMesh.name))
                {
                    lastObjUsed = MiscMesh.name;
                    MeshLayersSet = checkLayers(MiscMesh);
                    MeshLayer = getLayerName(MiscMesh);
                }
            }

            private static bool checkLayers(GameObject obj)
            {
                bool layerIsGood = LayerMask.LayerToName(obj.layer).Equals(Constants.Names.LayerName);

                if (!layerIsGood)
                    return false;

                for(int i = 0; i < obj.transform.childCount; i++)
                {
                    if (!checkLayers(obj.transform.GetChild(i).gameObject))
                        return false;
                }

                return true;
            }

            private static string getLayerName(GameObject obj)
            {
                return LayerMask.LayerToName(obj.layer);
            }

            // ERROR TESTING REMOVE
            //private static void Waiting(int time)
            //{
            //    if (time > 20)
            //        time = 20;
            //    float startTime = Time.fixedTime;
            //    while (Time.fixedTime - startTime < time * Time.fixedDeltaTime) { }
            //}
        }
    }
}