using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	/// <summary>
	/// Amount of health.
	/// </summary>
	public int healthValue = 100;

	/// <summary>
	/// Gets damaged by the input amount.
	/// </summary>
	/// <param name="dmg">Dmg.</param>
	public void Damage(int dmg)
	{
		healthValue -= dmg;
		Debug.Log ("I am now at " +  healthValue + " HP!");
		if (healthValue <= 0)
		{
			Destroy (this.gameObject);
		}
	}
}
