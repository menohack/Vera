using UnityEngine;
using System.Collections;

public class DayNight : MonoBehaviour {

	/// <summary>
	/// The timer.
	/// </summary>
	public float timer;

	public float timeInSeconds = 0;

	/// <summary>
	/// True if it is daytime. False otherwise.
	/// </summary>
	private bool isDay = false;

	void Update () {
		timeInSeconds += Time.deltaTime; //just to let us know how many seconds have passed
	}

	void IsDay(bool b)
	{
		isDay = b;
	}

	void timeCheck(float t)
	{
		timer = t;
	}
	
}
