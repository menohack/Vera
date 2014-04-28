using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; //Has many data structures

public class MeleeCollider : MonoBehaviour {

	/// <summary>
	/// The amount of damage done by the melee attack.
	/// </summary>
	public int DamageValue = 10;

	public float coolDownMillis = 1000f;

	private TimeSpan attackCooldown;
	DateTime? lastAttack;

	private TimeSpan delayCoolDown;
	public float delayCooldownMillis = 500f;
	DateTime? buildDelay;
	private Build b;


	public Animator animator;

	void Start () {
		attackCooldown = TimeSpan.FromMilliseconds(coolDownMillis);
		delayCoolDown = TimeSpan.FromMilliseconds(delayCooldownMillis);
		//This needs to be fixed for networking
		b = GameObject.FindGameObjectWithTag ("Player").GetComponent<Build>();
	}

	void Update () {
		if (b.hasBuilding) 
		{
			buildDelay = DateTime.Now;
		}

		if ((DateTime.Now - buildDelay) > delayCoolDown || buildDelay == null) 
		{
			if (Input.GetButtonDown("Fire1") && (lastAttack == null || (DateTime.Now - lastAttack) >= attackCooldown))
			{
				if (animator)
					animator.SetTrigger("Attack");
				lastAttack = DateTime.Now;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (animator && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {

			Health enemyHealth = null;
			if (other.gameObject.tag == "Enemy")
			{
				enemyHealth = other.gameObject.GetComponent<Health> ();
			}
			if (enemyHealth != null) {
				enemyHealth.Damage (DamageValue);
			}
		}
	}
}
