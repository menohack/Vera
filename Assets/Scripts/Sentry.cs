using UnityEngine;
using System.Collections;
using System;

public class Sentry : MonoBehaviour {

	public GameObject arrowPrefab;
	public Transform arrowSpawn;

	DateTime lastShoot;

	TimeSpan shootCooldown = new TimeSpan(0, 0, 0, 0, 500);

	GameObject target = null;

	public float searchFrequencyMilliseconds = 500f;

	TimeSpan searchFrequency;

	DateTime lastSearch = DateTime.Now;

	void Start()
	{
		searchFrequency = TimeSpan.FromMilliseconds(searchFrequencyMilliseconds);
	}

	void Update()
	{
		if (DateTime.Now - lastSearch < searchFrequency)
		{
			target = Targeting.FindClosestTarget(transform.position, "Enemy");
			lastSearch = DateTime.Now;
		}

		if (target != null && (lastShoot == null || (DateTime.Now - lastShoot) >= shootCooldown))
		{
			GameObject arrow = Instantiate(arrowPrefab) as GameObject;
			Physics.IgnoreCollision(collider, arrow.collider);
			Arrow arrowScript = arrow.GetComponent<Arrow>();
			arrow.transform.position = arrowSpawn.position;
			arrow.transform.rotation = arrowSpawn.rotation;
			Debug.Log("Shooting an arrow at " + target);
			if (arrowScript)
				arrowScript.Shoot(target);

			lastShoot = DateTime.Now;
		}
	}
}