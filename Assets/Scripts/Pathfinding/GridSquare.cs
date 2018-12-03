using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour {



	private bool wasPath;
	private Renderer renderRef;
	public Node parentNode;
	
	private void Start()
	{
		renderRef = GetComponent<Renderer>();
	}

	void LateUpdate () 
	{
		if(parentNode.isInPath == true)
		{
			renderRef.material.color = Color.black;

		} else if(parentNode.inRange)
		{
			renderRef.material.color = Color.cyan;
		}else if(parentNode.mouseOverNode){
			renderRef.material.color = Color.green;
		}
		else if(parentNode.inTackleRange){
			renderRef.material.color = Color.magenta;
		}else
		{
			renderRef.material.color = Color.gray;
		}


		if(!parentNode.walkable)
		{
			renderRef.material.color = Color.red;
		}
	
	}
}
