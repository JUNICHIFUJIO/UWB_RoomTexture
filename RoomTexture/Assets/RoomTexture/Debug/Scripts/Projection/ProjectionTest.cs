﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // For getting filepath of Texture pushed in
using System.IO;

namespace UWB_RoomTexture
{
    namespace UWB_Debug
    {
        public class ProjectionTest : MonoBehaviour
        {
            public Texture2D Texture = null;
            public string CameraLocationFileName = "";

            private string lastTextureLoaded = "";
            private string lastCameraLoadedName = "";
            private CameraLocation lastCameraLoaded = null;

            private GameObject tempObj = null;

            void FixedUpdate()
            {
                if (Texture != null
                    && !Texture.name.Equals(lastTextureLoaded))
                {
                    if (tempObj != null)
                        Destroy(tempObj);

                    lastTextureLoaded = Texture.name;

                    string textureFilePath = AssetDatabase.GetAssetPath(Texture);
                    Texture.wrapMode = TextureWrapMode.Clamp;

                    tempObj = new GameObject();
                    tempObj.name = "Temp Projector Object";

                    if (!CameraLocationFileName.Equals(string.Empty)
                        && lastCameraLoadedName.Equals(CameraLocationFileName))
                    {
                        lastCameraLoadedName = CameraLocationFileName;

                        lastCameraLoaded = CameraLocation.Load(CameraLocationFileName);

                        TextureProjector.GenerateProjector(tempObj, CameraLocationFileName, textureFilePath);
                    }
                    else
                    {
                        Projector proj = tempObj.AddComponent<Projector>();
                        proj.material = MaterialMaker.GenerateRoomMaterial(Texture);
                    }
                }
            }
        }
    }
}