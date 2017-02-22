using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace UWB_RoomTexture
{
    public class SaveTexturePrefab : MonoBehaviour
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

        public static void Save()
        {
            GameObject texturePrefab = GameObject.Find(Constants.Names.Holotoolkit_SpatialMappingName).transform.FindChild(Constants.Names.TexturePrefabName).gameObject;

            int numLines = numLines_HousingObject + numLines_Projectors * texturePrefab.transform.childCount;
            string[] fileLines = new string[numLines];

            int currentLineIndex = 0;
            TexturePrefabToStrings(texturePrefab, fileLines, currentLineIndex);

            File.WriteAllLines(Constants.Folders.FolderRoot + Constants.Folders.PrefabFolderPath + Constants.Names.TexturePrefabName + Constants.Suffixes.FileSuffix_Prefab, fileLines);
        }

        // Returns final line index
        private static void TexturePrefabToStrings(GameObject prefab, string[] fileLines, int currentLineIndex)
        {
            string indents = "";
            currentLineIndex = AddHousingObject(prefab, indents, fileLines, currentLineIndex);
            for(int i = 0; i < prefab.transform.childCount; i++)
            {
                currentLineIndex = AddProjector(prefab.transform.GetChild(i).GetComponent<Projector>(), indents + "\t", fileLines, currentLineIndex);
            }
        }

        private static int AddHousingObject(GameObject obj, string indents, string[] fileLines, int currentLineIndex)
        {
            fileLines[currentLineIndex++] = indents + tagOP + tag_HousingObject + tagEND;
            currentLineIndex = AddName(obj.name, indents + "\t", fileLines, currentLineIndex);
            currentLineIndex = AddLayer(LayerMask.LayerToName(obj.layer), indents + "\t", fileLines, currentLineIndex);
            currentLineIndex = AddTransform(obj.transform, indents + "\t", fileLines, currentLineIndex);
            fileLines[currentLineIndex++] = indents + tagOP_close + tag_HousingObject + tagEND;

            return currentLineIndex;
        }

        private static int AddProjector(Projector projector, string indents, string[] fileLines, int currentLineIndex)
        {
            fileLines[currentLineIndex++] = indents + tagOP + tag_Projector + tagEND;
            currentLineIndex = AddName(projector.name, indents + "\t", fileLines, currentLineIndex);
            currentLineIndex = AddTransform(projector.transform, indents + "\t", fileLines, currentLineIndex);
            currentLineIndex = AddTexture(projector.material.mainTexture.name, indents + "\t", fileLines, currentLineIndex);
            fileLines[currentLineIndex++] = indents + tagOP_close + tag_Projector + tagEND;

            return currentLineIndex;
        }

        private static int AddName(string name, string indents, string[] fileLines, int currentLineIndex)
        {
            fileLines[currentLineIndex++] = indents + tagOP + tag_Name + tagEND;
            fileLines[currentLineIndex++] = indents + "\t" + name;
            fileLines[currentLineIndex++] = indents + tagOP_close + tag_Name + tagEND;

            return currentLineIndex;
        }

        private static int AddLayer(string layerName, string indents, string[] fileLines, int currentLineIndex)
        {

            fileLines[currentLineIndex++] = indents + tagOP + tag_Layer + tagEND;
            fileLines[currentLineIndex++] = indents + "\t" + layerName;
            fileLines[currentLineIndex++] = indents + tagOP_close + tag_Layer + tagEND;
            return currentLineIndex;
        }

        // indents means how many indents it should be assumed the first line needs
        private static int AddTransform(Transform transform, string indents, string[] fileLines, int currentLineIndex)
        {
            fileLines[currentLineIndex++] = indents + tagOP + tag_Transform + tagEND;
            
            // Position
            fileLines[currentLineIndex++] = indents + "\t" + transform.position.x + separator + transform.position.y + separator + transform.position.z;
            // Rotation
            fileLines[currentLineIndex++] = indents + "\t" + transform.rotation.x + separator + transform.rotation.y + separator + transform.rotation.z + separator + transform.rotation.w;
            // Scale
            fileLines[currentLineIndex++] = indents + "\t" + transform.localScale.x + separator + transform.localScale.y + separator + transform.localScale.z;
            
            fileLines[currentLineIndex++] = indents + tagOP_close + tag_Transform + tagEND;

            return currentLineIndex;
        }

        private static int AddTexture(string textureName, string indents, string[] fileLines, int currentLineIndex)
        {
            fileLines[currentLineIndex++] = indents + tagOP + tag_Texture + tagEND;
            fileLines[currentLineIndex++] = indents + "\t" + textureName;
            fileLines[currentLineIndex++] = indents + tagOP_close + tag_Texture + tagEND;

            return currentLineIndex;
        }
    }
}
