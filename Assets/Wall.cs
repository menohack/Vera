using UnityEngine;
using System.Collections;

public class Wall : Building
{
	/// <summary>
	/// The minimum distance for a wall to be considered for attaching.
	/// </summary>
	public float minAttachDistance = 3.0f;

	/// <summary>
	/// The wall attached to the right.
	/// </summary>
	public Wall right = null;

	/// <summary>
	/// The wall attached to the left.
	/// </summary>
	public Wall left = null;


	/// <summary>
	/// A data structure that stores a position, rotation, and whether it is to the left or right
	/// of a given wall.
	/// </summary>
	public struct AttachPoint
	{
		public AttachPoint(Vector3 position, Quaternion rotation, bool left)
		{
			this.position = position;
			this.rotation = rotation;
			this.left = left;
		}
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
		//THESE SHOULD NOT CHANGE
		Vector3 originalPos = transform.position;
		Quaternion originalRotation = w.transform.rotation;

		Vector3 rightPosition = w.transform.position + w.transform.forward * 4.0f ;
		Vector3 leftPosition = w.transform.position - w.transform.forward * 4.0f ;

		//45Forward
		transform.position = leftPosition;
		transform.rotation = w.transform.rotation;
		transform.RotateAround (w.transform.position - w.transform.forward * 2.0f, Vector3.up, 45.0f);
		AttachPoint leftCenterForward = new AttachPoint (transform.position, transform.rotation, true);
		AttachPoint leftUpForward = new AttachPoint (transform.position + w.transform.up * 1.0f, transform.rotation, true);
		AttachPoint leftDownForward = new AttachPoint (transform.position - w.transform.up * 1.0f, transform.rotation, true);

		transform.rotation = w.transform.rotation;
		transform.position = rightPosition;
		transform.RotateAround (w.transform.position + w.transform.forward * 2.0f, Vector3.up, -45.0f);
		AttachPoint rightCenterForward = new AttachPoint (transform.position, transform.rotation, false);
		AttachPoint rightUpForward = new AttachPoint (transform.position + w.transform.up * 1.0f, transform.rotation, false);
		AttachPoint rightDownForward = new AttachPoint (transform.position - w.transform.up * 1.0f, transform.rotation, false);

		//set back to normal
		transform.rotation = originalRotation;
		transform.position = originalPos;

		//Straight On
		AttachPoint leftCenter = new AttachPoint(w.transform.position - w.transform.forward * 4.0f, w.transform.rotation, true);
		AttachPoint leftUp = new AttachPoint(w.transform.position - w.transform.forward * 4.0f + w.transform.up * 1.0f, w.transform.rotation, true);
		AttachPoint leftDown = new AttachPoint(w.transform.position - w.transform.forward * 4.0f - w.transform.up * 1.0f, w.transform.rotation, true);

		AttachPoint rightCenter = new AttachPoint(w.transform.position + w.transform.forward * 4.0f, w.transform.rotation, false);
		AttachPoint rightUp = new AttachPoint(w.transform.position + w.transform.forward * 4.0f + w.transform.up * 1.0f, w.transform.rotation, false);
		AttachPoint rightDown = new AttachPoint(w.transform.position + w.transform.forward * 4.0f - w.transform.up * 1.0f, w.transform.rotation, false);

		//45Back
		transform.position = leftPosition;
		transform.rotation = w.transform.rotation;
		transform.RotateAround (w.transform.position - w.transform.forward * 2.0f, Vector3.up, -45.0f);
		AttachPoint leftCenterBack = new AttachPoint (transform.position, transform.rotation, true);
		AttachPoint leftUpBack = new AttachPoint (transform.position + w.transform.up * 1.0f, transform.rotation, true);
		AttachPoint leftDownBack = new AttachPoint (transform.position - w.transform.up * 1.0f, transform.rotation, true);
		
		transform.rotation = w.transform.rotation;
		transform.position = rightPosition;
		transform.RotateAround (w.transform.position + w.transform.forward * 2.0f, Vector3.up, 45.0f);
		AttachPoint rightCenterBack = new AttachPoint (transform.position, transform.rotation, false);
		AttachPoint rightUpBack = new AttachPoint (transform.position + w.transform.up * 1.0f, transform.rotation, false);
		AttachPoint rightDownBack = new AttachPoint (transform.position - w.transform.up * 1.0f, transform.rotation, false);

		//Both positions are filled
		if (w.left != null && w.right != null)
		{
			AttachPoint[] attachPositions = new AttachPoint[0];
			return attachPositions;
		}

		//Both positions are empty
		else if (w.left == null && w.right == null)
		{
			AttachPoint[] attachPositions = { leftCenterBack, leftUpBack, leftDownBack, leftCenterForward, leftUpForward, leftDownForward, leftCenter, leftUp, leftDown, rightCenter, rightUp, rightDown, rightCenterForward, rightUpForward, rightDownForward,rightCenterBack, rightUpBack, rightDownBack };
			return attachPositions;
		}

		//The right position is open
		else if (w.right == null)
		{
			AttachPoint[] attachPositions = { rightCenter, rightUp, rightDown, rightCenterForward, rightUpForward, rightDownForward,rightCenterBack, rightUpBack, rightDownBack };
			return attachPositions;
		}

		//The left position is open
		else
		{
			AttachPoint[] attachPositions = { leftCenterBack, leftUpBack, leftDownBack, leftCenterForward, leftUpForward, leftDownForward, leftCenter, leftUp, leftDown };
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
				continue;

			AttachPoint[] attachPositions = GetAttachPositions(w);
			foreach (AttachPoint attachPoint in attachPositions)
			{
				float distance = Vector3.Distance(attachPoint.position, floatPoint.transform.position);
				//checks within distance and if not already filled
				if (distance < min)
				{
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

		if (result == null)
		{
			attached = false;
			transform.parent = floatPoint.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			left = right = null;
			return false;
		}
		else
		{
			attached = true;
			transform.parent = null;
			transform.position = result.Value.position;
			transform.rotation = result.Value.rotation;
			this.right = null;
			this.left = null;
			if (result.Value.left)
				this.right = parent;
			else
				this.left = parent;
			return true;
		}
	}

	/// <summary>
	/// Attempts to set the wall to a parent. (This is called in Build.cs in PlaceItem()
	/// </summary>
	/// <returns><c>true</c>, if wall was set, <c>false</c> otherwise.</returns>
	public void SetWall()
	{
		if ((this.right != null) && (this.right.left == null))
		{
			this.right.left = this;
			Debug.Log ("On The Right Of Parent");
		}
		if ((this.left != null) && (this.left.right == null))
		{
			this.left.right = this;
			Debug.Log ("On The Left Of Parent");
		}
	}

	public void DestroyUpdate()
	{
		if (this.right != null)
			this.right.left = null;
		if (this.left != null)
			this.left.right = null;
	}

}
