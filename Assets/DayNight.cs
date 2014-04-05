using UnityEngine;
using System.Collections;

public class DayNight : MonoBehaviour {

	/// <summary>
	/// The timer.
	/// </summary>
	private float timer = 0;

	private float timeInSeconds = 0;

	public Color dayColor;
	public Color nightColor;
	public Color sunriseColor;
	public Color sunsetColor;

	/// <summary>
	/// The length of the day in seconds
	/// </summary>
	public float dayLength = 100;
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime / dayLength;
		timeInSeconds += Time.deltaTime; //just to let us know how many seconds have passed

		float time = timer - Mathf.Floor (timer);
		//0 is midnight, .5 is always noon
		if (time < .25)
		{
			light.color = Color.Lerp (nightColor, sunriseColor , time * 4f);
		}
		else if(time < .5)
		{
			light.color = Color.Lerp (sunriseColor, dayColor, (time - .25f) * 4f);
		}
		else if(time < .75)
		{
			light.color = Color.Lerp (dayColor, sunsetColor, (time - .5f) * 4f);
		}
		else
		{
			light.color = Color.Lerp (sunsetColor, nightColor, (time - .75f) * 4f);
		}
	}
}
