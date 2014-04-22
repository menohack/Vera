using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Targeting
{
	public static GameObject FindClosestTarget(Transform origin, string tag, float range=float.PositiveInfinity, float awarenessRange=-1)
	{
		List<GameObject> targetList = new List<GameObject>(GameObject.FindGameObjectsWithTag(tag));
		targetList = targetList.Where (target => WithinRange(origin, target.transform, range, awarenessRange)).ToList();

		if (targetList.Count == 0)
			return null;
		else if (targetList.Count == 1)
			return targetList.First ();
		else { //returns min
			return GetClosest(origin, targetList);
		}
	}

	//awarenessRange is for the dot product. -1 implies all around (circle), 0 implies forward + sides (semicircle), 1 is strictly forward, and values in between are in between these areas
	private static bool WithinRange(Transform origin, Transform target, float range=float.PositiveInfinity, float awarenessRange=0) 
	{
		return WithinDist (origin, target, range) && WithinAngle (origin, target, awarenessRange);
	}

	//returns true if within the distance of attack
	private static bool WithinDist(Transform origin, Transform target, float range=float.PositiveInfinity) 
	{
		return Vector3.Distance (target.position, origin.position) <= range;
	}

	//awarenessRange is for the dot product. -1 implies all around (circle), 0 implies forward + sides (semicircle), 1 is strictly forward, and values in between are in between these areas
	private static bool WithinAngle(Transform origin, Transform target, float awarenessRange=0) 
	{
		Vector3 heading = Vector3.Normalize(target.position - origin.position);
		float dot = Vector3.Dot (heading, origin.forward);
		return awarenessRange <= dot; 
	}

	//fold list to return the enemy at closest distance
	private static GameObject GetClosest(Transform origin, List<GameObject> targetList) 
	{
		return targetList.Aggregate ((x, y) => Vector3.Distance (x.transform.position, origin.position) < Vector3.Distance (y.transform.position, origin.position) ? x : y);
	}

}
