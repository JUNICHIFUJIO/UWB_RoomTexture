using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticesLog : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        string vertexPositions = "";
        for(int i = 0; i < 10000; i += 100)
        {
            vertexPositions += "{" + vertices[i].x + ", " + vertices[i].y + ", " + vertices[i].z + "}; \n";
        }

        Debug.Log("vertexPositions = " + vertexPositions);
	}
}
