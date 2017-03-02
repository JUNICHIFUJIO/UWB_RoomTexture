// ERROR TESTING - Maybe USE ONLY SETLAYER, NOT CHANGEMESHLAYER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture {
    public class LayerSwitcher : MonoBehaviour {

        public GameObject Item = null;
        public string RoomTextureLayer = ""; // meant to be a constant, but not sure exactly how to make it so in Unity

        #region Events
        public delegate void ItemReceivedEventHandler();
        public event ItemReceivedEventHandler ItemReceived;
        
        private void LayerSwitcher_ItemReceived()
        {
            if (Item != null)
            {
                SetLayer(Item, Constants.Names.LayerName);
                RoomTextureLayer = Constants.Names.LayerName;
                Item = null;
            }
        }
        #endregion
        
        /// <summary>
        /// Initialize the class's constants.
        /// </summary>
        void Awake()
        {
            ItemReceived += LayerSwitcher_ItemReceived;
            
            RoomTextureLayer = Constants.Names.LayerName;

            // Unity has no way to dynamically create layers
            // User must create the layer before using this
            // If the layer doesn't exist for the scene, throw an 
            // error that doesn't get caught by anything and tells
            // users what to do
            int layerID = LayerMask.NameToLayer(Constants.Names.LayerName);
            if (layerID < 0)
            {
                // ERROR TESTING REINSTANTE WITH NON-ROOMTEXTURE reference
                //throw new System.Exception(RoomTexture.Error_RoomTextureLayerNotFound);
            }
        }
        
        void Update()
        {
            if (Item != null)
            {
                // Automatically update the item's layer
                ItemReceived.Invoke();
            }
        }

        /// <summary>
        /// Sets the layer of the GameObject pushed in with the passed-in
        /// layer name. Assumes you want to change the layers of children,
        /// but can be adjusted with bool parameter.
        /// </summary>
        /// <param name="item">The item whose layer will change.</param>
        /// <param name="layerName">The name of the layer that will be set.</param>
        /// <param name="setChildLayer">Whether to recursively set ALL children's layers to be the specified layer.</param>
        public static void SetLayer(GameObject item, string layerName, bool setChildLayer = true)
        {
            if(item != null)
            {
                item.layer = LayerMask.NameToLayer(layerName);
                
                for (int i = 0; i < item.transform.childCount; i++)
                {
                    Transform child = item.transform.GetChild(i);
                    SetLayer(child.gameObject, layerName);
                }
            }
        }
        
        /// <summary>
        /// Change the layer mask for any mesh/submeshes attached to an existing SpatialMapping object.
        /// NOTE: This assumes the mesh is ALREADY ATTACHED to the spatial mapping object and that there is only one layer of children.
        /// </summary>
        public static void ChangeMeshLayer()
        {
            // ERROR TESTING REMOVE
            //GameObject spatialMapper = GameObject.Find("SpatialMapping");
            GameObject remoteMapper = GameObject.Find(Constants.Names.Holotoolkit_RemoteMappingName);

            ChangeMeshLayer(remoteMapper);

            //ERROR TESTING REMOVE
            //if (remoteMapper != null)
            //{
            //    SetLayer(remoteMapper, Constants.Names.LayerName);

            //    for (int i = 0; i < remoteMapper.transform.childCount; i++)
            //    {
            //        Transform child = remoteMapper.transform.GetChild(i);
            //        ChangeMeshLayer(child.gameObject);
            //    }
            //}
        }

        /// <summary>
        /// Recursive method to change mesh/submesh layer mask for non-SpatialMapping meshes. (i.e. custom meshes saved outside of SpatialMapping and manipulated in the editor)
        /// </summary>
        /// <param name="mesh"></param>
        public static void ChangeMeshLayer(GameObject mesh)
        {
            if (mesh != null)
            {
                SetLayer(mesh, Constants.Names.LayerName);

                // Set all child object's layers
                for (int i = 0; i < mesh.transform.childCount; i++)
                {
                    Transform child = mesh.transform.GetChild(i);
                    ChangeMeshLayer(child.gameObject);
                }
            }
        }
    }
}
