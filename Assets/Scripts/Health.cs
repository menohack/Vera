using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	/// <summary>
	/// Max health.
	/// </summary>
	public float maxHealth = 100f;

	/// <summary>
	/// Current health.
	/// </summary>
	float health = 100f;

	void Start()
	{
		health = maxHealth;
	}

	/// <summary>
	/// Gets damaged by the input amount.
	/// </summary>
	/// <param name="dmg">Dmg.</param>
	public void Damage(float dmg)
	{
		health -= dmg;
		if (health <= 0)
		{
			health = 0f;
			if (gameObject.tag == "Player")
				Menu.EndGame();
			else
				Destroy(this.gameObject);
		}
	}

	public float GetHealth()
	{
		return health;
	}
}
