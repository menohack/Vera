using UnityEngine;
using System.Collections;
using System;

public class Arrow : MonoBehaviour {

	public float ARROW_DAMAGE = 50f;

	public float MAX_VELOCITY = 20f;

	DateTime birthTime;

	public TimeSpan liveTime = new TimeSpan(0, 0, 0, 5);

	bool shot = false;
	bool hit = false;

	GameObject target;

	void Start ()
	{
		birthTime = DateTime.Now;
	}
	

	void Update () {
		if (DateTime.Now - birthTime >= liveTime)
			Utility.DestroyHelper(gameObject);
		if (target == null && shot)
			Utility.DestroyHelper(gameObject);


		if (!hit && shot && target != null)
		{
			Vector3 targetTransform = target.collider.bounds.center;
			if (target.tag == "Enemy")
				targetTransform += new Vector3(0f, -0.5f * target.collider.bounds.extents.y, 0f);
			transform.LookAt(targetTransform);
			Vector3 direction = targetTransform - transform.position;
			direction.Normalize();
			transform.position += direction * MAX_VELOCITY * Time.deltaTime;
		}
	}

	/// <summary>
	/// Shoots an arrow at a target GameObject. The arrow will follow the target magically.
	/// </summary>
	/// <param name="target">The target to hit.</param>
	public void Shoot(GameObject target)
	{
		if (shot)
			throw new UnityException("Arrow has already been shot!");
		if (target == null)
			throw new UnityException("Target is null!");
		shot = true;
		this.target = target;
	}

	void OnTriggerEnter(Collider collision)
	{
		if (hit)
			return;

		Health health = collision.gameObject.GetComponent<Health>();
		if (health && collision.gameObject.tag == "Enemy")
		{
			gameObject.transform.parent = collision.gameObject.transform;
			health.Damage(ARROW_DAMAGE);
		}
			
		Destroy(rigidbody);
		hit = true;
	}
}
