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
		if (t < .25)
			light.color = Color.Lerp(sunriseColor, sunriseColor, t * 4f);
		else if (t < .5)
			light.color = Color.Lerp(dayColor, dayColor, (t - .25f) * 4f);
		else if (t < .75)
			light.color = Color.Lerp(sunsetColor, sunsetColor, (t - .5f) * 4f);
		else
			light.color = Color.Lerp(nightColor, nightColor, (t - .75f) * 4f);
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
