/// A script that creates a copy of a texture, but all of its edge pixels
/// will have an alpha value of 0 (i.e. they are transparent).
/// 
/// This script is a workaround for Unity's projectors which assume that
/// projected textures will have an alpha falloff, else it will project 
/// the texture across the entire mesh it collides with.
/// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    public class AlphaClipTexture : MonoBehaviour
    {
        public static Texture2D AlphaClip(Texture2D tex)
        {
            // Safeguard against bad input
            bool debug = Constants.DebugStrings.DebugFlag;
            if(tex == null)
            {
                if(debug)
                    Debug.Log("AlphaClip() encountered a null texture.");

                throw new System.Exception(Constants.ErrorStrings.NullTexture);
            }

            // Run method
            if (debug)
            {
                // Display startup of method
                Debug.Log("Starting AlphaClip() method.");

                // Display texture info
                if (tex != null)
                    Debug.Log("AlphaClip() received texture\n"
                        + "\tName: " + tex.name + "\n"
                        + "\tWidth: " + tex.width + "\n"
                        + "\tHeight: " + tex.height + "\n");
            }

            // Texture format options limited because of Unity method
            Texture2D clippedTex = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false, false);
            // Prevent texture from tiling in projector
            clippedTex.wrapMode = TextureWrapMode.Clamp;

            // Adjust the pixels
            Color[] pixels = tex.GetPixels();

            // Make horizontal edge pixels transparent
            for (int i = 0; i < clippedTex.width; i++)
            {
                // Overwrite the bottom row of pixels
                int bottomRowPixel = i;
                pixels[bottomRowPixel] = new Color(
                    pixels[bottomRowPixel].r, // Same red
                    pixels[bottomRowPixel].g, // Same green
                    pixels[bottomRowPixel].b, // Same blue
                    0                         // Make pixel transparent
                    );
                
                // Overwrite the top row of pixels
                int topRowPixel = clippedTex.width * (clippedTex.height - 1) + i;
                pixels[topRowPixel] = new Color(
                    pixels[topRowPixel].r, // Same red
                    pixels[topRowPixel].g, // Same green
                    pixels[topRowPixel].b, // Same blue
                    0                      // Make pixel transparent
                    );
            }

            // Make vertical edge pixels transparent
            for (int j = 0; j < clippedTex.height; j++)
            {
                //Debug.Log("j = " + j + "; index = " + (j * tex.width + tex.width - 1).ToString());

                // Overwrite the left column of pixels
                int leftColumnPixel = j * clippedTex.width;
                pixels[leftColumnPixel] = new Color(
                    pixels[leftColumnPixel].r, // Same red
                    pixels[leftColumnPixel].g, // Same green
                    pixels[leftColumnPixel].b, // Same blue
                    0                          // Make pixel transparent
                    );
                
                // Overwrite the right column of pixels
                int rightColumnPixel = j * clippedTex.width + clippedTex.width - 1;
                pixels[rightColumnPixel] = new Color(
                    pixels[rightColumnPixel].r, // Same red
                    pixels[rightColumnPixel].g, // Same green
                    pixels[rightColumnPixel].b, // Same blue
                    0                           // Make pixel transparent
                    );
            }

            // Set adjusted colors for new texture
            clippedTex.SetPixels(pixels);
            // Adjust name of texture to desired name
            clippedTex.name = FileNameTranslator.TextureToClippedTexture(tex.name);

            // ERROR TESTING REMOVE
            //clippedTex.name = tex.name;

            // Save the texture
            SaveTexture.Save(clippedTex, Constants.Suffixes.ImageSuffixTypes.PNG, Constants.Folders.ClippedRoomTextureFolderPath);

            if (debug)
            {
                Debug.Log("AlphaClip() has saved the clipped texture to " + Constants.Folders.ClippedRoomTextureFolderPath + ".");
            }

            // Reload the Unity Asset database
            UnityEditor.AssetDatabase.Refresh();

            if (debug)
            {
                Debug.Log("AlphaClip() has refreshed the Asset database to show the new clipped texture.");
            }

            return clippedTex;
        }
    }


}
