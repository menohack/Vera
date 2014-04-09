using UnityEngine;
using System.Collections;
using Pathfinding;


public class SeekerAI : AIPath {

	/**
	 *  Range at which the AI becomes alert and starts following
	 **/
	public float alertRange = 10;
	public bool DEBUG = false;
	private bool enabled = true;

	public override void OnTargetReached () {
		gameObject.transform.LookAt (target);
		//End of path has been reached
		//If you want custom logic for when the AI has reached it's destination
		//add it here
		//You can also create a new script which inherits from this one
		//and override the function in that script
	}

	public override void Update () {
		//check distance, if too far, disable, otherwise enable
		base.Update ();
		float dist = getDistToTarget (); 
		if (dist <= alertRange && enabled == false) {
			if (DEBUG) Debug.Log ("dist is " + dist + ". Path reenabled.");
			OnEnable ();
			enabled = true;
		} else if (dist > alertRange && enabled == true) {
			if (DEBUG) Debug.Log ("dist is " + dist + ". Path disabled.");
			OnDisable ();
			enabled = false;
		}

	}

	protected float getDistToTarget () {
		return Vector3.Distance (gameObject.transform.position, target.position);
	}

}
