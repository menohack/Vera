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
			Physics.IgnoreCollision(collider, arrow.collider);
			Arrow arrowScript = arrow.GetComponent<Arrow>();
			arrow.transform.position = arrowSpawn.position;
			arrow.transform.rotation = arrowSpawn.rotation;
			if (arrowScript)
				arrowScript.Shoot(target.transform.position);

			lastShoot = DateTime.Now;
		}
	}
}