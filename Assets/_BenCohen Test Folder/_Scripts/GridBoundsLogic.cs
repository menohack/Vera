using UnityEngine;
using System.Collections;
using Pathfinding;

public class GridBoundsLogic : MonoBehaviour {

	void Start () {
		//Debug.Log ("original grid position is: " + gameObject.transform.position);
	}
	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			//Debug.Log ("player has left the bounding box");
			//want to recalculate grid base on players location
			//first move the bounding box
			Vector3 newPos = other.gameObject.transform.position;
			gameObject.transform.position = newPos;
			//Debug.Log ("new grid position is: " + gameObject.transform.position);

			//now move the gridgraph and rescan

//			GridGraph g = AstarPath.active.astarData.gridGraph;
//			g.center = gameObject.transform.position;
//			Matrix4x4 m = g.matrix;
//			g.GenerateMatrix();
//			g.RelocateNodes (m, g.matrix);
//			AstarPath.active.Scan ();
		}

	}

}
