using UnityEngine;
using System.Collections;

public class DayLight : MonoBehaviour {
	
	public Color dayColor;
	public Color nightColor;
	public Color sunriseColor;
	public Color sunsetColor;
	
	void lightColors (float t)
	{
		//0 is midnight, .5 is always noon
		if (t < .25)
		{
			light.color = Color.Lerp (nightColor, sunriseColor , t * 4f);
		}
		else if(t < .5)
		{
			light.color = Color.Lerp (sunriseColor, dayColor, (t - .25f) * 4f);
		}
		else if(t < .75)
		{
			light.color = Color.Lerp (dayColor, sunsetColor, (t - .5f) * 4f);
		}
		else
		{
			light.color = Color.Lerp (sunsetColor, nightColor, (t - .75f) * 4f);
		}
	}
}
