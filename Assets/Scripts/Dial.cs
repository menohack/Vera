using UnityEngine;
using System.Collections;

[ExecuteInEditMode()] 
public class Dial : MonoBehaviour {

	public Texture2D texture;
	public float xpos = 0;
	public float ypos = 0;
	private Rect rect;

	// Use this for initialization
	void Start () {
	
	}

	void UpdateSettings () {
		if (texture != null){
//			GUI.DrawTexture (new Rect (30, 30, 100, 100), texture);
			GUILayout.BeginArea(new Rect(xpos, ypos, 100, 100));
			GUILayout.Label (texture);
			GUILayout.EndArea();
		}
	}

	void OnGUI() {
		if (Application.isEditor) { UpdateSettings(); }
	}
}
