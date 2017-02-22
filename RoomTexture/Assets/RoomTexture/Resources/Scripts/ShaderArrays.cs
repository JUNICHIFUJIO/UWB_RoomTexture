using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace UWB_RoomTexture {
    public class ShaderArrays : MonoBehaviour {
        
        public static void GetShaderArrays(out Texture2DArray texArray, out Matrix4x4[] worldToCameraMatrixArray, out Matrix4x4[] projectionMatrixArray)
        {
            string[] clippedTexFiles = Directory.GetFiles(Constants.Folders.ClippedRoomTextureFolderPath);
            string[] cameraLocFiles = Directory.GetFiles(Constants.Folders.CameraLocationFolderPath);
            Resolution camRes = Constants.Camera.CameraResolution();
            Resolution downgradedRes = GetDowngradedResolution(camRes);

            int numLegitimateTextureFiles = 0;
            foreach(string texFile in clippedTexFiles)
            {
                string fileExtension = Path.GetExtension(texFile);
                if (!fileExtension.Equals(Constants.Suffixes.FileSuffix_PNG)
                    && !fileExtension.Equals(Constants.Suffixes.FileSuffix_JPG))
                    continue;

                ++numLegitimateTextureFiles;
            }

            if (clippedTexFiles.Length > 0)
                texArray = new Texture2DArray(downgradedRes.width, downgradedRes.height, numLegitimateTextureFiles, Constants.Camera.Format, false);
            else
                texArray = null;
            worldToCameraMatrixArray = new Matrix4x4[cameraLocFiles.Length];
            projectionMatrixArray = new Matrix4x4[cameraLocFiles.Length];
            
            int arrayIndex = 0;
            foreach (string texFile in clippedTexFiles)
            {
                string fileExtension = Path.GetExtension(texFile);
                if(!fileExtension.Equals(Constants.Suffixes.FileSuffix_PNG)
                    && !fileExtension.Equals(Constants.Suffixes.FileSuffix_JPG))
                {
                    continue;
                }
                
                // ERROR TESTING REMOVE
                // Adjust the texture filepath to ready it for loading by Resources.Load, which requires a relative path with NO file extension
                //string correctFilepath = Constants.Folders.ClippedRoomTextureFolderPath_Load + Path.GetFileNameWithoutExtension(texFile);

                Texture2D tex = LoadTexture.Load(texFile);
                Texture2D downgradedTex = DowngradeTexture(tex, downgradedRes);

                // copy texture into texture array
                texArray.SetPixels32(downgradedTex.GetPixels32(), arrayIndex);
                
                // ERROR TESTING REMOVE
                //Graphics.CopyTexture(downgradedTex, 0, 0, texArray, arrayIndex, 0);

                CameraLocation camLoc = CameraLocation.Load(Constants.Folders.CameraLocationFolderPath + FileNameTranslator.ClippedTextureToCameraLocation(Path.GetFileNameWithoutExtension(texFile)) + Constants.Suffixes.FileSuffix_CameraLocation);

                if (camLoc != null)
                {
                    worldToCameraMatrixArray[arrayIndex] = camLoc.WorldToCameraTransform;
                    projectionMatrixArray[arrayIndex] = camLoc.ProjectionTransform;
                }

                ++arrayIndex;
            }
        }

        /// <summary>
        /// Returns the next highest resolution with a width and height of a power of 2. This "power-of-2 requirement" is a specific requirement for using Texture2DArray.
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static Resolution GetDowngradedResolution(Resolution res)
        {
            int newWidth = res.width;
            int newHeight = res.height;

            int step = 1;
            while(step < res.width || step < res.height)
            {
                int nextStep = step * 2;
                if(step <= res.width 
                    && nextStep > res.width)
                {
                    newWidth = step;
                }
                if (step <= res.height
                    && nextStep > res.height)
                {
                    newHeight = step;
                }
                step = nextStep;
            }

            Resolution downgradedResolution = new Resolution();
            downgradedResolution.width = newWidth;
            downgradedResolution.height = newHeight;
            downgradedResolution.refreshRate = res.refreshRate;

            return downgradedResolution;
        }

        public static Texture2D DowngradeTexture(Texture2D tex, Resolution downgradedRes)
        {
            if (tex == null)
                return null;

            Texture2D downgradedTex = new Texture2D(tex.width, tex.height, Constants.Camera.Format, false);
            downgradedTex.SetPixels(tex.GetPixels());
            if(downgradedTex.Resize(downgradedRes.width, downgradedRes.height))
            {
                downgradedTex.Apply(false, false);

                return downgradedTex;
            }

            return null;
        }
    }
}