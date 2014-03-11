using UnityEngine;
using System.Collections;

public class BuildingHealth : MonoBehaviour {
	//simple health component
	private float health = 100f;

	public void Damage (float dmg)
	{
		health -= dmg;
	}

	public bool IsAlive ()
	{
		Debug.Log ("Remaining BuildingHP: " + health);
		if (health == 0) {return false;}
		else return true;
	}

}
