using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RenderManager : MonoBehaviour {

	public MeshRenderer RenderRef;
	public BoxCollider ColliderRef;
	public Card cardRef;
	public TextMesh BurnCost;
	public TextMesh PurchaseCost;
	

	private void Awake() 
	{
		if(RenderRef == null || ColliderRef == null)
		{
			RenderRef = gameObject.GetComponent<MeshRenderer>();
			ColliderRef = gameObject.GetComponent<BoxCollider>();	
		}
		BurnCost.text = cardRef.BurnValue + " AP";
	}

	public void EnableCollider(bool b)
	{
		
		ColliderRef.enabled = b;
	
	}
	public void EnableRenderer(bool b)
	{

		RenderRef.enabled = b;
		if(!b){
			BurnCost.gameObject.SetActive(b);
		}

	}

	private void OnMouseDown() {

		cardRef.Play();
		
	}

	private void OnMouseEnter() {
		cardRef.CardMouseEnter();
	}
	private void OnMouseExit() {
		cardRef.CardMouseExit();	
	}
}
