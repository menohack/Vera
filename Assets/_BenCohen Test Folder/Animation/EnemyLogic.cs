using UnityEngine;
using System.Collections;

public class EnemyLogic : MonoBehaviour {

	public int health = 100;

	void applyDamage(int damage) {
		health -= damage;
		//Debug.Log (gameObject + " took " + damage + " damage");
		
		if(health <= 0)
		{
			onDeath();
		}
	}

	void onDeath () {
		//Debug.Log (gameObject + " is dead");
		Destroy (this.gameObject);
	}

}
