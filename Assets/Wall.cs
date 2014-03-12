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
	/// Finds the closest attach point to the position of the wall. I.E. finds the closest potential placement
	/// for the wall based on its current position.
	/// </summary>
	/// <param name="parent"></param>
	/// <returns></returns>
	AttachPoint? GetClosestAttachPoint(out Wall parent)
	{
		float min = minAttachDistance;
		AttachPoint? result = null;
		parent = null;

		foreach (Wall w in GameObject.FindObjectsOfType<Wall>())
		{
			if (w == this)
			{
				continue;
			}

			AttachPoint[] attachPositions = GetAttachPositions(w);
			//Debug.Log("AttachPoints: " + attachPositions.Length);
			foreach (AttachPoint attachPoint in attachPositions)
			{
				float distance = Vector3.Distance(attachPoint.position, transform.position);
				Debug.Log(distance);
				if (distance < min)
				{
					Debug.Log("Found one");
					min = distance;
					result = attachPoint;
					parent = w;
				}
			}
		}
		return result;
	}

	/// <summary>
	/// Attempts to attach a wall to another wall.
	/// </summary>
	public override bool GhostAttach()
	{
		Wall parent = null;
		AttachPoint? result = GetClosestAttachPoint(out parent);


		if (result != null)
		{
			attached = true;

			transform.parent = null;
			Destroy(floatPoint);

			Debug.Log("Setting position and rotation to " + result.Value.position + " and " + result.Value.rotation);
			transform.position = result.Value.position;
			transform.rotation = result.Value.rotation;

			if (result.Value.left)
				parent.left = this;
			else
				parent.right = this;


			//Debug.Log("Parent set to null");
			return true;
		}
		else
		{
			attached = false;
			transform.localPosition = new Vector3(0f, 2.0f, 3.0f);
			transform.localRotation = Quaternion.Euler(0, 90, 0);
			//Debug.Log("Parent reset, result is null: " + (result!=null));
			return false;
		}
	}
}
