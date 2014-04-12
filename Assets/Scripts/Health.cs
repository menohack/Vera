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
			else if (gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
				//Pathfinding.Console.Write ("// Placing Object\n");
				if (issueGUOs) {
					GraphUpdateObject guo = new GraphUpdateObject(gameObject.collider.bounds);
					AstarPath.active.UpdateGraphs (guo,0.0f);
					if (direct) {
						//Pathfinding.Console.Write ("// Flushing\n");
						AstarPath.active.FlushGraphUpdates();
					}
				}
				Destroy(this.gameObject);
			}

			else
				Destroy(this.gameObject);
		}
	}

	public float GetHealth()
	{
		return health;
	}
}
