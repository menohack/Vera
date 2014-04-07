using UnityEngine;

public class Wall : Building
{
	/// <summary>
	/// The wood cost to build a wall.
	/// </summary>
	public static int WALL_COST_WOOD = 1;

	/// <summary>
	/// The ore cost to build a wall.
	/// </summary>
	public static int WALL_COST_ORE = 1;

	public float SCALE = 2f;

	Transform held;

	public override void SetGhostPosition(Transform heldPosition)
	{
		held = heldPosition;
	}

	protected override void Update()
	{
		if (held && !placed)
			transform.position = new Vector3(Mathf.Floor(held.position.x / SCALE) * SCALE, held.position.y, Mathf.Floor(held.position.z / SCALE) * SCALE);
		base.Update();
	}
}
