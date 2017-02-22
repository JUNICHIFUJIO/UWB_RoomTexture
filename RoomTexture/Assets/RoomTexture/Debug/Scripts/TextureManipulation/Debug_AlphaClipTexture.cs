using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    namespace UWB_Debug
    {
        public class Debug_AlphaClipTexture : MonoBehaviour
        {
            public Texture2D Texture = null;
            public Texture2D AlphaClipped = null;
            public Vector2 TextureDimensions = new Vector2();
            public Vector2 ClippedVisibleDimensions = new Vector2();

            private string texName = "";

            void Update()
            {
                if(Texture != null
                    && !texName.Equals(Texture.name))
                {
                    texName = Texture.name;
                    AlphaClipped = AlphaClipTexture.AlphaClip(Texture);

                    int height = 0;
                    int width = 0;

                    for(int y = 0; y < AlphaClipped.height / 2; y++)
                    {
                        if(AlphaClipped.GetPixel(AlphaClipped.width / 2, y).a != 0)
                        {
                            height = AlphaClipped.height - y * 2;
                            break;
                        }
                    }
                    for(int x = 0; x < AlphaClipped.width; x++)
                    {
                        if(AlphaClipped.GetPixel(x, AlphaClipped.height / 2).a != 0)
                        {
                            width = AlphaClipped.width - x * 2;
                            break;
                        }
                    }

                    TextureDimensions.x = Texture.width;
                    TextureDimensions.y = Texture.height;
                    ClippedVisibleDimensions.x = width;
                    ClippedVisibleDimensions.y = height;
                }
            }
        }
    }
}