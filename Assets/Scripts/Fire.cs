using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public GameObject flame;

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

	void Extinguish()
	{
		if (flame)
			flame.SetActive(false);
	}

	void Light()
	{
		if (flame)
			flame.SetActive(true);
	}
}
