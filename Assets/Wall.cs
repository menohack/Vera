using UnityEngine;
using System.Collections;

public class Wall : Building
{
	/// <summary>
	/// The minimum distance for a wall to be considered for attaching.
	/// </summary>
	public float minAttachDistance = 5.0f;

	/// <summary>
	/// The wall attached to the right.
	/// </summary>
	Wall right = null;

	/// <summary>
	/// The wall attached to the left.
	/// </summary>
	Wall left = null;

	/// <summary>
	/// The previous parent of the wall. Generally the player carrying it.
	/// </summary>
	Transform oldParent = null;

	/// <summary>
	/// A data structure that stores a position, rotation, and whether it is to the left or right
	/// of a given wall.
	/// </summary>
	public struct AttachPoint
	{
		public Vector3 position;
		public Quaternion rotation;
		public bool left;
	}

	/// <summary>
	/// Computes the possible positions that a wall may attach to another wall.
	/// </summary>
	/// <param name="w">The wall to which we are attempting to attach.</param>
	/// <returns>An array of potential positions.</returns>
	AttachPoint[] GetAttachPositions(Wall w)
	{
		AttachPoint[] attachPositions;

		if (w.left != null && w.right != null)
		{
			attachPositions = new AttachPoint[0];
			return attachPositions;
		}
		else if (w.left == null && w.right == null)
		{
			attachPositions = new AttachPoint[2];
			attachPositions[1].position = w.transform.position - w.transform.forward * 4.0f;
			attachPositions[1].rotation = w.transform.rotation;
			attachPositions[1].left = true;
			attachPositions[0].position = w.transform.position + w.transform.forward * 4.0f;
			attachPositions[0].rotation = w.transform.rotation;
			attachPositions[0].left = false;
			return attachPositions;
		}
		else if (w.right == null)
		{
			attachPositions = new AttachPoint[1];
			attachPositions[0].position = w.transform.position + w.transform.forward * 4.0f;
			attachPositions[0].rotation = w.transform.rotation;
			attachPositions[0].left = false;
			return attachPositions;
		}
		else
		{
			attachPositions = new AttachPoint[1];
			attachPositions[0].position = w.transform.position - w.transform.forward * 4.0f;
			attachPositions[0].rotation = w.transform.rotation;
			attachPositions[0].left = true;
			return attachPositions;
		}
	}

	/// <summary>
	/// Attempts to attach a wall to another wall.
	/// </summary>
	public override void Attach()
	{
		float min = minAttachDistance;
		AttachPoint? result = null;
		Wall parent = null;

		foreach (Wall w in GameObject.FindObjectsOfType<Wall>())
		{
			if (w == this)
				continue;

			AttachPoint[] attachPositions = GetAttachPositions(w);

			foreach (AttachPoint attachPoint in attachPositions)
			{
				float distance = Vector3.Distance(attachPoint.position, transform.position);
				if (distance < min)
				{
					min = distance;
					result = attachPoint;
					parent = w;
				}
			}
		}

		if (result != null)
		{
			attached = true;
			transform.position = result.Value.position;
			transform.rotation = result.Value.rotation;

			if (result.Value.left)
				parent.left = this;
			else
				parent.right = this;

			oldParent = transform.parent;
			transform.parent = null;
			Debug.Log("Parent set to null");
		}
		else if (oldParent != null)
		{
			attached = false;
			//transform.parent = oldParent;
			Debug.Log("Parent reset, result is null: " + (result!=null));
		}
	}
}
