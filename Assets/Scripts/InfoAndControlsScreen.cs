using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class InfoAndControlsScreen : MonoBehaviour
{
	public Texture2D texture, quitTexture, backButtonTexture;
	public Texture2D cursorTexture;

	void Update()
	{
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture, ScaleMode.ScaleAndCrop);

		GUIStyle labelStyle = new GUIStyle();
		labelStyle.alignment = TextAnchor.UpperCenter;

		if (GUI.Button(new Rect(Screen.width / 2 - backButtonTexture.width / 2, Screen.height - quitTexture.height - backButtonTexture.height, backButtonTexture.width, backButtonTexture.height), backButtonTexture, labelStyle))
			Application.LoadLevel("Title");

		if (GUI.Button(new Rect(Screen.width / 2 - quitTexture.width / 2, Screen.height - quitTexture.height, quitTexture.width, quitTexture.height), quitTexture, labelStyle))
			Application.Quit();
	}
}
