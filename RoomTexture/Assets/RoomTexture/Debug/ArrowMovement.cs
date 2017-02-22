using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    public float speed = 1.5f;

	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        else if (Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        else if (Input.GetKey(KeyCode.UpArrow))
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        else if (Input.GetKey(KeyCode.DownArrow))
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        else if (Input.GetKey(KeyCode.E))
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        else if (Input.GetKey(KeyCode.Q))
            transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
	}
}
