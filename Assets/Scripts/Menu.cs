using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	static bool gameOver = false;

	public Texture2D menuTexture;

	GUIStyle labelStyle, verticalStyle;

	void Start()
	{
		labelStyle = new GUIStyle();
		labelStyle.alignment = TextAnchor.UpperCenter;
		labelStyle.fontSize = 32;
		labelStyle.fontStyle = FontStyle.Bold;

		verticalStyle = new GUIStyle();
		verticalStyle.padding.left = verticalStyle.padding.right = 10;
	}

	public static void EndGame()
	{
		if (!gameOver)
		{
			Time.timeScale = 0f;
			gameOver = true;

			//set mouse to visible and free
			Screen.lockCursor = false;
			Screen.showCursor = true;

			GameObject musicWorld = GameObject.Find("MusicWorld");
			if (musicWorld && musicWorld.audio)
				musicWorld.audio.Stop();
			GameObject musicMenu = GameObject.Find("MusicMenu");
			if (musicMenu && musicMenu.audio)
			{
				musicMenu.audio.time = 16.6f;
				musicMenu.audio.Play();
			}
			MouseLook[] mouseLook = GameObject.FindObjectsOfType<MouseLook>();
			foreach (MouseLook m in mouseLook)
				Destroy(m);
		}
	}

	public static void StartGame()
	{
		Time.timeScale = 1f;
		gameOver = false;

		//set mouse to visible and free
		Screen.lockCursor = true;
		Screen.showCursor = false;

		Application.LoadLevel(0);
	}

	void OnGUI()
	{
		if (gameOver)
		{
			GUILayout.BeginArea(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 150, 300, 300));
			GUILayout.BeginArea(new Rect(0,0, menuTexture.width, menuTexture.height), menuTexture);
			GUILayout.EndArea();
			GUILayout.BeginVertical(verticalStyle);
			GUILayout.FlexibleSpace();
			GUILayout.Label("Game Over", labelStyle);
			if (GUILayout.Button("New Game"))
				StartGame();
			if (GUILayout.Button("Quit"))
				Application.Quit();
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
}
