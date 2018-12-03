using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {


	public int startPosition = 0;
	public int endPosition = 20;

	public List<Card> handRef;

	public float cardHeightModifier;

	public static HandManager instance;

	public float currentOffset= 0;
	public float offset = 0; 

	private void Awake()
	{
		instance = this;
		handRef = null;
	}

	private void Update() 
	{
		offset = Mathf.Lerp(offset, currentOffset, 0.1f);
		if(handRef != null)
		{
			if(handRef.Count != 0)
			{
				float newCardPosition = endPosition * ((1.0f/(handRef.Count+1)));

				for(int i = 1; i <= handRef.Count; i++)
				{
					if(SelectionStateManager.instance.currentState != SelectionStateManager.SelectionState.CARDS)
					{
						cardHeightModifier = 0;
					} else
					{
						cardHeightModifier = 1;
					}
					if(handRef[i-1].mouseOver)
					{
						cardHeightModifier += 1.5f;
					}
					handRef[i-1].gameObject.transform.localPosition = Vector3.Lerp(handRef[i-1].gameObject.transform.localPosition, new Vector3(((newCardPosition - offset/3f) * i),(-10 + cardHeightModifier) + offset / 1.05f ,4- (i*.2f)), .1f);
					handRef[i-1].gameObject.transform.localScale = Vector3.Lerp(handRef[i-1].gameObject.transform.localScale,new Vector3(3 - (offset/3f), 5-(offset/3f), 0.2f), 0.8f);
				}	
			}
		}
	}
}
