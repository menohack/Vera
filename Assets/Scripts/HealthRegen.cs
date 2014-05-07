using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; 

public class HealthRegen : MonoBehaviour {

	public int hpRegen = 5;
	public int coolDownMilli = 5000; //how often
	public bool DEBUG = false;

	private TimeSpan regenCooldown;
	Building parentBuilding;

	DateTime? lastHeal;

	private HashSet<GameObject> players;

	void Start () {
		regenCooldown = new TimeSpan(0,0,0,0, coolDownMilli);
		players = new HashSet<GameObject>();
		parentBuilding = gameObject.transform.parent.gameObject.GetComponent<Building>();
	}
	
	void Update () {
		if (lastHeal == null || (DateTime.Now - lastHeal) >= regenCooldown && parentBuilding != null && parentBuilding.placed)
		{
			foreach (GameObject p in players)
			{
				Health hp = p.GetComponent<Health>();
				hp.Heal(hpRegen);
			}
			lastHeal = DateTime.Now;

		}
		
	}

	void OnTriggerEnter(Collider other) {
		GameObject whoami = other.gameObject;
		if (whoami.tag == "Player" && !players.Contains(whoami))
		{
			players.Add (whoami);
		}
	}

	void OnTriggerExit(Collider other) {
		GameObject whoami = other.gameObject;
		if (whoami.tag == "Player")
		{
			players.Remove(whoami);
		}
	}




}
