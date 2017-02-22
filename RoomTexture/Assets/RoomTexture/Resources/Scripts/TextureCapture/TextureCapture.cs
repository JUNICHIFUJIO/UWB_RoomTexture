/*
This script is designed to listen to a voice command saying "Start Room Texturing". It'll start up the photo mode of Unity's locatable camera with a preset resolution width/height, and pixel format as appropriately dictated by Hololens documentation (ERROR TESTING MUST BE DOUBLE CHECKED). A photo will be taken when the user says "Take Texture". The photo will be processed into a PNG and stored away as a file in a folder listed below among the public and private variables. It will be named something generic.
The script REQUIRES the user to say "Stop Room Texturing"

THINGS TO DO:
--Make voice command for taking pictures
--Integrate texture atlas generation into photo mode ending
--Have text pop up on screen saying that stuff is processing after the picture was taken
--Have text pop up saying debug info if somethign goes wrong
--Have text pop up saying that the user must say "Stop Room Texturing" to end photo taking
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.WebCam; // For accessing the Hololens camera
using UnityEngine.Windows; // For writing textures to a file - required for File. methods
using UnityEngine.Windows.Speech; // For listening to voice commands for Hololens
using System.IO; // For Path

namespace UWB_RoomTexture {
    public class TextureCapture : MonoBehaviour {
        // create public event and for UVReassignment script
        /*
        public class RoomTextureCapturedArgs : EventArgs
        {
            public int numTextures { get; set; }
            public ArrayList worldToCamTransforms { get; set; }
            public ArrayList projectionTransforms { get; set; }
        }
        public delegate void EventHandler<RoomTextureCapturedArgs>(object obj, RoomTextureCapturedArgs args);
        public event EventHandler<RoomTextureCapturedArgs> RoomTextureCaptured;
        */

        // other class items

        //public int ResolutionWidth = 1268;
        //public int ResolutionHeight = 720;
        public static PhotoCapture photoCaptureObject = null;
        //public int numVertices; // MUST GRAB THIS FROM THE ROOM MESH SOMEHOW
        //public ArrayList[] uvArray = null;

        //public Matrix4x4[] worldToCameraTransforms = null;
        //public Matrix4x4[] projectionTransforms = null;
        //public ArrayList worldToCameraTransforms = null;
        //public ArrayList projectionTransforms = null;

        // Set the number suffix for the temp texture files correctly
        void Awake()
        {
            // Reset static int used to track temp texture file suffix number
            Constants.Names.TextureAutoSuffixInt = GetNextAvailableAutoTextureNumber();
        }

        // ERROR TESTING REINCORPORATE ELSEWHERE?
        public static int GetNextAvailableAutoTextureNumber()
        {
            string[] textureFolderFiles = System.IO.Directory.GetFiles(Constants.Folders.RoomTextureFolderPath);

            int maxTextureNumberTaken = 0;
            foreach(string file in textureFolderFiles)
            {
                // Safeguard against files injected into the folder that 
                // aren't texture files
                string fileExtension = Path.GetExtension(file);
                if (fileExtension != Constants.Suffixes.FileSuffix_JPG
                    && fileExtension != Constants.Suffixes.FileSuffix_PNG)
                {
                    continue;
                }

                // Get the file name & look at the trailing 
                // automatically assigned room texture number
                string filename = Path.GetFileNameWithoutExtension(file);
                string[] fileNamePieces = filename.Split('_');
                if(fileNamePieces.Length > 0)
                {
                    string textureNumberString = fileNamePieces[fileNamePieces.Length - 1];

                    // Try to parse the number found at the end of the 
                    // file name as an integer. If successful and it's 
                    // greater than the previously found integer, record 
                    // it and continue
                    int textureNumberTaken;
                    if (int.TryParse(textureNumberString, out textureNumberTaken))
                    {
                        if (maxTextureNumberTaken < textureNumberTaken)
                            maxTextureNumberTaken = textureNumberTaken;
                    }
                }
                else
                {
                    continue;
                }
            }

            return maxTextureNumberTaken + 1;
        }

        #region High-Level Methods
        public static void BeginCapture()
        {
            if(photoCaptureObject == null)
                PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
        }

        public static void Capture()
        {
            if(photoCaptureObject != null)
            {
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            }
        }

        public static void EndCapture()
        {
            // Close and clean up object
            if (photoCaptureObject != null)
                photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }
        #endregion

        // For when photo capture object gets created. initializes object
        public static void OnPhotoCaptureCreated(PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;

            // Get the first camera resolution available - should be correct
            // ERROR TESTING is the first resolution the best resolution? May have to order this
            //Resolution cameraRes = PhotoCapture.SupportedResolutions.GetEnumerator().Current;

            Resolution cameraRes = Constants.Camera.CameraResolution();

            CameraParameters c = new CameraParameters();
            c.hologramOpacity = 0.0f;
            c.cameraResolutionWidth = cameraRes.width;
            c.cameraResolutionHeight = cameraRes.height;
            c.pixelFormat = CapturePixelFormat.BGRA32;
            
            photoCaptureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);
        }

        // For when starting to capture photos
        public static void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
        {
            if (result.success)
            {
                // Enable gestures which call the Capture() method, which takes screenshots and saves them as textures
                Interactions.EnableVoiceCommand(Constants.Commands.VoiceCommandsEnum.RoomScreenshot);
                Interactions.EnableGesture(Constants.Commands.GesturesEnum.RoomScreenshot);
            }
            else
            {
                if (Constants.DebugStrings.DebugFlag)
                    Debug.Log(Constants.DebugStrings.PhotoCaptureModeInitFailed);

                // ERROR TESTING - do i want this to be an exception thrown? do i want to have it retry several times?
            }

        }

        // Save a photo and its location information to a file
        public static void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame frame)
        {
            // If photo was taken...
            if (result.success)
            {
                Resolution camRes = Constants.Camera.CameraResolution();

                // Generate blank texture canvas
                Texture2D targetTexture = new Texture2D(
                    camRes.width, // Width of the texture
                    camRes.height, // Height of the texture
                    TextureFormat.BGRA32, // Format of the texture (Hololens image format)
                    false // Don't mipmap the texture
                    );
                
                // Copy the raw image data into our target texture
                frame.UploadImageDataToTexture(targetTexture);
                
                // The image is flipped and MUST BE REORIENTED in a custom shader
                targetTexture = FlipTexture.FlipHorizontal(targetTexture);

                // Save the texture to a file
                targetTexture.name = Constants.Names.TextureAutoName + Constants.Names.TextureAutoSuffixInt;
                //SaveTexture.Save(targetTexture, RoomTexture.FileSuffixTypes.PNG, RoomTexture.RoomTextureFolderPath);
                SaveTexture.Save(targetTexture, Constants.Suffixes.ImageSuffixTypes.PNG, Constants.Folders.RoomTextureFolderPath);
                Destroy(targetTexture);
                
                // Save the camera location information to a file
                CameraLocation camLoc = new CameraLocation(frame);
                CameraLocation.Save(camLoc, Constants.Camera.CameraLocationAutoName + Constants.Names.TextureAutoSuffixInt);

                // Adjust the automatic tracker number for the texture/cameraLocation file
                Constants.Names.TextureAutoSuffixInt++;

                // Display to DisplayManager most recently saved photo and location
                DisplayManager.UpdateDisplay(targetTexture, camLoc);
            }
        }

        // For when photo taking is manually stopped (clean up)
        public static void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
        {
            if (Constants.DebugStrings.DebugFlag)
            {
                // ERROR TESTING REMOVE?
                Debug.Log(Constants.DebugStrings.PhotoCaptured);
            }

            // Disable the ability to take screenshots until the photo capture mode is started again
            Interactions.DisableVoiceCommand(Constants.Commands.VoiceCommandsEnum.RoomScreenshot);
            Interactions.DisableGesture(Constants.Commands.GesturesEnum.RoomScreenshot);

            // Destroy the photo capture object for memory management
            photoCaptureObject.Dispose();
            photoCaptureObject = null;

            if (Constants.DebugStrings.DebugFlag)
            {
                // ERROR TESTING ADD THIS TEXT TO DEBUG STRINGS
                Debug.Log("Photo capture mode ended successfully.");
            }

            // Let manager know texture capture is finished
            GameObject.Find("TextureManager").GetComponent<TextureManager>().Trigger_TextureCaptureFinished();
            // ERROR TESTING REMOVE
            //TextureManager.Trigger_TextureCaptureFinished();

            // ERROR TESTING UNHOOK AND RELOCATE
            //CentralProcessor.GenerateTexturePrefab();

            // Destroy the temporary DisplayManager helper
            DisplayManager.DestroyDisplay();
        }
    }
}
