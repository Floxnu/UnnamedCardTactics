using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private bool isSelected{get; set;} = false;
	float Speed = 13f;
	public Vector3[] path;
	public double actionPoints;
	public DeckManager playerDeck;
	public int attackRange;
	SelectionStateManager selectionRef;

	private void Awake() {
		playerDeck = gameObject.GetComponent<DeckManager>();
	}

	private void Start() {
		selectionRef = SelectionStateManager.instance;
		Pathfinding.instance.grid.GridFromWorldPoint(transform.position).unitInSquare = null;
	}

	public void StartPath (Vector3[] pathArray)
	{
		path = pathArray;
		StopCoroutine("FollowPath");
		StartCoroutine("FollowPath");
	}


	private void OnMouseDown() {
		if(selectionRef.currentState == SelectionStateManager.SelectionState.PLAYER){
			selectionRef.setSelectedUnit(this, true);
		}
	}


	IEnumerator FollowPath()
	{
		Pathfinding.instance.grid.GridFromWorldPoint(transform.position).unitInSquare = null;
		int targetIndex = 0;
		Vector3 currentWaypoint = path[0];
		bool moving = true;

		while (moving)
		{
			if(transform.position == currentWaypoint)
			{
				targetIndex++;
				if (targetIndex > path.Length - 1)
				{
					moving = false;
				}else
				{
					currentWaypoint = path[targetIndex];
				}
			}

			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, Speed * Time.deltaTime);
			yield return null;
		}
		SelectionStateManager.instance.RefreshPaths(true);
		Pathfinding.instance.grid.GridFromWorldPoint(transform.position).unitInSquare = this;
		SelectionStateManager.instance.inMotion = false;
		yield break;
	}

	public void ModifyActionPoints(int actionPointsToGain){
		actionPoints += actionPointsToGain;
	}
	
}
