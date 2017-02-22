using System; // for EventArgs for events
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows; // for grabbing texture files
using System.IO; // For accessing all files and bytes in texture directory

public class UVReassignment : MonoBehaviour {

    public bool calculated = false;

    string meshName = "default_MeshPart0";

    // ERROR TESTING REINSTANTIATE
    //GameObject.Find(meshName).GetComponent(TextureCapture).RoomTextureCaptured += 
    //    GenerateAtlasAndSetUVs;

    // Event delegate and event 
    //public delegate void RoomTexturesRetrievedHandler(object obj, EventArgs e);
    //public event RoomTexturesRetrievedHandler RoomTexturesRetrieved;

    //void OnRoomTexturesRetrieved()
    //{
    //    if (RoomTexturesRetrieved != null)
    //        RoomTexturesRetrieved(this, e);
    //}

	// Use this for initialization
    /*
    void GenerateAtlasAndSetUVs(object sender, TextureCapture.RoomTextureCapturedArgs args)
    {
        if (!calculated)
        {
            // Unpack the arguments passed in
            int numTextures = args.numTextures;
            ArrayList worldToCamTransforms = args.worldToCamTransforms;
            ArrayList projectionTransforms = args.projectionTransforms;

            // Generate a blank texture to make a texture atlas
            Texture2D atlas = new Texture2D(10, 10);
            // Grab all of the textures
            Texture2D[] atlasTextures = new Texture2D[numTextures];
            int atlasIndex = 0;
            string[] textureFilePaths = System.IO.Directory.GetFiles(Application.dataPath + "//" + TextureCapture.RoomTextureFolderName + "//");
            foreach(string textureFilePath in textureFilePaths)
            {
                byte[] textureBytes = File.ReadAllBytes(textureFilePath);
                atlasTextures[atlasIndex++].LoadImage(textureBytes);
                // ERROR TESTING ^ MIGHT NEED TO INSTANTIATE EACH TEXTURE BEFORE LOADIMAGE CAN WORK
            }

            // Set all of the atlas UVs after packing the textures into a texture atlas
            Rect[] atlasUVs = atlas.PackTextures(atlasTextures, 0, numTextures);
            
            // Need to send over all stuff to a shader
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;

            // Iterate through all of the vertices and reassign the uvs

            Vector2[] uvs = new Vector2[vertices.Length];

            // assume that the camera is going to be at 0, 0, 0
            // point the camera over to 

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 uv = new Vector2((float)i * 40.0f / vertices.Length, (float)i * 40.0f / vertices.Length);
                uvs[i] = uv;
            }

            string meshUVs = "Mesh UVs = ";

            for (int i = 0; i < uvs.Length; i++)
            {
                meshUVs += "[" + uvs[i].x + ", " + uvs[i].y + "]; \n";
            }

            Debug.Log(meshUVs);

            mesh.uv = uvs;
            calculated = true;
        }

        // mesh.uv = ;
    }
    */

    void Awake()
    {
        if (!calculated)
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;

            // Iterate through all of the vertices and reassign the uvs

            Vector2[] uvs = new Vector2[vertices.Length];

            // assume that the camera is going to be at 0, 0, 0
            // point the camera over to 

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 uv = new Vector2((float)i * 40.0f / vertices.Length, (float)i * 40.0f / vertices.Length);
                uvs[i] = uv;
            }

            string meshUVs = "Mesh UVs = ";

            for (int i = 0; i < uvs.Length; i++)
            {
                meshUVs += "[" + uvs[i].x + ", " + uvs[i].y + "]; \n";
            }

            Debug.Log(meshUVs);

            mesh.uv = uvs;
            calculated = true;
        }

        // mesh.uv = ;
    }
    
}
