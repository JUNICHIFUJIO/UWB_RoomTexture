// ERROR TESTING REMOVE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq; // For getting the correct resolution from the webcam for Hololens

#if !UNITY_EDITOR
using Windows.Storage; // for getting access to the correct Windows folder for Holotoolkit's storage of the room mesh
#endif

namespace UWB_RoomTexture
{
    public static class RoomTexture
    {
        /*
        public static string RoomTextureFolderPath =
            Application.dataPath + Path.AltDirectorySeparatorChar
            + "Textures" + Path.AltDirectorySeparatorChar
            + "Raw Room Textures" + Path.AltDirectorySeparatorChar;

        public static string ClippedRoomTextureFolderPath =
            Application.dataPath + Path.AltDirectorySeparatorChar
            + "Textures" + Path.AltDirectorySeparatorChar
            + "Final Room Textures" + Path.AltDirectorySeparatorChar;

        public static string RoomMeshFolderPath =
            Application.dataPath + Path.AltDirectorySeparatorChar
            + "Meshes" + Path.AltDirectorySeparatorChar;

        public static string RoomMeshFolderPath_HoloToolkit =
#if !UNITY_EDITOR
            ApplicationData.Current.RoamingFolder.Path;
#else
            Application.persistentDataPath;
#endif

        public static string CameraLocationFolderPath =
            Application.dataPath + Path.AltDirectorySeparatorChar
            + "CameraLocations" + Path.AltDirectorySeparatorChar;

        public static string MaterialFolderPath =
            Application.dataPath + Path.AltDirectorySeparatorChar
            + "Materials" + Path.AltDirectorySeparatorChar;

        public static string ProjectorFolderPath =
            Application.dataPath + Path.AltDirectorySeparatorChar
            + "Projectors" + Path.AltDirectorySeparatorChar;

        // ERROR TESTING REMOVE?
        public static string TextureFolderPath =
            Application.dataPath + Path.AltDirectorySeparatorChar
            + "Materials" + Path.AltDirectorySeparatorChar
            + "Textures" + Path.AltDirectorySeparatorChar;

        public static string PrefabFolderPath =
            Application.dataPath + Path.AltDirectorySeparatorChar
            + "Prefabs" + Path.AltDirectorySeparatorChar;

        public static string TexturePrefabName = "TexturePrefab";

        public static string RoomMeshFile = "RoomMesh";
        public static string RoomMeshSuffix = ".room";
        
        public static string FileSuffix = ".png";

        public static TextureFormat Format = TextureFormat.RGBA32;

        public static Resolution CameraResolution = UnityEngine.VR.WSA.WebCam.PhotoCapture.SupportedResolutions.OrderByDescending<Resolution, int>((res) => res.width * res.height).First();
        public static TextureWrapMode WrapMode = TextureWrapMode.Clamp;

        public static string SpatialMappingName = "Spatial Mapping";

        public enum FileSuffixTypes
        {
            Jpeg,
            PNG
        };

        public static string FileSuffix_PNG = ".png";
        public static string FileSuffix_JPG = ".jpg";
        
        public static string LayerName = "Room Mesh Layer";
        
        public static string SpatialMappingPrefabPath =
            Application.dataPath + Path.DirectorySeparatorChar
            + "HoloToolkit" + Path.DirectorySeparatorChar
            + "SpatialMapping" + Path.DirectorySeparatorChar
            + "Prefabs" + Path.DirectorySeparatorChar
            + "SpatialMapping.prefab";

        public static string Shader_RoomBackground = "RoomCanvasShader";
        public static string Shader_RoomTexture = "RoomProjectionShader";
        public static string Shader_RoomTexture_NonProjector = "RoomProjectionShader_NonProjector";

        public static bool DebugFlag = false;

        public static string RoomObjectName = "Room";

        public static int TextureAutoSuffixInt = 1;
        public static string TextureAutoName = "RoomTex_";

        public static string CameraLocationAutoName = "CameraLocation_";
        public static string FileSuffix_CameraLocation = ".camloc";
        public static char CameraLocation_MatrixCellSeparator = ',';
        public static string CameraLocation_MatrixSeparator = "\n\n";

        public static string Projector_AutoName = "Projector_";

        public static string Projector_MaterialAutoName = "Material_";
        public static string Material_Canvas = "Canvas Material";

        public static string FileSuffix_Material = ".mat";
        public static string FileSuffix_Prefab = ".prefab";

        public enum VoiceCommandsEnum
        {
            BeginRoomTexture,
            RoomScreenshot,
            EndRoomTexture
        };

        public enum GesturesEnum
        {
            RoomScreenshot
        };

        public static int TimeToLoadRoomMesh = 3;
        public static int TextureThreshold = 30;
        public static bool LessThanTextureThreshold = true;

        #region Unity Button Presses
        public static KeyCode KeyCode_TextureCaptureFinished = KeyCode.T;
        #endregion

        #region Hololens Audio Keyphrases
        public static string Keyword_BeginRoomTexturing = "Start Room Texturing";
        public static string Keyword_RoomScreenshot = "Texture";
        public static string Keyword_EndRoomTexturing = "Stop Room Texturing";
#endregion

#region Error Message Strings
        public static string Error_HololensKeyphraseNotFound = "Hololens Keyphrase not found.";
        public static string Error_LocatableCameraLocationNotFound = "Unity locatable camera cannot retrieve location data for image taken.";
        public static string Error_CameraResolutionNotFoundForFOV = "Camera resolution specified not found when trying to determine horizontal field of view for projector.";
        public static string Error_RoomTextureLayerNotFound = "Room Texture's layer was not found. Please ensure that there is a layer in the Unity editor with the exact same name as set by the string variable \"LayerName\" in the \"RoomTexture\" script.";
        public static string Error_GestureNotFound = "Gesture not found.";
#endregion

#region Debug Message Strings
        public static string Debug_PhotoCaptureModeInitFailed = "Photo Capture mode initialization failed.";
        public static string Debug_PhotoCaptured = "Photo capture mode ended. Processing data...";
        public static string Debug_PhotoCaptureModeEnded = "Photo capture mode ended successfully.";
        public static string Error_NullTexture = "Null Texture encountered.";
        public static string Error_NonAlphaTextureFileTypeUsed =
            "This project manipulates and utilizes alpha values "
            + "and will only allow filetypes that support them to "
            + "be saved as textures. If the filetype passed in "
            + "supports alpha values, please reference Save"
            + "() in the SaveTexture class.";
        public static string Error_UnsupportedTextureFileFormat =
            "Encountered unsupported file format. "
            + "You may need to alter method to use this type of "
            + "file format. Without alpha values, this file format "
            + "may be incompatible with this asset's internal workings.";
        public static string Error_UnsupportedMaterialFileFormat =
            "Encountered unsupported material file format. File format should be " + FileSuffix_Material;
        public static string Error_UnsupportedPrefabFileType =
            "Encountered incorrect prefab file format.";
#endregion
        
#region Methods
        public static int GetHorizontalFOV(Resolution res)
        {
            int horizontalFOV = 45;

            if(res.width == 1280
                && res.height == 720)
            {
                horizontalFOV = 45;
            }
            else if(res.width == 2048
                && res.height == 1152)
            {
                horizontalFOV = 67;
            }
            else if(res.width == 1408
                && res.height == 792)
            {
                horizontalFOV = 48;
            }
            else if(res.width == 1344
                && res.height == 756)
            {
                horizontalFOV = 67;
            }
            else if(res.width == 896
                && res.height == 504)
            {
                horizontalFOV = 48;
            }
            else
            {
                throw new System.Exception(Error_CameraResolutionNotFoundForFOV);
            }

            return horizontalFOV;
        }
#endregion
*/
    }
}