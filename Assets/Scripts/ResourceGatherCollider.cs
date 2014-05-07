﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; //Has many data structures

public class ResourceGatherCollider : MonoBehaviour {

	public bool DEBUG = false;
	public int DamageValue = 50;

	public int numHitsOnSwing = 1;
	public int coolDownMilli = 1000; //TODO change this to a float and allow for 
	
	private TimeSpan attackCooldown;
	private DateTime? lastAttack;
	
	private TimeSpan delayCoolDown;
	private DateTime? buildDelay;
	private Build b;
	private Inventory inventory;
	
	
	public Animator animator;
	
	void Start () {
		attackCooldown = new TimeSpan(0,0,0,0, coolDownMilli);
		delayCoolDown = new TimeSpan (0, 0, 0, 0, 500);
	}
	
	void Update () {
		b = Spawn.GetMyPlayer().GetComponent<Build>();
		inventory = Spawn.GetMyPlayer().GetComponent<Inventory>();
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
			if (other.gameObject.tag == "Ore" || other.gameObject.tag == "Tree")
			{
				if (DEBUG) PrintCollided(other.gameObject.tag);
				//play relevant resource gathering sound on every hit
				gameObject.audio.Play ();
				//imported from old resource gathering code in Build.cs
				Resource resource = other.gameObject.GetComponent<Resource>();
				int gatherCount = resource.Gather(numHitsOnSwing);
				if (gatherCount > 0)
				{
					if (resource is Tree) {
						inventory.AddWood(gatherCount);
						if (DEBUG) PrintSuccess(other.gameObject.tag);
					}else if (resource is Ore) {
						inventory.AddOre(gatherCount);
						if (DEBUG) PrintSuccess(other.gameObject.tag);
					}else
						Debug.LogError("No such resource");
				}

			}
			else if (other.gameObject.tag == "Building")
			{
				if (DEBUG) PrintCollided(other.gameObject.tag);
				//play relevant resource gathering sound on every hit
				gameObject.audio.Play ();
				Health buildingHealth = other.gameObject.GetComponent<Health> ();
				if (buildingHealth != null) {
					buildingHealth.Damage (DamageValue);
					// on destroyed
					if (buildingHealth.GetHealth () <= 0) 
					{
						if (DEBUG) Debug.Log ("this is where we might have re-gathering code");
						if (DEBUG) PrintSuccess(other.gameObject.tag);
					}
				}
			}
		}
	}

	private void PrintCollided(String tag) {
		Debug.Log (tag + " collided with");
	}
	private void PrintSuccess(String tag) {
		Debug.Log (tag + " succesfully gathered");
	}
}