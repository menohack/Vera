using UnityEngine;
using System.Collections;
using Pathfinding;


public class SeekerAI : AIPath {

	public override void OnTargetReached () {
		Debug.Log ("target reached");
		//End of path has been reached
		//If you want custom logic for when the AI has reached it's destination
		//add it here
		//You can also create a new script which inherits from this one
		//and override the function in that script
	}

}
