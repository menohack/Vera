﻿using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public GameObject flame;

	void Start()
	{
		Sundial sundial = FindObjectOfType<Sundial>();
		if (sundial)
		{
			sundial.dawnListener += new Sundial.EventListener(Extinguish);
			sundial.fireListener += new Sundial.EventListener(Light);
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
