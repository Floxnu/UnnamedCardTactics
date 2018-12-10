using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseTray : MonoBehaviour {

	private Image trayParent;
	
	[SerializeField]
	private Image darkenedBackground;

	private RectTransform trayTransform;
	Vector2 targetLocation;

	private void Awake() {
		trayParent = GetComponent<Image>();
		trayTransform = trayParent.GetComponent<RectTransform>();
	}

	public void MovePurchasetray(bool isDown){
		if(isDown){
			StopCoroutine("MoveUp");
			StartCoroutine("MoveDown");
			ShowBackground(true);
		}else
		{
			StopCoroutine("MoveDown");
			StartCoroutine("MoveUp");
			ShowBackground(false);
		}
	}

	IEnumerator MoveDown(){
		targetLocation = new Vector2(0, -60);
		while(trayTransform.anchoredPosition.y > targetLocation.y + .1f){
			trayTransform.anchoredPosition = Vector2.Lerp(trayTransform.anchoredPosition, targetLocation, .3f);
			yield return null;
		}
		print("OutOfLoop");
		yield break;
		print("GotHere");
	}

	IEnumerator MoveUp(){
		targetLocation = new Vector2(0, 30);
		while(trayTransform.anchoredPosition.y < targetLocation.y - .1f){
			trayTransform.anchoredPosition = Vector2.Lerp(trayTransform.anchoredPosition, targetLocation, .3f);
			yield return null;
		}
		print("OutOfLoop2");
		yield break;
		print("GotHere");
	}

	private void ShowBackground(bool active){
		darkenedBackground.enabled = active;
	}
	
}
