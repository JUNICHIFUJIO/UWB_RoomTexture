using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// ERROR TESTING - MUST MAKE LAYER APPROPRIATELY BEFORE LOADING OBJECT
namespace UWB_RoomTexture
{
    public class LoadTexturePrefab : MonoBehaviour
    {
        public static char tagOP = '<';
        public static char tagEND = '>';
        public static string tagOP_close = "</";
        public static char separator = ',';
        public static string tag_Name = "name";
        public static string tag_Layer = "layer";
        public static string tag_HousingObject = "object";
        public static string tag_Transform = "transform";
        public static string tag_Projector = "projector";
        public static string tag_Texture = "texture";

        public static int numLines_HousingObject = 13;
        public static int numLines_Projectors = 13;

        public static GameObject Load()
        {
            GameObject texturePrefab = new GameObject();

            string[] fileLines = File.ReadAllLines(Constants.Folders.PrefabFolderPath + Constants.Names.TexturePrefabName + Constants.Suffixes.FileSuffix_Prefab);
            int numProjectors = (fileLines.Length - numLines_HousingObject) / numLines_Projectors;
            int currentLineIndex = 0;

            currentLineIndex = ReadHousingObject(texturePrefab, fileLines, currentLineIndex);
            for(int i = 0; i < numProjectors; i++)
            {
                // Generate a projector
                Projector projector = new Projector();
                string textureName;
                currentLineIndex = ReadProjector(projector, out textureName, fileLines, currentLineIndex);
                
                // Generate a material from the projector texture
                // ERROR TESTING DON'T MAKE THIS JUST PNG
                Texture2D tex = LoadTexture.Load(Constants.Folders.ClippedRoomTextureFolderPath + textureName + Constants.Suffixes.FileSuffix_PNG);
                Material mat = MaterialMaker.GenerateRoomMaterial(tex);
                SaveMaterial.Save(mat);
                projector.material = mat;

                // Add the projector to the texture prefab
                GameObject projectorHolder = new GameObject();
                projectorHolder.name = projector.name;
                projector.transform.parent = projectorHolder.transform;
            }

            return texturePrefab;
        }

        private static int ReadHousingObject(GameObject texturePrefab, string[] fileLines, int currentLineIndex)
        {
            string name;
            string layerName;

            ++currentLineIndex;
            // Get name
            currentLineIndex = ReadName(out name, fileLines, currentLineIndex);
            texturePrefab.name = name;
            // Get Layer
            currentLineIndex = ReadLayer(out layerName, fileLines, currentLineIndex);
            int layerID = LayerMask.NameToLayer(layerName);
            if (layerID < 0)
            {
                throw new System.Exception(Constants.ErrorStrings.RoomTextureLayerNotFound);
            }
            // Get Transform
            currentLineIndex = ReadTransform(texturePrefab.transform, fileLines, currentLineIndex);
            ++currentLineIndex;
            
            return currentLineIndex;
        }

        private static int ReadProjector(Projector projector, out string textureName, string[] fileLines, int currentLineIndex)
        {
            string name;

            ++currentLineIndex;
            // Get name
            currentLineIndex = ReadName(out name, fileLines, currentLineIndex);
            projector.name = name;
            // Get transform
            currentLineIndex = ReadTransform(projector.transform, fileLines, currentLineIndex);
            // Get texture
            currentLineIndex = ReadTexture(out textureName, fileLines, currentLineIndex);
            ++currentLineIndex;

            return currentLineIndex;
        }

        private static int ReadName(out string name, string[] fileLines, int currentLineIndex)
        {
            ++currentLineIndex;
            name = fileLines[currentLineIndex++].Trim();
            ++currentLineIndex;

            return currentLineIndex;
        }

        private static int ReadLayer(out string layerName, string[] fileLines, int currentLineIndex)
        {
            ++currentLineIndex;
            layerName = fileLines[currentLineIndex++].Trim();
            ++currentLineIndex;

            return currentLineIndex;
        }

        private static int ReadTransform(Transform transform, string[] fileLines, int currentLineIndex)
        {
            ++currentLineIndex;
            string[] positionStrings = fileLines[currentLineIndex++].Split(separator);
            transform.position = new Vector3(float.Parse(positionStrings[0]), float.Parse(positionStrings[1]), float.Parse(positionStrings[2]));
            string[] rotationStrings = fileLines[currentLineIndex++].Split(separator);
            transform.rotation = new Quaternion(float.Parse(rotationStrings[0]), float.Parse(rotationStrings[1]), float.Parse(rotationStrings[2]), float.Parse(rotationStrings[3]));
            string[] scaleStrings = fileLines[currentLineIndex++].Split(separator);
            transform.localScale = new Vector3(float.Parse(scaleStrings[0]), float.Parse(scaleStrings[1]), float.Parse(scaleStrings[2]));
            ++currentLineIndex;

            return currentLineIndex;
        }

        private static int ReadTexture(out string textureName, string[] fileLines, int currentLineIndex)
        {
            ++currentLineIndex;
            textureName = fileLines[currentLineIndex++].Trim();
            ++currentLineIndex;

            return currentLineIndex;
        }
    }
}
