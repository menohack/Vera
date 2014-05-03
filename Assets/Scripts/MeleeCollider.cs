﻿using UnityEngine;
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


	public PlayerAnimation anime;

	void Start () {
		attackCooldown = TimeSpan.FromMilliseconds(coolDownMillis);
		delayCoolDown = TimeSpan.FromMilliseconds(delayCooldownMillis);
	}

	void Update () {
		b = Spawn.GetMyPlayer().GetComponent<Build>();
		if (b.hasBuilding) 
		{
			buildDelay = DateTime.Now;
		}

		if ((DateTime.Now - buildDelay) > delayCoolDown || buildDelay == null) 
		{
			if (Input.GetButtonDown("Fire1") && (lastAttack == null || (DateTime.Now - lastAttack) >= attackCooldown))
			{
				if (anime)
					anime.Attack();
				else
					Debug.Log("Animation missing from Player");
				lastAttack = DateTime.Now;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (anime != null && anime.GetAttacking()) {

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
