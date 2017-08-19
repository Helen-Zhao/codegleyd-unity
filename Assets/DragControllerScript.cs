using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragControllerScript : MonoBehaviour {

    public float dragSpeed = 2;
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

	    if (mousePosition.x < left)
	    {
	        cameraDragging = true;
	    }
	    else if (mousePosition.x > right)
	    {
	        cameraDragging = true;
	    }

	    if (cameraDragging)
	    {

	        if (Input.GetMouseButtonDown(0))
	        {
	            dragOrigin = Input.mousePosition;
	            return;
	        }

	        if (!Input.GetMouseButton(0)) return;

	        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
	        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

            if (move.x > 0f)
	        {
	            if (this.transform.position.x < outerRight)
	            {
	                transform.Translate(move, Space.World);
	            }
	        }
	        else
	        {
	            if (this.transform.position.x > outerLeft)
	            {
	                transform.Translate(move, Space.World);
	            }
	        }

            if (move.z > 0f)
            {
                if (this.transform.position.z < zLimitIn)
                {
                    transform.Translate(move, Space.World);
                }
            }
            else
            {
                if (this.transform.position.z > zLimitOut)
                {
                    transform.Translate(move, Space.World);
                }
            }
        }
    }
}
