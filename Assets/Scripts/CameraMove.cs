using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {


	public int speed;
	public int currentRot;
	public float cameraZoom;

	// Update is called once per frame
	void Update () {

		transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, Input.GetAxis("Vertical") * speed * Time.deltaTime, 0);
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(30, 30 + 90 * currentRot, 0), .4f);
		
		if(Input.GetKeyDown(KeyCode.Q)){
			ModifyRotation(-1);
		}
		if(Input.GetKeyDown(KeyCode.E)){
			ModifyRotation(1);
		}

		cameraZoom -= Input.mouseScrollDelta.y;
		HandManager.instance.currentOffset +=Input.mouseScrollDelta.y;
		

		Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, cameraZoom, .05f);

	}

	public void ModifyRotation(int amount){
		currentRot += amount;
	}
}
