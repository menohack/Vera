using UnityEngine;
using System.Collections;

public class Sundial : MonoBehaviour
{
	/// <summary>
	/// The day.
	/// </summary>
	int day = 1;

	/// <summary>
	/// The time of day in seconds.
	/// </summary>
	float time = 0f;

	/// <summary>
	/// The length of a day in seconds.
	/// </summary>
	public float dayLengthSeconds = 60f;

	/// <summary>
	/// The length of time for which the new day prompt should display.
	/// </summary>
	public float promptLength = 5f;

	/// <summary>
	/// The time over which the prompt fades in linearly.
	/// </summary>
	public float fadeIn = 1f;

	/// <summary>
	/// The time over which the font pauses fully visible after the fade in.
	/// </summary>
	public float pause = 3f;

	/// <summary>
	/// The time over which the font fades out after the pause.
	/// </summary>
	public float fadeOut = 1f;

	/// <summary>
	/// The font used for the prompt.
	/// </summary>
	public Font promptFont;

	/// <summary>
	/// The spawn GameObject.
	/// </summary>
	public Spawn spawn;

	/// <summary>
	/// The time of day between 0 and 1 at which to spawn wolves.
	/// </summary>
	public float spawnWolvesScalar = 0.5f;

	/// <summary>
	/// Have the wolves been spawned today?
	/// </summary>
	bool spawned = false;

	/// <summary>
	/// The day skybox.
	/// </summary>
	public Material daySkybox;

	/// <summary>
	/// The night skybox.
	/// </summary>
	public Material nightSkybox;

	/// <summary>
	/// The time of day between 0 and 1 when night falls.
	/// </summary>
	public float nightScalar = 0.5f;
	
	void Update ()
	{
		float newTime = time + Time.deltaTime;
		if (newTime >= dayLengthSeconds)
		{
			time = newTime - dayLengthSeconds;
			day++;
			spawned = false;
			spawn.DespawnWolves();
		}
		else
			time = newTime;

		if (GetProgress() > spawnWolvesScalar && !spawned && spawn)
		{
			spawn.SpawnWolves();
			spawned = true;
		}

		float skyboxBlend;
		//time:  0 middle, 0.25 day, 0.5 middle, 0.75 night, 1 == 0
		//blend: 0.5		0			0.5			1			0.5
		if (GetProgress() < 0.25f)
			skyboxBlend = 0.5f - 2.0f * GetProgress();
		else if (GetProgress() < 0.5f)
			skyboxBlend = (GetProgress() - 0.25f) * 2.0f;
		else if (GetProgress() < 0.75f)
			skyboxBlend = (GetProgress() - 0.5f) * 2.0f + GetProgress();
		else
			skyboxBlend = 1.0f - (GetProgress() - 0.75f) * 2.0f;
		RenderSettings.skybox.SetFloat("_Blend", skyboxBlend);
	}

	/// <summary>
	/// Gets the number of days that we have survived (starting at 1).
	/// </summary>
	/// <returns>The current day.</returns>
	public int GetDay()
	{
		return day;
	}

	/// <summary>
	/// Gets the time of day as a value between 0 and 1.
	/// </summary>
	/// <returns>The time of day.</returns>
	public float GetProgress()
	{
		return time / dayLengthSeconds;
	}

	/// <summary>
	/// Prompt for the new day.
	/// </summary>
	void OnGUI()
	{
		if (time < promptLength && promptFont)
		{
			float labelWidth = 400f;
			float labelHeight = 200f;
			GUILayout.BeginArea(new Rect(Screen.width / 2.0f - labelWidth / 2.0f, Screen.height / 2.0f - labelHeight / 2.0f, labelWidth, labelHeight));

			GUIStyle labelStyle = new GUIStyle();
			
			labelStyle.fontStyle = FontStyle.Bold;
			labelStyle.fontSize = 48;
			labelStyle.alignment = TextAnchor.UpperCenter;

			//Fade the prompt in for fadeIn seconds, pause for pause seconds, then fade out over fadeOut seconds
			float fadeAlpha;
			if (time < fadeIn)
				fadeAlpha = time / fadeIn;
			else if (time < fadeIn + pause)
				fadeAlpha = 1f;
			else
				fadeAlpha = (fadeOut - (time - (fadeIn + pause))) / fadeOut;

			labelStyle.font = promptFont;
			GUI.color = new Color(0f, 0f, 0f, fadeAlpha);

			GUILayout.Label("Day " + day, labelStyle);
			GUILayout.EndArea();
		}
	}
}
