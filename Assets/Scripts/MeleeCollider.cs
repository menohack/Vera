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


	public PlayerAnimation anime;

	void Start () {
		attackCooldown = TimeSpan.FromMilliseconds(coolDownMillis);
	}

	void Update ()
	{
		GameObject player = Spawn.GetCurrentPlayer();
		if (player != null) 
		{
			Build buildScript = player.GetComponent<Build>();
			if (buildScript && !buildScript.HasBuilding())
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
