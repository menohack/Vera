using UnityEngine;
using System.Collections;

public class Light : MonoBehaviour {

	public GameObject sun;

	public Sundial sundial;

	public Color dayColor;
	public Color nightColor;
	public Color sunriseColor;
	public Color sunsetColor;

	void SetLightColors(float t)
	{
		Debug.Log("Derp");
		if (t < .25)
			light.color = Color.Lerp(sunriseColor, dayColor, t * 4f);
		else if (t < .5)
			light.color = Color.Lerp(dayColor, sunsetColor, (t - .25f) * 4f);
		else if (t < .75)
			light.color = Color.Lerp(sunsetColor, nightColor, (t - .5f) * 4f);
		else
			light.color = Color.Lerp(nightColor, sunriseColor, (t - .75f) * 4f);
	}

	void Update ()
	{
		if (sun)
		{
			Vector3 direction = sun.transform.position - Camera.main.transform.position;
			direction.Normalize();
			transform.rotation = Quaternion.LookRotation(-direction);

			SetLightColors(sundial.GetProgress());
		}
	}
}
