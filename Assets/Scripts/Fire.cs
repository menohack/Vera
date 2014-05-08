using UnityEngine;
using System.Collections;

/// <summary>
/// The Fire class is used to control the flame on campfires.
/// </summary>
public class Fire : MonoBehaviour
{
	/// <summary>
	/// The flame GameObject.
	/// </summary>
	public GameObject flame;

	/// <summary>
	/// Adds the Extinguish and Light functions to the dawn and dusk event listener so that the flame
	/// turns on at night and off during the day.
	/// </summary>
	void Start()
	{
		Sundial sundial = FindObjectOfType<Sundial>();
		if (sundial)
		{
			sundial.dawnListener += new Sundial.EventListener(Extinguish);
			sundial.duskListener += new Sundial.EventListener(Light);
		}
		if (sundial.GetProgress () > 0.5) 
		{
			flame.SetActive(true);
		}
	}

	/// <summary>
	/// Extinguishes the flames.
	/// </summary>
	void Extinguish()
	{
		if (flame)
			flame.SetActive(false);
	}

	/// <summary>
	/// Lights the flames.
	/// </summary>
	void Light()
	{
		if (flame)
			flame.SetActive(true);
	}
}
