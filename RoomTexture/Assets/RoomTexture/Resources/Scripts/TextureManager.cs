using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace UWB_RoomTexture
{
    public class TextureManager : MonoBehaviour
    {
        #region Events
        public delegate void RoomMeshLoadedEventHandler();
        public event RoomMeshLoadedEventHandler RoomMeshLoaded;
        public delegate void TextureCaptureFinishedEventHandler();
        public event TextureCaptureFinishedEventHandler TextureCaptureFinished;
        #endregion

        // Use this for initialization
        void Awake()
        {
            // Enable Hololens commands for starting and stopping texture capture
            Interactions.EnableVoiceCommand(Constants.Commands.VoiceCommandsEnum.BeginRoomTexture);
            Interactions.EnableVoiceCommand(Constants.Commands.VoiceCommandsEnum.EndRoomTexture);

            RoomMeshLoaded += TextureManager_RoomMeshLoaded;
            TextureCaptureFinished += TextureManager_TextureCaptureFinished;

            CentralProcessor.UpdateRenderMethodUsed(true);
        }
        
        private void TextureManager_RoomMeshLoaded()
        {
            // Waiting(Constants.Thresholds.ExpectedTimeToLoadRoomMesh + 1);
            // LayerSwitcher.ChangeMeshLayer();
            //CentralProcessor.SetMaterials();
            StartCoroutine(RoomMeshLoaded_Coroutine());
        }

        public void Trigger_TextureCaptureFinished()
        {
            TextureCaptureFinished.Invoke();
        }

        // ERROR TESTING only for debugging
        public void Trigger_RoomMeshLoaded()
        {
            StartCoroutine(RoomMeshLoaded_Coroutine());

            // ERROR TESTING REMOVE
            //LayerSwitcher.ChangeMeshLayer();
            //CentralProcessor.SetMaterials();
        }

        private IEnumerator RoomMeshLoaded_Coroutine()
        {            
            yield return new WaitForSeconds(Constants.Thresholds.ExpectedTimeToLoadRoomMesh);
            
            LayerSwitcher.ChangeMeshLayer();
            CentralProcessor.SetMaterials();
        }

        private void TextureManager_TextureCaptureFinished()
        {
            CentralProcessor.GenerateTexturePrefab();
            CentralProcessor.UpdateRenderMethodUsed();
        }

        void Update()
        {
            if(Input.GetKeyDown(Constants.ButtonPresses.TextureCaptureFinished))
            {
                TextureCaptureFinished.Invoke();
            }
            if(Input.GetKeyDown(Constants.ButtonPresses.Holotoolkit_LoadRoomMesh))
            {
                RoomMeshLoaded.Invoke();
            }
        }
    }
}