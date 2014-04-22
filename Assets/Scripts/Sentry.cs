using UnityEngine;
using System.Collections;
using System;

public class Sentry : MonoBehaviour {

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
	public float attackDistance = 5.0f;

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

	void Start()
	{
		shootCooldown = new TimeSpan (0, 0, 0, shootCooldownSec, shootCooldownMilli);
		searchFrequency = TimeSpan.FromMilliseconds(searchFrequencyMilliseconds);
		target = Targeting.FindClosestTarget(transform.position, "Enemy", attackDistance);
		lastSearch = DateTime.Now;
	}

	void Update()
	{
		if (DateTime.Now - lastSearch > searchFrequency)
		{
			target = Targeting.FindClosestTarget(transform.position, "Enemy", attackDistance);
			lastSearch = DateTime.Now;
		}

		if (target != null && (lastShoot == null || (DateTime.Now - lastShoot) >= shootCooldown))
		{
			GameObject arrow = Instantiate(arrowPrefab) as GameObject;
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