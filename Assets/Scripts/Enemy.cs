using UnityEngine;
using System.Collections;
using System;

public class Enemy : MonoBehaviour {

	public float MOVE_FORCE = 10f;
	public float MAX_SPEED = 20f;
	public float speed = 10.0f;

	public float minFollowDistance = 2.0f;

	public float maxFollowDistance = 20.0f;

	GameObject target = null;

	public float attackDistance = 5f;

	public TimeSpan attackCooldown = new TimeSpan(0, 0, 1);

	DateTime? lastAttack = null;

	public float attackDamage = 10.0f;

	int ignoreLayerMask;

	Vector3 attackStartPoint, attackEndPoint;

	public Boolean debug = false;

	// Use this for initialization
	void Start () {
		ignoreLayerMask = ~(1 << 10 | 1 << 11);
		Debug.Log(gameObject + " is alive");
	}

	void Update()
	{
		if (target)
		{
			attackStartPoint = transform.position + new Vector3(0, 1f, 0);
			if (target.collider)
			{
				CapsuleCollider targetCollider = target.collider as CapsuleCollider;
				if (targetCollider)
					attackEndPoint = target.transform.position + new Vector3(0f, targetCollider.height / 2.0f, 0f);
				else
					throw new UnityException("Enemy script expected a capsule collider on player");
				Attack();
			}
		}
	}

	/// <summary>
	/// Attacks the current target.
	/// </summary>
	void Attack()
	{
		Vector3 debugRay = attackEndPoint - attackStartPoint;
		debugRay.Normalize();
		debugRay *= attackDistance;

		if (target != null && (lastAttack == null || (DateTime.Now - lastAttack) >= attackCooldown))
		{
			if (IsTargetVisible())
			{
				float distance = Vector3.Distance(target.gameObject.transform.position, transform.position);
				if (distance < attackDistance)
				{
					Health health = target.GetComponent<Health>();
					if (health)
					{
						if (debug)
							Debug.DrawLine(attackStartPoint, attackEndPoint, Color.red, 1f);
						health.Damage(attackDamage);
						lastAttack = DateTime.Now;
					}
				}
				else if (debug)
					Debug.DrawLine(attackStartPoint, attackStartPoint + debugRay, Color.blue, 1f);
			}
			else if (debug)
				Debug.DrawLine(attackStartPoint, attackStartPoint + debugRay, Color.green, 1f);
		}
	}

	bool IsTargetVisible()
	{
		Vector3 ray = attackEndPoint - attackStartPoint;
		bool hit = Physics.Raycast(new Ray(attackStartPoint, ray), ray.magnitude, ignoreLayerMask);
		return !hit;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		target = Targeting.FindClosestTarget(transform.position, "Player");

		if (target)
		{
			float minDistance = Vector3.Distance(target.transform.position, transform.position);
			
			Vector3 direction = target.transform.position - transform.position;
			direction.Normalize();
			if (rigidbody && minDistance < maxFollowDistance && minDistance > minFollowDistance && transform.rigidbody.velocity.magnitude < MAX_SPEED)
				rigidbody.AddForce(direction * MOVE_FORCE * Time.fixedDeltaTime, ForceMode.VelocityChange);
		}	
	}
}
