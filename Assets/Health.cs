using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	/// <summary>
	/// Amount of health.
	/// </summary>
	public float healthValue = 100.0f;

	/// <summary>
	/// Gets damaged by the input amount.
	/// </summary>
	/// <param name="dmg">Dmg.</param>
	public void Damage(float dmg)
	{
		healthValue -= dmg;
		Debug.Log ("I am now at " +  healthValue + " HP!");
		if (healthValue <= 0)
		{
			Destroy (this.gameObject);
		}
	}
}
