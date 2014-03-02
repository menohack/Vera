using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	
	}
	
 
	void Update()
	{
		
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(0, Screen.height / 2, 200, 20));
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Connect to server"))
			Debug.Log(Networking.GetMap());
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}
