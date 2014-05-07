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
	/// The size of the day prompt font.
	/// </summary>
	public int fontSize = 48;

	/// <summary>
	/// The color of the day prompt font.
	/// </summary>
	public Color fontColor = Color.black;

	/// <summary>
	/// The spawn GameObject.
	/// </summary>
	public Spawn spawn;

	/// <summary>
	/// The event that fires at dawn.
	/// </summary>
	public event EventListener dawnListener;

	/// <summary>
	/// The event that fires at dusk.
	/// </summary>
	public event EventListener duskListener;


	/// <summary>
	/// Lighting fires at night.
	/// </summary>
	public event EventListener fireListener;

	/// <summary>
	/// The type function that dusk and dawn events invoke.
	/// </summary>
	public delegate void EventListener();

	/// <summary>
	/// Style used for the day prompt label.
	/// </summary>
	GUIStyle labelStyle;

	/// <summary>
	/// Whether dawn events have been invoked for today.
	/// </summary>
	bool invokedDawnEvents = false;

	/// <summary>
	/// Whether dusk events have been invoked for today.
	/// </summary>
	bool invokedDuskEvents = false;

	void Start()
	{
		labelStyle = new GUIStyle();

		labelStyle.font = promptFont;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.fontSize = fontSize;
		labelStyle.alignment = TextAnchor.UpperCenter;

		dawnListener += new EventListener(spawn.DespawnWolves);
		duskListener += new EventListener(spawn.SpawnWolves);
	}

	void Update ()
	{
		float newTime = time + Time.deltaTime;
		if (newTime >= dayLengthSeconds)
		{
			time = newTime - dayLengthSeconds;
			day++;
		}
		else
			time = newTime;

		SetSkyboxInterpolation();

		//Trigger events at dusk and dawn
		if (GetProgress() < 0.5f && !invokedDawnEvents)
		{
			invokedDawnEvents = true;
			invokedDuskEvents = false;
			if (dawnListener != null)
				dawnListener.Invoke();
		}
		else if (GetProgress() >= 0.5f && !invokedDuskEvents)
		{
			invokedDuskEvents = true;
			invokedDawnEvents = false;
			if (duskListener != null)
				duskListener.Invoke();
		}

		if (GetProgress() >= 0.5f)
		{
			if (fireListener != null)
				fireListener.Invoke ();
		}
	}

	void SetSkyboxInterpolation()
	{
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
	/// Serializes and deserializes the Sundial script across the network.
	/// </summary>
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			float timeW = time;
			int dayW = day;
			stream.Serialize(ref timeW);
			stream.Serialize(ref dayW);
		}
		else
		{
			float timeR = time;
			int dayR = day;
			stream.Serialize(ref timeR);
			stream.Serialize(ref dayR);
			time = timeR;
			day = dayR;
		}
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
		if (time < fadeIn + pause + fadeOut && promptFont)
		{
			//float labelWidth = 400f;
			//float labelHeight = 200f;
			string dayString = "Day " + day;
			Vector2 size = labelStyle.CalcSize(new GUIContent(dayString));
			float labelWidth = size.x;
			float labelHeight = size.y;
			GUILayout.BeginArea(new Rect(Screen.width / 2.0f - labelWidth / 2.0f, Screen.height * 0.4f - labelHeight / 2.0f, labelWidth, labelHeight));

			//Fade the prompt in for fadeIn seconds, pause for pause seconds, then fade out over fadeOut seconds
			float fadeAlpha;
			if (time < fadeIn)
				fadeAlpha = time / fadeIn;
			else if (time < fadeIn + pause)
				fadeAlpha = 1f;
			else
				fadeAlpha = (fadeOut - (time - (fadeIn + pause))) / fadeOut;

			GUI.color = new Color(fontColor.r, fontColor.g, fontColor.b, fadeAlpha);

			GUILayout.Label(dayString, labelStyle);
			GUILayout.EndArea();
		}
	}
}
