﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
public class Card : MonoBehaviour {
	public string Name;
	public int APCost;
	public RenderManager RenderMan;
	public int BurnValue;
	private TextMesh cardText;
	public bool isInPurchase = false;
	public enum Effects {Draw, Deal, Heal, Action}
	public Effects[] cardEffects;
	public DeckManager charDeck;


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
		cardText = GetComponentInChildren<TextMesh>();
		cardText.text = refreshText();
	}

	public void Play()
	{
		if(SelectionStateManager.instance.currentState == SelectionStateManager.SelectionState.BURN){
			charDeck.playerRef.ModifyActionPoints(BurnValue);
			SelectionStateManager.instance.setSelectedUnit(charDeck.playerRef, false);
			toPlayed();
		} else{
			if(charDeck.playerRef.actionPoints - APCost >= 0){
				charDeck.playerRef.ModifyActionPoints(-APCost);
				Effect();
				toPlayed();
			}

		}
	
	}

	public void Discard()
	{
		charDeck.Hand.Remove(this);
		charDeck.Discard.Add(this);
		EnableEverything(false);
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

}