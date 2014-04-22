using UnityEngine;
using System.Collections;

public class Targeting
{

	public static GameObject FindClosestTarget(Vector3 position, string tag, float range)
	{
		GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
		GameObject target = null;

		if (targets.Length > 0)
		{
			target = targets[0];
			float minDistance = Vector3.Distance(targets[0].gameObject.transform.position, position);
			for (int i = 1; i < targets.Length; i++)
			{
				float distance = Vector3.Distance(targets[i].gameObject.transform.position, position);
				if (distance < minDistance)
				{
					minDistance = distance;
					target = targets[i];
				}
			}
			if (minDistance > range) //if the closest target found is still out of range, don't target anything
				target = null;
		}

		return target;
	}
}
