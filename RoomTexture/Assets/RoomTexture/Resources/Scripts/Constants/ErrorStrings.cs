using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    namespace Constants
    {
        public class ErrorStrings : MonoBehaviour
        {
            public static string HololensKeyphraseNotFound = "Hololens Keyphrase not found.";
            public static string LocatableCameraLocationNotFound = "Unity locatable camera cannot retrieve location data for image taken.";
            public static string CameraResolutionNotFoundForFOV = "Camera resolution specified not found when trying to determine horizontal field of view for projector.";
            public static string RoomTextureLayerNotFound = "Room Texture's layer was not found. Please ensure that there is a layer in the Unity editor with the exact same name as set by the string variable \"LayerName\" in the \"RoomTexture\" script.";
            public static string GestureNotFound = "Gesture not found.";


            public static string NullTexture = "Null Texture encountered.";
            public static string NonAlphaTextureFileTypeUsed =
                "This project manipulates and utilizes alpha values "
                + "and will only allow filetypes that support them to "
                + "be saved as textures. If the filetype passed in "
                + "supports alpha values, please reference Save"
                + "() in the SaveTexture class.";
            public static string UnsupportedTextureFileFormat =
                "Encountered unsupported file format. "
                + "You may need to alter method to use this type of "
                + "file format. Without alpha values, this file format "
                + "may be incompatible with this asset's internal workings.";
            public static string UnsupportedMaterialFileFormat =
                "Encountered unsupported material file format. File format should be .png";
            public static string UnsupportedPrefabFileType =
                "Encountered incorrect prefab file format.";
        }
    }
}