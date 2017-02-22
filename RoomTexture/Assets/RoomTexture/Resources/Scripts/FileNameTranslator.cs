using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture {
    public class FileNameTranslator : MonoBehaviour
    {

        public static string TextureToClippedTexture(string textureName)
        {
            return textureName;
        }

        public static string ClippedTextureToTexture(string clippedTextureName)
        {
            return clippedTextureName;
        }

        // Append the Material prefix to the front
        public static string ClippedTextureToMaterial(string clippedTextureName)
        {
            return Constants.Names.Projector_MaterialAutoName + clippedTextureName;
        }

        // Remove the Material prefix from the front
        public static string MaterialToClippedTexture(string materialName)
        {
            return RemovePrefix(materialName);
        }

        // Append the CameraLocation prefix to the front
        public static string ClippedTextureToCameraLocation(string clippedTextureName)
        {
            return Constants.Names.CameraLocationAutoName + clippedTextureName;
        }

        // Remove the CameraLocation prefix from the front
        public static string CameraLocationToClippedTexture(string cameraLocationName)
        {
            return RemovePrefix(cameraLocationName);
        }

        public static string ClippedTextureToProjector(string clippedTextureName)
        {
            return Constants.Names.Projector_AutoName + clippedTextureName;
        }

        public static string ProjectorToClippedTexture(string projectorName)
        {
            return RemovePrefix(projectorName);
        }

        private static string RemovePrefix(string str)
        {
            string result = str;

            string[] nameComponents = str.Split('_');
            if (nameComponents.Length > 1)
            {
                result = nameComponents[1];
                for (int i = 2; i < nameComponents.Length; i++)
                {
                    result += nameComponents[i];
                }
            }

            return result;
        }
        
        private static string RemoveSuffix(string str)
        {
            string result = str;

            string[] nameComponents = str.Split('_');
            if(nameComponents.Length > 1)
            {
                result = nameComponents[0];
                for(int i = 1; i < nameComponents.Length - 1; i++)
                {
                    result += nameComponents[i];
                }
            }

            return result;
        }
    }
}