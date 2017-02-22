using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    namespace UWB_Debug
    {
        public class Debug_FlipTexture : MonoBehaviour
        {
            public Texture2D Texture = null;
            public Texture2D HorizontalFlip = null;
            public Texture2D VerticalFlip = null;
            public Texture2D DiagonalFlip = null;

            private string texName = "";

            void Update()
            {
                if(Texture != null
                    && texName != Texture.name)
                {
                    texName = Texture.name;
                    
                    HorizontalFlip = FlipTexture.FlipHorizontal(Texture);
                    VerticalFlip = FlipTexture.FlipVertical(Texture);
                    DiagonalFlip = FlipTexture.FlipHorizontal(FlipTexture.FlipVertical(Texture));

                    HorizontalFlip.name = "HorizontalFlip";
                    VerticalFlip.name = "VerticalFlip";
                    DiagonalFlip.name = "DiagonalFlip";

                    SaveTexture.Save(HorizontalFlip, Constants.Suffixes.ImageSuffixTypes.PNG, Constants.Folders.ClippedRoomTextureFolderPath);
                    SaveTexture.Save(VerticalFlip, Constants.Suffixes.ImageSuffixTypes.PNG, Constants.Folders.ClippedRoomTextureFolderPath);
                    SaveTexture.Save(DiagonalFlip, Constants.Suffixes.ImageSuffixTypes.PNG, Constants.Folders.ClippedRoomTextureFolderPath);
                }
            }
        }
    }
}