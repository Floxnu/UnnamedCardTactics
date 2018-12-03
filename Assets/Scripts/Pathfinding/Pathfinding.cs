using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour {


	public static Pathfinding instance;


	public PathfindingGrid grid;
	List<Node> minPQ;
	List<Node> visited = new List<Node>();


	private void Awake() 
	{
		grid = GetComponent<PathfindingGrid>();
		instance = this;

	}

	private void Start() 
	{
		minPQ = new List<Node>();

	}

	public void GetPath(Node startNode, Node endNode )
	{
		RetracePath(startNode, endNode);
	}


	public void FindPaths(Node start, int distanceMultiplyer, double actionPoints, bool isInTackle)
	{	
		minPQ.Clear();
		visited.Clear();

		start.totalCost = 0;
		minPQ.Add(start);
		foreach(Node n in grid.grid)
		{
			if(n != start)
			{
				n.totalCost = int.MaxValue;
			}
		}

		while(minPQ.Count > 0)
		{
			Node newSmallest = RemoveFirst();
			visited.Add(newSmallest);

			foreach(Node neighbor in grid.GetNeighbours(newSmallest))
			{
				
				if(!visited.Contains(neighbor) && neighbor.walkable)
				{
					if(!minPQ.Contains(neighbor))
					{
						minPQ.Add(neighbor);
					}
					int altPath = newSmallest.totalCost + neighbor.distance;

					if(altPath < neighbor.totalCost)
					{
						neighbor.totalCost = altPath * distanceMultiplyer;
						if(!isInTackle)
						{	
							if(neighbor.totalCost <= actionPoints){
								neighbor.inRange = true;
							} else
							{
								neighbor.inRange = false;
							}
						} else
						{
							if(neighbor.totalCost <= actionPoints){
								neighbor.inTackleRange = true;
							}else
							{
								neighbor.inTackleRange = false;
							}
						}
						neighbor.parent = newSmallest;
					}

				}
			}

		}	
	}

	public void RetracePath (Node startNode, Node endNode)
	{
		clearInPath();
		List<Node> path = new List<Node>();
		Node currentNode = endNode;


		while (currentNode != startNode)
		{
			path.Add(currentNode);

			currentNode.isInPath = true;
			currentNode = currentNode.parent;
		}


		
		path.Reverse();
		grid.path = path;

	}

	public Vector3[] SimplifyAndSend(List<Node> finalPath)
	{
		List<Vector3> waypoints = new List<Vector3>();


		for (int i = 1; i <= finalPath.Count; i++)
		{
			waypoints.Add(finalPath[i-1].worldPosition);	
		}
		return waypoints.ToArray();
	}

	public Node RemoveFirst()
	{
		Node nodeToRemove = minPQ[0];
		foreach(Node n in minPQ){
			if (n.totalCost < nodeToRemove.totalCost)
			{
				nodeToRemove = n;
			}
		}
		minPQ.Remove(nodeToRemove);
		return nodeToRemove;
	}

	public void clearInPath()
	{
		foreach (Node n in grid.grid)
		{
			n.isInPath = false;
		}
	}

	public void clearInRange()
	{
		foreach (Node n in grid.grid)
		{
			n.inRange = false;
		}
	}

	public void clearInTacklekRange()
	{
		foreach (Node n in grid.grid)
		{
			n.inTackleRange = false;
			n.mouseOverNode = false;
		}
	}

}
