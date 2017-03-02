using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.WebCam; // For PhotoCaptureFrame
using System.IO;
using System; // For platform-specific newlines (Environment)

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UWB_RoomTexture
{
    // ERROR TESTING - REMOVE INHERITANCE - UNNEEDED
    public class CameraLocation //: MonoBehaviour
    {
        private bool hasLocationData;
        private Matrix4x4 cameraToWorldTransform;
        private Matrix4x4 projectionTransform;
        private Matrix4x4 worldToCameraTransform;
        private float nearClipPlane;
        private float farClipPlane;
        private string customName;

        public CameraLocation()
        {
            hasLocationData = false;
            cameraToWorldTransform = new Matrix4x4();
            projectionTransform = new Matrix4x4();
            worldToCameraTransform = new Matrix4x4();
            nearClipPlane = 0.1f;
            farClipPlane = 50.0f;
            customName = Constants.Names.CameraLocationAutoName;
        }

        public CameraLocation(PhotoCaptureFrame frame)
        {
            hasLocationData = frame.hasLocationData;
            nearClipPlane = 0.1f;
            farClipPlane = 50.0f;

            // Grab the PhotoCaptureFrame's location data and record it
            if (frame.hasLocationData)
            {
                if (!frame.TryGetCameraToWorldMatrix(out cameraToWorldTransform)
                    || !frame.TryGetProjectionMatrix(nearClipPlane, farClipPlane, out projectionTransform))
                {
                    hasLocationData = false;
                }
                else
                {
                    worldToCameraTransform = cameraToWorldTransform.inverse;
                }
            }

            // Safeguard against a failure to grab the data
            if(!hasLocationData)
            {
                cameraToWorldTransform = new Matrix4x4();
                projectionTransform = new Matrix4x4();
                worldToCameraTransform = new Matrix4x4();

                if (Constants.DebugStrings.DebugFlag)
                    Debug.Log(Constants.ErrorStrings.LocatableCameraLocationNotFound);
            }

            customName = Constants.Names.CameraLocationAutoName;
        }

        /// <summary>
        /// Save a file representing the stored matrices with the given filename. Assumes that the filename doesn't have an extension (will automatically assign it).
        /// </summary>
        /// <param name="cameraLocation"></param>
        /// <param name="filename"></param>
        public static void Save(CameraLocation cameraLocation, string filename)
        {
            string camToWorldString = "{";
            string projectionString = "{";
            string nearClipPlaneString = cameraLocation.NearClipPlane.ToString();
            string farClipPlaneString = cameraLocation.FarClipPlane.ToString();
            
            if (cameraLocation.hasLocationData)
            {
                // Record the Camera To World Transform
                for (int y = 0; y < 4; y++) {
                    Vector4 rowData = cameraLocation.CameraToWorldTransform.GetRow(y);
                    camToWorldString +=
                        Environment.NewLine + rowData.x + Constants.Camera.CameraLocation_MatrixCellSeparator
                        + rowData.y + Constants.Camera.CameraLocation_MatrixCellSeparator
                        + rowData.z + Constants.Camera.CameraLocation_MatrixCellSeparator
                        + rowData.w;
                }
                camToWorldString += Environment.NewLine + "}";

                // Record the Projection Transform
                for (int y = 0; y < 4; y++)
                {
                    Vector4 rowData = cameraLocation.ProjectionTransform.GetRow(y);
                    projectionString +=
                        Environment.NewLine + rowData.x + Constants.Camera.CameraLocation_MatrixCellSeparator
                        + rowData.y + Constants.Camera.CameraLocation_MatrixCellSeparator
                        + rowData.z + Constants.Camera.CameraLocation_MatrixCellSeparator
                        + rowData.w;
                }
                projectionString += Environment.NewLine + "}";
            }
            else {
                for (int y = 0; y < 4; y++)
                {
                    camToWorldString += Environment.NewLine;
                    projectionString += Environment.NewLine;
                    for (int x = 0; x < 3; x++)
                    {
                        camToWorldString += "0" + Constants.Camera.CameraLocation_MatrixCellSeparator;
                        projectionString += "0" + Constants.Camera.CameraLocation_MatrixCellSeparator;
                    }
                    camToWorldString += '0';
                    projectionString += '0';
                }
                camToWorldString += Environment.NewLine + "}";
                projectionString += Environment.NewLine + "}";
            }
            
            // Create a file or open one for overwriting
            File.WriteAllText(
                // Camera location info folder path + (name & file suffix)
                Constants.Folders.CameraLocationFolderPath
                + filename
                + Constants.Suffixes.FileSuffix_CameraLocation,

                // Contents to write to file
                camToWorldString + Constants.Camera.CameraLocation_MatrixSeparator
                + projectionString + Constants.Camera.CameraLocation_MatrixSeparator
                + nearClipPlaneString + Constants.Camera.CameraLocation_MatrixSeparator
                + farClipPlaneString
            );

#if UNITY_EDITOR
            // Immediately update folders / files shown
            AssetDatabase.Refresh();
#endif
        }

        public static CameraLocation Load(string filepath)
        {
            CameraLocation camLoc = null;

            if (File.Exists(filepath)) {
                camLoc = new CameraLocation();

                string[] fileLines = File.ReadAllLines(filepath);

                if (fileLines.Length != 17)
                {
                    return null;
                }

                // Read in CameraToWorldTransform matrix
                Matrix4x4 camToWorldTransform = new Matrix4x4();
                for (int i = 0; i < 4; i++)
                {
                    camToWorldTransform.SetRow(i, ReadMatrixRow(fileLines[i + 1]));
                }

                // Read in ProjectionTransform matrix
                Matrix4x4 projectionTransform = new Matrix4x4();
                for (int i = 0; i < 4; i++)
                {
                    projectionTransform.SetRow(i, ReadMatrixRow(fileLines[i + 8]));
                }

                // Read in Near Clip Plane
                float nearClipPlane = float.Parse(fileLines[14]);

                // Read in Far Clip Plane
                float farClipPlane = float.Parse(fileLines[16]);

                // Set CameraLocation data
                camLoc.HasLocationData = true;
                camLoc.CameraToWorldTransform = camToWorldTransform;
                camLoc.ProjectionTransform = projectionTransform;
                camLoc.WorldToCameraTransform = camToWorldTransform.inverse;
                camLoc.NearClipPlane = nearClipPlane;
                camLoc.FarClipPlane = farClipPlane;
                camLoc.name = GetFileNameWithoutExtension(filepath);
            }

            return camLoc;
        }

        public static Vector4 ReadMatrixRow(string matrixLine)
        {
            Vector4 cellValues = new Vector4();
            string[] cellValueStrings = matrixLine.Split(Constants.Camera.CameraLocation_MatrixCellSeparator);
            for(int i = 0; i < cellValueStrings.Length; i++)
            {
                cellValues[i] = float.Parse(cellValueStrings[i]);
            }

            return cellValues;
        }

        public static string GetFileNameWithoutExtension(string filepath)
        {
            string[] filenameComponents = Path.GetFileName(filepath).Split('.');
            string filenameWithoutExtension = filenameComponents[filenameComponents.Length - 2];

            return filenameWithoutExtension;
        }

        /*
        // Assumes only the lines between the curly braces are passed
        public static Matrix4x4 ReadFormattedMatrix(string[] matrixStringLines)
        {
            Matrix4x4 matrix = new Matrix4x4();
            for(int i = 0; i < matrixStringLines.Length; i++)
            {
                string[] matrixCells = matrixStringLines[i].Split(RoomTexture.CameraLocation_MatrixCellSeparator);
                for(int cellIndex = 0; cellIndex < matrixCells.Length; cellIndex++)
                {
                    int cell;
                    matrix[i, cellIndex] = (int.TryParse(matrixCells[cellIndex], out cell)) 
                        ? cell 
                        : 0;
                }
            }

            return matrix;
        }
        */

        public static int GetHorizontalFOV(Resolution res)
        {
            int horizontalFOV = 45;

            if (res.width == 1280
                && res.height == 720)
            {
                horizontalFOV = 45;
            }
            else if (res.width == 2048
                && res.height == 1152)
            {
                horizontalFOV = 67;
            }
            else if (res.width == 1408
                && res.height == 792)
            {
                horizontalFOV = 48;
            }
            else if (res.width == 1344
                && res.height == 756)
            {
                horizontalFOV = 67;
            }
            else if (res.width == 896
                && res.height == 504)
            {
                horizontalFOV = 48;
            }
            else
            {
                throw new System.Exception(Constants.ErrorStrings.CameraResolutionNotFoundForFOV);
            }

            return horizontalFOV;
        }

        // Set the position, rotation, and scale of the projector
        // This information was found on a forum post at http://answers.unity3d.com/questions/402280/how-to-decompose-a-trs-matrix.html
        public static void GetTransformValues(Matrix4x4 worldToCameraTransform, out Vector3 position, out Quaternion rotation, out Vector3 localScale)
        {
            // Set the position, rotation, and scale of the projector
            // This information was found on a forum post at http://answers.unity3d.com/questions/402280/how-to-decompose-a-trs-matrix.html
            position = worldToCameraTransform.GetColumn(3);
            rotation = Quaternion.LookRotation(
                worldToCameraTransform.GetColumn(2),
                worldToCameraTransform.GetColumn(1)
                );
            localScale = new Vector3(
                worldToCameraTransform.GetColumn(0).magnitude,
                worldToCameraTransform.GetColumn(1).magnitude,
                worldToCameraTransform.GetColumn(2).magnitude
                );
        }

        public static void SetTransformValues(Transform transform, CameraLocation camLoc)
        {
            Vector3 position;
            Quaternion rotation;
            Vector3 localScale;

            GetTransformValues(camLoc.WorldToCameraTransform, out position, out rotation, out localScale);
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = localScale;
        }

        #region Properties
        public bool HasLocationData
        {
            get
            {
                return hasLocationData;
            }
            set
            {
                hasLocationData = value;
            }
        }
        public Matrix4x4 CameraToWorldTransform
        {
            get
            {
                return cameraToWorldTransform;
            }
            set
            {
                cameraToWorldTransform = value;
                worldToCameraTransform = cameraToWorldTransform.inverse;
            }
        }
        public Matrix4x4 ProjectionTransform
        {
            get
            {
                return projectionTransform;
            }
            set
            {
                projectionTransform = value;
            }
        }
        public Matrix4x4 WorldToCameraTransform
        {
            get
            {
                return cameraToWorldTransform.inverse;
            }
            set
            {
                worldToCameraTransform = value;
                cameraToWorldTransform = worldToCameraTransform.inverse;
            }
        }
        public float NearClipPlane
        {
            get { return nearClipPlane; }
            set { nearClipPlane = value; }
        }
        public float FarClipPlane
        {
            get { return farClipPlane; }
            set { farClipPlane = value; }
        }
        public new string name
        {
            get { return customName; }
            set { customName = value; }
        }
        #endregion
    }
}