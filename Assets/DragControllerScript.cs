﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragControllerScript : MonoBehaviour {

    public float dragSpeed = 1;
    private Vector3 dragOrigin;

    public bool cameraDragging = true;

    public float outerLeft =  -5f;
    public float outerRight = 10f;
    public float zLimitOut = -10f;
    public float zLimitIn = 10f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        
	    Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

	    float left = Screen.width * 0.2f;
	    float right = Screen.width - (Screen.width * 0.2f);

	    if (Input.GetMouseButtonDown(0))
	    {
	        dragOrigin = Input.mousePosition;
	        return;
	    }

	    if (!Input.GetMouseButton(0)) return;

	    Vector3 pos = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
	    Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

	    transform.Translate(move);
    }
}
