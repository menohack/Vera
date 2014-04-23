using UnityEngine;
using System.Collections;

[ExecuteInEditMode()] 
public class Dial : MonoBehaviour {

	public Texture2D texture;
	public float xpos = 0;
	public float ypos = 0;
	public float alpha = 1;
	private Color prevColor;
	private Rect rect;

	// Use this for initialization
	void Start () {
		UpdateSettings ();
	}

	void UpdateSettings () {
		rect = new Rect (xpos, ypos, 100, 100);
//			GUILayout.BeginArea(new Rect(xpos, ypos, 100, 100), texture);
//			GUILayout.Label (texture);
//			GUILayout.EndArea();
	}

	void OnGUI() {
		if (Application.isEditor) { UpdateSettings(); }
		Matrix4x4 matrixBackup = GUI.matrix;
		prevColor = GUI.color;
		GUI.color = new Color(prevColor.r, prevColor.g, prevColor.b, alpha);
		GUI.DrawTexture (rect, texture);
		GUI.color = prevColor;
		GUI.matrix = matrixBackup;
	}
}
