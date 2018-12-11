using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour {

	public List<GameObject> Decklist;
	public List<Card> Discard = new List<Card>();
	public Stack<Card> Library = new Stack<Card>();
	public List<Card> Hand = new List<Card>();
	public List<Card> PlayedCards;
	Camera cameraRef;

	public Player playerRef;

	private void Awake() 
	{
		playerRef = GetComponent<Player>();
	}
	private void Start() 
	{
		foreach (GameObject c in Decklist)
		{
			GameObject current = Instantiate(c);
			current.transform.parent = Camera.main.transform;
			Card cardRef = current.GetComponent<Card>();
			cardRef.charDeck = this;
			cardRef.EnableEverything(false);
			Discard.Add(cardRef);
		}
		Library = Shuffle(Discard);	
		EndTurn();
	}
	public Stack<Card> Shuffle(List<Card> toShuffle)
	{
		Stack<Card> ShuffledList = new Stack<Card>(); 
		while (toShuffle.Count > 0)
		{
			int randomIndex = Mathf.RoundToInt(Random.Range(0, toShuffle.Count));
			ShuffledList.Push(toShuffle[randomIndex]);
			toShuffle.RemoveAt(randomIndex);
		}
		return ShuffledList;
	}
	public void Draw(int amount)
	{
		for (int i = 1; i <= amount; i++ )
		{	
			Card cardDrawn = null;
			if(Library.Count <= 0 && Discard.Count != 0)
			{
				Library = Shuffle(Discard);
				cardDrawn = Library.Pop(); 
				
				Hand.Add(cardDrawn);
			}
			else if(Library.Count == 0 && Discard.Count == 0)
			{
				return;
			} else
			{
				cardDrawn = Library.Pop(); 
				Hand.Add(cardDrawn);
			}
			if(cardDrawn != null && SelectionStateManager.instance.currentState != SelectionStateManager.SelectionState.PLAYER){
				cardDrawn.EnableEverything(true);
				cardDrawn.gameObject.transform.localPosition =  new Vector3(20,-10 ,4);
			}
		}
	}

	public void EndTurn()
	{
		if(Hand.Count > 0)
		{
			int toDiscard = Hand.Count;
			for(int i = 0; i < toDiscard; i++)
			{
				print(Hand[0].name);
				Hand[0].Discard();
			}
		}
		if(PlayedCards.Count > 0)
		{
			foreach (Card c in PlayedCards)
			{
				Discard.Add(c);	
			}
			PlayedCards.Clear();
		}

		Draw(5);

	}

	public void ToggleBurnActive(){
		foreach(Card c in Hand){
			c.ToggleBurnText();
		}
	}
	
	public void AddCard(Card cardToAdd){
		Discard.Add(cardToAdd);
		cardToAdd.charDeck = this;
	}
}
