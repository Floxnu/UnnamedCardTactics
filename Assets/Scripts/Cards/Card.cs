using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;


[SelectionBase]
public class Card : MonoBehaviour {
	public string Name;
	public int APCost;
	public RenderManager RenderMan;
	public int BurnValue;
	private TextMeshPro cardText;
	public bool isInPurchase = false;
	public enum Effects {Draw, Deal, Heal, Action}
	public Effects[] cardEffects;
	public DeckManager charDeck;
	public int indexInPurchase;
	public int purchaseCost;
	private bool isZoomed = false;


	public bool mouseOver;

	[Header("Effect Values")]
	public int cardsToDraw;
	public int actionsToGain;
	public int damageToDeal;
	public int healthToHeal;

	private string refreshText()
	{
		string outputString = APCost + "\n";
		foreach (Effects f in cardEffects)
		{
			switch(f){
				case Effects.Draw:
				outputString += "Draw " + cardsToDraw + "\n";
				break;
			}
		}
		return outputString;
	}

	public void Effect()
	{
		foreach (Effects f in cardEffects)
		{
			switch(f){
				case Effects.Draw:
				charDeck.Draw(cardsToDraw);
				break;
			}
		}
		SelectionStateManager.instance.setSelectedUnit(charDeck.playerRef, false);

	}

	private void Awake() 
	{
		cardText = GetComponentInChildren<TextMeshPro>();
		RenderMan.PurchaseCost.text = purchaseCost.ToString();
		cardText.text = refreshText();
	}

	public void Play()
	{
		switch (SelectionStateManager.instance.currentState){
			case SelectionStateManager.SelectionState.BURN:
				charDeck.playerRef.ModifyActionPoints(BurnValue);
				SelectionStateManager.instance.setSelectedUnit(charDeck.playerRef, false);
				toPlayed();
				break;
			case SelectionStateManager.SelectionState.CARDS:
				if(charDeck.playerRef.actionPoints - APCost >= 0){
					charDeck.playerRef.ModifyActionPoints(-APCost);
					Effect();
					toPlayed();
				}
				break;
			case SelectionStateManager.SelectionState.PURCHASE:
				if(SelectionStateManager.instance.unitSelected != null){
					if(!(indexInPurchase < 0)){
					SelectionStateManager.instance.unitSelected.playerDeck.AddCard(this);
					PurchaseAreaManager.instance.AddCardToPosition(indexInPurchase);
					EnableEverything(false);
					ToggleShadows(true);
					}
				}
				break;
		}
	}

	public void Discard()
	{
		charDeck.Hand.Remove(this);
		charDeck.Discard.Add(this);
		EnableEverything(false);
		charDeck.UpdateUI();
	}
	public void toPlayed()
	{
		charDeck.Hand.Remove(this);
		charDeck.PlayedCards.Add(this);
		EnableEverything(false);
	}

	public void EnableEverything(bool b)
	{
		this.gameObject.transform.position = new Vector3(-5, 2, -2);
		RenderMan.EnableCollider(b);
		RenderMan.EnableRenderer(b);
		RenderMan.PurchaseCost.gameObject.SetActive(b);
		cardText.gameObject.SetActive(b);
	}

	public void CardMouseEnter(){
		mouseOver = true;
	}	
	public void CardMouseExit(){
		mouseOver = false;	
	}

	public void ToggleBurnText(){
		RenderMan.BurnCost.gameObject.SetActive(!RenderMan.BurnCost.gameObject.activeSelf);
	}

	public void ToggleShadows(bool active){

		if(active){

			RenderMan.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;
		}else
		{
			RenderMan.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
			
		}
	}

	public void SetZoomedSize(bool zoomed){
		if(zoomed){
			gameObject.transform.localScale = gameObject.transform.localScale + new Vector3(2,2,2);
			isZoomed = true;
		} else
		{
			gameObject.transform.localScale = gameObject.transform.localScale + new Vector3(-2,-2,-2);
			isZoomed = false;
		}
	}


	IEnumerator ZoomCard(){
		yield return new WaitForSeconds(.3f);
		SetZoomedSize(true);
	}

	public void StartZoom(){
		StartCoroutine("ZoomCard");
	}
	public void StopZoom(){
		StopCoroutine("ZoomCard");
		if(isZoomed){
			SetZoomedSize(false);
		}
	}

}
