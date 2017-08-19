using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateController : MonoBehaviour {

    public float speed = 3.5f;
    private float X;
    private float Y;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
	    if (Input.GetMouseButton(1))
	    {
	        transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, -Input.GetAxis("Mouse X") * speed, 0), Space.World);
	        X = transform.rotation.eulerAngles.x;
	        Y = transform.rotation.eulerAngles.y;
	        transform.rotation = Quaternion.Euler(X, Y, 0);
	    }
    }
}
