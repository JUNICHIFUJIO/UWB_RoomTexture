using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    public class FlipTexture : MonoBehaviour
    {
        public static Texture2D FlipHorizontal(Texture2D tex)
        {
            // Safeguard against bad input
            if (tex == null
                || tex.width < 1
                || tex.height < 1)
                return null;
            if (tex.width == 1)
                return tex;

            // Initialize a new texture canvas to house the flipped texture
            Texture2D flippedTexture = new Texture2D(
                tex.width,
                tex.height,
                TextureFormat.RGBA32,
                false
                );
            
            // Get flipped pixels into an array
            Color[] origPixels = tex.GetPixels();
            //Color[] flippedPixels = new Color[origPixels.Length];

            Color[] flippedPixels = tex.GetPixels();

            for(int y = 0; y < tex.height; y++)
            {
                for(int x = 0; x < tex.width; x++)
                {
                    int destinationPixelIndex = (y * tex.width) + tex.width - x - 1;
                    int originalPixelIndex = y * tex.width + x;
                    
                    flippedPixels[destinationPixelIndex] = origPixels[originalPixelIndex];
                }
            }

            flippedTexture.SetPixels(flippedPixels);
            return flippedTexture;
        }
        
        public static Texture2D FlipVertical(Texture2D tex)
        {
            // Safeguard against bad input
            if (tex == null
                || tex.width < 1
                || tex.height < 1)
                return null;
            if (tex.height == 1)
                return tex;

            // Initialize a new texture canvas to house the flipped texture
            Texture2D flippedTexture = new Texture2D(
                tex.width,
                tex.height,
                TextureFormat.RGBA32,
                false
                );

            // Get flipped pixels into an array
            Color[] origPixels = tex.GetPixels();
            Color[] flippedPixels = new Color[origPixels.Length];
            for(int y = 0; y < tex.height; y++)
            {
                for(int x = 0; x < tex.width; x++)
                {
                    int destinationPixelIndex = (tex.height - y - 1) * tex.width + x;
                    int originalPixelIndex = y * tex.width + x;

                    flippedPixels[destinationPixelIndex] = origPixels[originalPixelIndex];
                }
            }

            flippedTexture.SetPixels(flippedPixels);
            return flippedTexture;
        }
    }
}