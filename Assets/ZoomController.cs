using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomController : MonoBehaviour {
    private float zoomSpeed = 5.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
	    float scroll = Input.GetAxis("Mouse ScrollWheel");
	    transform.Translate(0, scroll * zoomSpeed/10, scroll * zoomSpeed);
    }
}
