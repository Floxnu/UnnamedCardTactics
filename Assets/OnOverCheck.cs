using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOverCheck : MonoBehaviour {

	public void OnImageOver() {
		Pathfinding.instance.clearInPath();
		SelectionStateManager.instance.canSelectPaths = false;
	}

	public void OnImageExit() {
		SelectionStateManager.instance.canSelectPaths = true;
	}
}
