using UnityEngine;
using System.Collections;
using System;

public class Arrow : MonoBehaviour {

	public float MAX_SHOOT_FORCE = 1000f;
	public float TIP_FORCE = 10f;

	public Transform forcePosition;

	DateTime birthTime;

	public TimeSpan liveTime = new TimeSpan(0, 0, 0, 5);

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
		transform.rotation = Quaternion.FromToRotation(transform.forward, direction);
		
		rigidbody.AddForce(direction * MAX_SHOOT_FORCE);
		if (forcePosition)
			rigidbody.AddForceAtPosition(Vector3.up * TIP_FORCE, forcePosition.position);
		else
			throw new UnityException("Arrow script cannot find forcePosition");
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			gameObject.transform.parent = collision.gameObject.transform;
			rigidbody.isKinematic = true;
		}
	}

	void OnCollisionExit(Collision collision)
	{
	}
}
