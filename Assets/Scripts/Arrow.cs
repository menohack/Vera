using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public float SHOOT_FORCE = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Shoot(Vector3 direction)
	{
		direction.Normalize();
		rigidbody.AddForce(direction * SHOOT_FORCE);
	}

	void OnCollisionEnter(Collision collision)
	{
		rigidbody.isKinematic = true;
	}

	void OnCollisionExit(Collision collision)
	{
	}
}
