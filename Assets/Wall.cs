using UnityEngine;
using System.Collections;

public class Wall : Building
{
	/// <summary>
	/// The minimum distance for a wall to be considered for attaching.
	/// </summary>
	public float minAttachDistance = 3.0f;

	/// <summary>
	/// The wood cost to build a wall.
	/// </summary>
	public static int WALL_COST_WOOD = 1;

	/// <summary>
	/// The ore cost to build a wall.
	/// </summary>
	public static int WALL_COST_ORE = 1;
}
