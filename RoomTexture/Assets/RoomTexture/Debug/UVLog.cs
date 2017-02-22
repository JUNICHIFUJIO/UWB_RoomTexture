using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVLog : MonoBehaviour {

	// Use this for initialization
	void Awake () {

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        // Iterate through all of the vertices and reassign the uvs

        Vector2[] uvs = mesh.uv;
        string meshUVs = "Mesh UVs = ";

        for (int i = 0; i < uvs.Length; i++)
        {
            meshUVs += "[" + uvs[i].x + ", " + uvs[i].y + "]; \n";
        }

        Debug.Log(meshUVs);

        // mesh.uv = ;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
