using UnityEngine;
using System.Collections;
using System;

public class Sentry : MonoBehaviour {

	public GameObject arrowPrefab;
	public Transform arrowSpawn;

	DateTime lastShoot;

	TimeSpan shootCooldown = new TimeSpan(0, 0, 0, 0, 500);

	GameObject target;

	// Use this for initialization
	void Start ()
	{
		target = FindTarget();
	}

	GameObject FindTarget()
	{
		return GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update()
	{
		if (lastShoot == null || (DateTime.Now - lastShoot) >= shootCooldown)
		{
			GameObject arrow = Instantiate(arrowPrefab) as GameObject;
			Arrow arrowScript = arrow.GetComponent<Arrow>();
			arrow.transform.position = arrowSpawn.position;
			arrow.transform.rotation = arrowSpawn.rotation;
			Vector3 direction = target.transform.position - arrowSpawn.position;
			direction.Normalize();
			if (arrowScript)
				arrowScript.Shoot(direction);

			lastShoot = DateTime.Now;
		}
	}
}