using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // For saving the material into the editor 
// ERROR TESTING - WILL THIS CAUSE ISSUES OUTSIDE OF THE EDITOR (i.e. WHEN THE APPLICATION IS MADE FROM THIS PROGRAM?)

namespace UWB_RoomTexture
{
    public class MaterialMaker : MonoBehaviour
    {
        public static Material GenerateRoomCanvasMaterial()
        {
            Material mat = new Material(Shader.Find(Constants.Shaders.RoomBackground));
            mat.name = Constants.Names.CanvasMaterial;

            return mat;
        }

        public static Material GenerateRoomMaterial(Texture2D tex)
        {
            Material mat = new Material(Shader.Find(Constants.Shaders.RoomTexture));
            mat.SetTexture("_ProjectionTexture", tex);
            mat.name = FileNameTranslator.ClippedTextureToMaterial(tex.name);

            return mat;
        }

        public static Material GenerateRoomMaterial_NonProjector()
        {
            Texture2DArray texArray;
            Matrix4x4[] worldToCameraArray;
            Matrix4x4[] projectionArray;
            ShaderArrays.GetShaderArrays(out texArray, out worldToCameraArray, out projectionArray);

            Material mat = new Material(Shader.Find(Constants.Shaders.RoomTexture_NonProjector));
            mat.SetTexture("_TextureArray", texArray);
            mat.SetMatrixArray("_WorldToCameraMatrixArray", worldToCameraArray);
            mat.SetMatrixArray("_CameraProjectionMatrixArray", projectionArray);

            return mat;
        }
    }
}
