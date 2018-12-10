using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionStateManager : MonoBehaviour {

	public Player unitSelected;

	public delegate void TurnAction();

	public static event TurnAction OnNewTurn;

	public enum SelectionState
	{
		PLAYER,
		CARDS,
		PATH,
		BURN,
		TACKLE,
		PURCHASE
	}

	Node selectedNode;

	public SelectionState currentState;

	public static SelectionStateManager instance;

	Node playerNode;

	public bool inMotion;

	private void Awake() {
		instance = this;
		currentState = SelectionState.PLAYER;
	}

	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update ()
	{
		RaycastHit hit;
		hit = mouseRaycast();

		
		CardRegionCheck();

		switch(currentState)
		{
			case SelectionState.PATH:

				

				Node nodeToSee = null;


				if(!inMotion)
				{
					if(hit.collider != null)
					{

						if(hit.collider.gameObject.tag == "GridCube" && hit.collider.gameObject.GetComponent<GridSquare>().parentNode.walkable )
						{
							if( !hit.collider.gameObject.GetComponent<GridSquare>().parentNode.hasUnit())
							{
								nodeToSee = hit.collider.gameObject.GetComponent<GridSquare>().parentNode;
							}
						}
					}

					if(nodeToSee != null)
					{
						if(selectedNode != nodeToSee){
							
							Pathfinding.instance.RetracePath(playerNode, nodeToSee);
							selectedNode = nodeToSee;
						}
					}

					if(Input.GetMouseButtonDown(0))
					{
						if(Pathfinding.instance.grid.path != null && nodeToSee != null && nodeToSee.inRange){
							unitSelected.StartPath(Pathfinding.instance.SimplifyAndSend(Pathfinding.instance.grid.path));
							unitSelected.actionPoints -= nodeToSee.totalCost;
							inMotion = true;
						}
					}
				}
				break;
			case SelectionState.CARDS:

				RefreshPaths(false);

				break;
			case SelectionState.BURN:


				break;

			case SelectionState.TACKLE:

				Node nodeToTackle = null;
				if(hit.collider != null)
				{
					if(hit.collider.gameObject.tag == "GridCube" && hit.collider.gameObject.GetComponent<GridSquare>().parentNode.walkable )
					{
						nodeToTackle = hit.collider.gameObject.GetComponent<GridSquare>().parentNode;		
					}
				}
				if(nodeToTackle != null)
				{
					if(selectedNode != nodeToTackle){
						
						selectedNode.mouseOverNode = false;
						nodeToTackle.mouseOverNode = true;
						selectedNode = nodeToTackle;
					}
				}


				break;
		}

		if(Input.GetKeyDown(KeyCode.Space) && !inMotion){
			resetSelections();
			Pathfinding.instance.clearInRange();
			Pathfinding.instance.clearInTacklekRange();
			RefreshPaths(false);
			CanvasManager.instance.ResetButtonUIColor();			
			CanvasManager.instance.EnableCharacterUI(false);
		}

		if(Input.GetKeyDown(KeyCode.B) && unitSelected != null){
			ToggleBurn();
			RefreshPaths(false);
		}
		if(Input.GetKeyDown(KeyCode.T) && unitSelected != null){
			ToggleTackle();
			RefreshPaths(false);
		}

	}

	public void setSelectedUnit(Player newPlayer, bool refreshCards)
	{
		if(unitSelected != null && refreshCards){
			hideCards(unitSelected);
		}
		unitSelected = newPlayer;
		if(refreshCards){
			showCards(newPlayer);
		}

		CanvasManager.instance.EnableCharacterUI(true);

		RefreshAP();
		

		playerNode = Pathfinding.instance.grid.GridFromWorldPoint(unitSelected.transform.position);
		Pathfinding.instance.FindPaths(playerNode, 1, newPlayer.actionPoints, false);

		if(currentState != SelectionState.BURN){
			currentState = SelectionState.PATH;
		}
	}

	public void RefreshAP(){
		CanvasManager.instance.setAPText(unitSelected.actionPoints.ToString());
		CanvasManager.instance.setCurrentAP((float)unitSelected.actionPoints);
		CanvasManager.instance.setStaminaUI(unitSelected.currentStamina, unitSelected.staminaStat);
	}

	public void showCards(Player newPlayer){
		HandManager.instance.handRef = newPlayer.playerDeck.Hand;
		for (int i = 1; i <= newPlayer.playerDeck.Hand.Count; i ++)
		{
			newPlayer.playerDeck.Hand[i-1].EnableEverything(true);
			float newCardPosition = 20 * ((1.0f/(newPlayer.playerDeck.Hand.Count+1)));

			newPlayer.playerDeck.Hand[i-1].gameObject.transform.transform.localPosition = new Vector3(newCardPosition * i,-14 ,4- (i*.2f));
		}
	}

	public void hideCards(Player newPlayer){
		for (int i = 1; i <= newPlayer.playerDeck.Hand.Count; i ++)
		{
			newPlayer.playerDeck.Hand[i-1].EnableEverything(false);
			
		}
	}

	private RaycastHit mouseRaycast() {
		RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
		return hit;
        
	}

	public void resetSelections(){

		if(unitSelected != null){
			hideCards(unitSelected);
		}
		
		unitSelected = null;
		playerNode = null;
		currentState = SelectionState.PLAYER;
		if(Pathfinding.instance.grid.path != null){
			Pathfinding.instance.grid.path.Clear();
		}

	}


	public void RefreshPaths(bool withUnit)
	{
		if(withUnit){
			setSelectedUnit(unitSelected, false);
		}
		Pathfinding.instance.clearInPath();
		Pathfinding.instance.grid.path = null;
	
	}

	public void CardRegionCheck()
	{
	
		if((unitSelected != null && !inMotion && currentState == SelectionState.PATH) && (Input.mousePosition.y < Screen.height / 4 && Input.mousePosition.x > Screen.width / 2)){
			currentState = SelectionState.CARDS;
		} else if(unitSelected != null && currentState == SelectionState.CARDS && !(Input.mousePosition.y < Screen.height / 4 && Input.mousePosition.x > Screen.width / 2))
		{
			currentState = SelectionState.PATH;
		}
	}

	public void ToggleBurn()
	{
		currentState = currentState == SelectionState.BURN?SelectionState.PATH:SelectionState.BURN;
		CanvasManager.instance.BurnButton.image.color = currentState == SelectionState.BURN?Color.red:Color.white;
		RefreshPaths(false);
		unitSelected.playerDeck.ToggleBurnActive();
	}

	public void ToggleTackle()
	{
		currentState = currentState == SelectionState.TACKLE?SelectionState.PATH:SelectionState.TACKLE;
		CanvasManager.instance.TackeButton.image.color = currentState == SelectionState.TACKLE?Color.red:Color.white;
		RefreshPaths(false);
		if(currentState == SelectionState.TACKLE){
			Pathfinding.instance.clearInRange();
			Pathfinding.instance.FindPaths(playerNode, 1, unitSelected.attackRange, true);
		} else
		{
			Pathfinding.instance.clearInTacklekRange();
			Pathfinding.instance.FindPaths(playerNode, 1, unitSelected.actionPoints, false);
		}
	}

	public void EndTurn(){
		RefreshPaths(false);
		OnNewTurn();
		if(unitSelected!=null){
			setSelectedUnit(unitSelected, false);
		}
	}

}
