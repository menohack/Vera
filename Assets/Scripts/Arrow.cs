using UnityEngine;
using System.Collections;
using System;

public class Arrow : MonoBehaviour {

	public float SHOOT_FORCE = 10f;

	DateTime birthTime;

	TimeSpan liveTime = new TimeSpan(0, 0, 0, 5);

	// Use this for initialization
	void Start ()
	{
		birthTime = DateTime.Now;
	}
	
	// Update is called once per frame
	void Update () {
		if (DateTime.Now - birthTime >= liveTime)
			Destroy(gameObject);
	}

	public void Shoot(Vector3 direction)
	{
		direction.Normalize();
		rigidbody.AddForce(direction * SHOOT_FORCE);
	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Arrow hit");
		rigidbody.isKinematic = true;
	}

	void OnCollisionExit(Collision collision)
	{
	}
}
