using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseAreaManager : MonoBehaviour {

	public int startPosition = 0;
	public int endPosition = 20;

	private Card[] cardRef;

	public List<GameObject> purchasableCards;

	public float cardHeightModifier;

	public static PurchaseAreaManager instance;

	public PurchaseTray purchaseTrayRef;

	public float currentOffset= 0;
	public float offset = 0; 

	private bool isOpen;

	private void Awake()
	{
		instance = this;
		cardRef = null;
	}

	private void Start() {
		cardRef = new Card[5];
		for(int i = 0; i < 5; i++){
			AddCardToPosition(i);
		}
	}

	public void ToggleTray(){
		purchaseTrayRef.MovePurchasetray(!isOpen);
		isOpen = !isOpen;
		if(SelectionStateManager.instance.currentState != SelectionStateManager.SelectionState.PURCHASE && isOpen){
			SelectionStateManager.instance.currentState = SelectionStateManager.SelectionState.PURCHASE;
		} else
		{
			SelectionStateManager.instance.currentState = SelectionStateManager.SelectionState.PLAYER;
		}
	}

	public void AddCardToPosition(int cardPosition){
		int randomIndex = Random.Range(0, purchasableCards.Count);
		GameObject current = Instantiate(purchasableCards[randomIndex], Vector3.zero, Quaternion.identity);

		current.transform.SetParent(Camera.main.transform);
		current.transform.localEulerAngles = new Vector3(0,0,0);

		Card currentCard = current.GetComponent<Card>();
		currentCard.ToggleShadows(false);

		if(cardRef[cardPosition] != null){
			Destroy(cardRef[cardPosition].gameObject);
		}
		cardRef[cardPosition] = currentCard;
	}

	private void Update() 
	{
		offset = Mathf.Lerp(offset, currentOffset, 0.1f);
		if(cardRef != null)
		{
			if(cardRef.Length != 0)
			{
				float newCardPosition = endPosition * ((1.0f/(cardRef.Length+1)));

				for(int i = 1; i <= cardRef.Length; i++)
				{
					if (SelectionStateManager.instance.currentState == SelectionStateManager.SelectionState.PURCHASE)
					{
						cardHeightModifier = -4;
					} else
					{
						cardHeightModifier = 0;
					}
					cardRef[i-1].gameObject.transform.localPosition = Vector3.Lerp(cardRef[i-1].gameObject.transform.localPosition, new Vector3((-10 + (newCardPosition - offset/3f) * i),(12 + cardHeightModifier) + offset / 1.05f ,4- (i*.2f)), .3f);
					cardRef[i-1].gameObject.transform.localScale = Vector3.Lerp(cardRef[i-1].gameObject.transform.localScale,new Vector3(3 - (offset/3f), 5-(offset/3f), 0.2f), 0.8f);
				}	
			}
		}
	}
}
