﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node : IHeapItem<Node> {

	public bool walkable;
	public Vector3 worldPosition;
	public bool isInPath;
	public bool mouseOverNode;
	public int totalCost = int.MaxValue;
	public int distance = 1;
	public int gridX;
	public int gridY;
	public Node parent;
	public Player unitInSquare;
	public bool inRange;
	public bool inTackleRange;
	

	int heapIndex;

	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
	{
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	public int HeapIndex
	{
		get{
			return heapIndex;
		}
		set{
			heapIndex = value;
		}
	}	
	
	public int CompareTo(Node nodeToCompare)
	{
		return totalCost - nodeToCompare.totalCost;
	}

	public bool hasUnit(){
		return unitInSquare != null;
	}
	
}



