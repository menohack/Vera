using UnityEngine;
using System.Collections;

public class Wall : Building
{
	public float minAttachDistance = 5.0f;

	Wall right = null;
	Wall left = null;

	Transform oldParent = null;

	public override void Attach()
	{
		float min = minAttachDistance;
		Wall other = null;
		Vector3 newPosition = transform.position;

		foreach (Wall g in GameObject.FindObjectsOfType<Wall>())
		{
			Vector3[] attachPositions = new Vector3[2];
			attachPositions[0] = g.transform.position + g.transform.forward * 4.0f;
			attachPositions[1] = g.transform.position - g.transform.forward * 4.0f;

			if (g == this)
				continue;
			foreach (Vector3 attachPosition in attachPositions)
			{
				float distance = Vector3.Distance(attachPosition, transform.position);
				if (distance < min)
				{
					min = distance;
					other = g;
					newPosition = attachPosition;
				}
			}
		}

		if (other != null)
		{
			attached = true;
			transform.position = newPosition;
			transform.rotation = other.transform.rotation;
			other.right = this;
			oldParent = transform.parent;
			transform.parent = null;
			Debug.Log("Parent set to null");
		}
		else if (oldParent != null)
		{
			transform.parent = oldParent;
			Debug.Log("Parent reset");
		}
	}
}
