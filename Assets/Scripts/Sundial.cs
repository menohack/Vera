using UnityEngine;
using System.Collections;

public class Sundial : MonoBehaviour
{

	/// <summary>
	/// The day.
	/// </summary>
	int day = 0;

	/// <summary>
	/// The time of day in seconds.
	/// </summary>
	float time = 0f;

	/// <summary>
	/// The length of a day in seconds.
	/// </summary>
	public float dayLengthSeconds = 60f;

	/// <summary>
	/// The length of time that the prompt has been visible.
	/// </summary>
	float promptTime = 0f;

	/// <summary>
	/// The length of time for which the new day prompt should display.
	/// </summary>
	public float promptLength = 5f;
	
	void Update ()
	{
		float newTime = time + Time.deltaTime;
		if (newTime >= dayLengthSeconds)
		{
			time = newTime - dayLengthSeconds;
			day++;
			promptTime = time;
		}
		else
			time = newTime;
	}

	/// <summary>
	/// Gets the time of day as a value between 0 and 1.
	/// </summary>
	/// <returns>The time of day.</returns>
	public float GetProgress()
	{
		return time / dayLengthSeconds;
	}

	float fadeIn = 2f, pause = 1f, fadeOut = 2f;

	/// <summary>
	/// Prompt for the new day.
	/// </summary>
	void OnGUI()
	{
		if (promptTime < promptLength)
		{
			float labelWidth = 400f;
			float labelHeight = 200f;
			GUILayout.BeginArea(new Rect(Screen.width / 2.0f - labelWidth / 2.0f, Screen.height / 2.0f - labelHeight / 2.0f, labelWidth, labelHeight));

			GUIStyle labelStyle = new GUIStyle();

			//Fade the prompt in for fadeIn seconds, pause for pause seconds, then fade out over fadeOut seconds
			float fadeAlpha;
			if (promptTime < fadeIn)
				fadeAlpha = promptTime / fadeIn;
			else if (promptTime < fadeIn + pause)
				fadeAlpha = 1f;
			else
				fadeAlpha = (fadeOut - (promptTime - (fadeIn + pause))) / fadeOut;

			guiText.color = new Color(0f, 0f, 0f, fadeAlpha);

			GUILayout.Label("Day " + day);
			GUILayout.EndArea();

			promptTime += Time.deltaTime;
		}
	}
}
