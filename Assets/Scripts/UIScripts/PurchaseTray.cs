using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseTray : MonoBehaviour {

	private Image trayParent;
	
	[SerializeField]
	private Image darkenedBackground;

	public enum TrayState
	{
		PLAYER_SELECTED,
		OPEN,
		NOT_SELECTED
	}

	public Text buttonText;
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
		SetButtonText(TrayState.OPEN);
		targetLocation = new Vector2(0, -60);
		while(trayTransform.anchoredPosition.y > targetLocation.y + .1f){
			trayTransform.anchoredPosition = Vector2.Lerp(trayTransform.anchoredPosition, targetLocation, .3f);
			yield return null;
		}
		print("OutOfLoop");
		yield break;

	}

	IEnumerator MoveUp(){
		targetLocation = new Vector2(0, 30);
		while(trayTransform.anchoredPosition.y < targetLocation.y - .1f){
			trayTransform.anchoredPosition = Vector2.Lerp(trayTransform.anchoredPosition, targetLocation, .3f);
			yield return null;
		}
		print("OutOfLoop2");
		yield break;

	}

	private void ShowBackground(bool active){
		darkenedBackground.enabled = active;
	}

	private void OnMouseOver() {
		Pathfinding.instance.clearInPath();
	}

	public void SetButtonText(TrayState targetState){
		switch (targetState){
			case TrayState.NOT_SELECTED:
				buttonText.text = "LOOK AT";
				break;
			case TrayState.PLAYER_SELECTED:
				buttonText.text = "PURCHASE";
				break;
			case TrayState.OPEN:
				buttonText.text = "CLOSE";
				break;
		}
	}
	
}
