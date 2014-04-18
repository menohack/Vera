using UnityEngine;
using System.Collections;
using System;

public class Arrow : MonoBehaviour {

	public float MAX_SHOOT_FORCE = 10f;
	public float TIP_FORCE = 10f;
	public float ARROW_DAMAGE = 5f;

	public float MAX_VELOCITY = 10f;

	public Transform forcePosition;

	DateTime birthTime;

	public TimeSpan liveTime = new TimeSpan(0, 0, 0, 5);

	bool chaseTarget = true;
	bool shot = false;
	bool hit = false;

	GameObject target;

	// Use this for initialization
	void Start ()
	{
		birthTime = DateTime.Now;
		rigidbody.detectCollisions = false;
		if (chaseTarget && rigidbody)
			Destroy(rigidbody);
	}
	
	// Update is called once per frame
	void Update () {
		if (DateTime.Now - birthTime >= liveTime)
			Destroy(gameObject);

		if (target && chaseTarget)
		{
			transform.LookAt(target.collider.bounds.center);
			Vector3 direction = target.collider.bounds.center - transform.position;
			direction.Normalize();
			transform.position += direction * MAX_VELOCITY * Time.deltaTime;
		}
	}

	public void Shoot(Vector3 direction)
	{
		if (shot)
			throw new UnityException("Arrow has already been shot!");
		chaseTarget = false;
		shot = true;

		direction.Normalize();
		transform.rotation = Quaternion.FromToRotation(transform.forward, direction);

		rigidbody.AddForce(direction * MAX_SHOOT_FORCE, ForceMode.VelocityChange);
		if (forcePosition)
			rigidbody.AddForceAtPosition(Vector3.up * TIP_FORCE, forcePosition.position);
		else
			throw new UnityException("Arrow script cannot find forcePosition");
	}

	public void Shoot(GameObject target, bool chase = true)
	{
		if (shot)
			throw new UnityException("Arrow has already been shot!");
		if (target == null)
			throw new UnityException("Target is null!");
		shot = true;
		chaseTarget = chase;
		this.target = target;
		if (!chaseTarget)
		{
			Vector3 targetPosition = target.transform.position;
			Vector3 direction = targetPosition - transform.position;
			direction.Normalize();
			transform.rotation = Quaternion.FromToRotation(transform.forward, direction);

			float deltaX = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetPosition.x, targetPosition.z));
			float deltaY = targetPosition.y - transform.position.y;
			float vMax = 300f;
			float g = 9.8f;
			float t1 = 2f / (g * g) * ((vMax - g * deltaY) + Mathf.Sqrt((vMax - g * deltaY) * (vMax - g * deltaY) - g * g * (deltaX * deltaX + deltaY * deltaY)));
			float t2 = 2f / (g * g) * ((vMax - g * deltaY) - Mathf.Sqrt((vMax - g * deltaY) * (vMax - g * deltaY) - g * g * (deltaX * deltaX + deltaY * deltaY)));

			float vx = deltaX / t1;
			float vy = Mathf.Sqrt(vMax * vMax - vx * vx);

			float theta = Mathf.Asin(vy / vMax);
			Debug.Log("theta: " + theta);

			Vector3 forwardDirection = new Vector3(direction.x, 0f, direction.z);
			forwardDirection.Normalize();
			forwardDirection.y = Mathf.Sin(theta);
			forwardDirection.Normalize();


			rigidbody.AddForce(forwardDirection * vMax, ForceMode.VelocityChange);
			if (forcePosition)
				rigidbody.AddForceAtPosition(Vector3.up * TIP_FORCE, forcePosition.position);
			else
				throw new UnityException("Arrow script cannot find forcePosition");
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (hit)
			return;

		Health health = collision.gameObject.GetComponent<Health>();
		if (health)
		{
			gameObject.transform.parent = collision.gameObject.transform;
			health.Damage(ARROW_DAMAGE);
		}
			
		Destroy(rigidbody);
		hit = true;
	}

	void OnCollisionExit(Collision collision)
	{
	}
}
