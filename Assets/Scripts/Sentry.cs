using UnityEngine;
using System.Collections;
using System;

public class Sentry : Building {

	/// <summary>
	/// The arrow prefab.
	/// </summary>
	public GameObject arrowPrefab;

	/// <summary>
	/// The top point of a triangle from which arrows spawn.
	/// </summary>
	public Transform arrowSpawn1;

	/// <summary>
	/// The left point of a triangle from which arrows spawn.
	/// </summary>
	public Transform arrowSpawn2;

	/// <summary>
	/// The right point of a triangle from which arrows spawn.
	/// </summary>
	public Transform arrowSpawn3;

	public int shootCooldownSec = 1;
	public int shootCooldownMilli = 500;

	//attack distance is the length distance away from origin the sentry can attack
	public float attackDistance = 5.0f;

	//attack range is the arc range within which the sentry can attack
	// all values need to be between [-1,1], as its based on the dot product
	// an arc of 1 means *only* directly in front, and arc of 0 means a semi-circle  from the front,, an arc of -1 means a full circle
	// by example, an arc of .5 would mean an arc from -45 to 45 degrees about the sentries origin
	public float attackArc = 0;

	/// <summary>
	/// The last time an arrow was fired.
	/// </summary>
	DateTime lastShoot;

	/// <summary>
	/// The minimum time between shots.
	/// </summary>
	TimeSpan shootCooldown;

	/// <summary>
	/// The current target.
	/// </summary>
	GameObject target = null;

	/// <summary>
	/// The number of milliseconds to wait before searching for a target.
	/// </summary>
	public float searchFrequencyMilliseconds = 500f;

	/// <summary>
	/// The TimeSpan version of searchFrequencyMilliseconds.
	/// </summary>
	TimeSpan searchFrequency;

	/// <summary>
	/// The last time that a search for a target happened.
	/// </summary>
	DateTime lastSearch = DateTime.Now;

	/// <summary>
	/// The wood cost to build a sentry.
	/// </summary>
	public static int SENTRY_COST_WOOD = 4;

	/// <summary>
	/// The ore cost to build a sentry.
	/// </summary>
	public static int SENTRY_COST_ORE = 4;

	public override int GetOreCost()
	{
		return SENTRY_COST_ORE;
	}

	public override int GetWoodCost()
	{
		return SENTRY_COST_WOOD;
	}

	protected override void Start()
	{
		base.Start();
		shootCooldown = new TimeSpan (0, 0, 0, shootCooldownSec, shootCooldownMilli);
		searchFrequency = TimeSpan.FromMilliseconds(searchFrequencyMilliseconds);
		target = Targeting.FindClosestTarget(transform, "Enemy", attackDistance, attackArc);
		lastSearch = DateTime.Now;
	}


	protected override void Update()
	{
		if (!placed)
			base.Update();
		else
		{
			if (DateTime.Now - lastSearch > searchFrequency)
			{
				target = Targeting.FindClosestTarget(transform, "Enemy", attackDistance, attackArc);
				lastSearch = DateTime.Now;
			}

			if (target != null && (lastShoot == null || (DateTime.Now - lastShoot) >= shootCooldown))
			{
				GameObject arrow = Utility.InstantiateHelper(arrowPrefab);
				Physics.IgnoreCollision(collider, arrow.collider);
				Arrow arrowScript = arrow.GetComponent<Arrow>();

				Vector2 randomValues = new Vector2(UnityEngine.Random.value, UnityEngine.Random.value);
				randomValues.Normalize();
				Vector3 position = arrowSpawn1.position + randomValues.x * (arrowSpawn2.position - arrowSpawn1.position)
					+ randomValues.y * (arrowSpawn3.position - arrowSpawn1.position);
				arrow.transform.position = position;

				arrow.transform.rotation = arrowSpawn1.rotation;
				if (arrowScript)
					arrowScript.Shoot(target);

				lastShoot = DateTime.Now;
			}
		}
	}
}