using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public Texture2D loadingScreen;

	public Spawn spawn;

	bool loading = false;

	void Start()
	{
		/*
		GUITexture texture = gameObject.AddComponent<GUITexture>();
		texture.texture = loadingScreen;
		transform.position = new Vector3(0.5f, 0.5f, 0.0f);
		texture.enabled = true;
		*/
		//Application.LoadLevel(0);
		//async.allowSceneActivation = true;
	}

	void OnGUI()
	{

		//GUI.Box(new Rect(0, 0, loadingScreen.width, loadingScreen.height), loadingScreen, style);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), loadingScreen, ScaleMode.ScaleAndCrop);
		GUIStyle labelStyle = new GUIStyle();
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.fontSize = 32;
		labelStyle.alignment = TextAnchor.UpperCenter;

		if (loading)
			GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height * 0.8f, 200, 100), "LOADING", labelStyle);

		if (!loading && GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height * 0.8f, 100, 40), "Start Game"))
		{
			loading = true;
			Application.LoadLevel("Game");
		}
	}
}
