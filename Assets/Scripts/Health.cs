using UnityEngine;
using System.Collections;
using Pathfinding;


public class Health : MonoBehaviour {

	/// <summary>
	/// Max health.
	/// </summary>
	public float maxHealth = 100f;

	/// <summary>
	/// Current health.
	/// </summary>
	float health = 100f;

	public bool direct = false; /** Flush Graph Updates directly after placing. Slower, but updates are applied immidiately */
	public bool issueGUOs = true; /** Issue a graph update object after destruction */

	void Start()
	{
		health = maxHealth;
	}

	/// <summary>
	/// Serializes and deserializes the Health script for network communication.
	/// </summary>
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			float healthC = health;
			float maxHealthC = maxHealth;
			bool directC = direct;
			bool issueGUOsC = issueGUOs;
			stream.Serialize(ref healthC);
			stream.Serialize(ref maxHealthC);
			stream.Serialize(ref directC);
			stream.Serialize(ref issueGUOsC);
		}
		else
		{
			float healthZ = 100f;
			float maxHealthZ = 100f;
			bool directZ = false;
			bool issueGUOsZ = true;
			stream.Serialize(ref healthZ);
			stream.Serialize(ref maxHealthZ);
			stream.Serialize(ref directZ);
			stream.Serialize(ref issueGUOsZ);
			health = healthZ;
			maxHealth = maxHealthZ;
			direct = directZ;
			issueGUOs = issueGUOsZ;
		}
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
			else if (gameObject.layer == LayerMask.NameToLayer("Obstacle"))
			{
				//Pathfinding.Console.Write ("// Placing Object\n");
				if (issueGUOs)
				{
					GraphUpdateObject guo = new GraphUpdateObject(gameObject.collider.bounds);
					AstarPath.active.UpdateGraphs(guo, 0.0f);
					if (direct)
					{
						//Pathfinding.Console.Write ("// Flushing\n");
						AstarPath.active.FlushGraphUpdates();
					}
				}
				Utility.DestroyHelper(this.gameObject);
			}
			else
				Utility.DestroyHelper(this.gameObject);
		}
	}

	public void Heal(float heal)
	{
		if (health < maxHealth)
		{
			health += heal;
			if (health > maxHealth) {health = maxHealth;}
		}
	}

	public float GetHealth()
	{
		return health;
	}
}
