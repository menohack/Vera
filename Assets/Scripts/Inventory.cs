using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	public int startingWood = 20;

	public int startingOre = 20;

	int wood = 0;
	int ore = 0;

	public Texture2D oreTexture;
	public Texture2D woodTexture;
	public Texture2D hudTexture;

	public Texture2D buttonTexture1, buttonTexture2, buttonTexture3, buttonTexture4;

	// Use this for initialization
	void Start () {
		wood = startingWood;
		ore = startingOre;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int GetWood()
	{
		return wood;
	}

	public int GetOre()
	{
		return ore;
	}

	public void AddWood(int units)
	{
		if (units < 0)
			throw new UnityException("Number of units must be positive");
		wood += units;
	}

	public void AddOre(int units)
	{
		if (units < 0)
			throw new UnityException("Number of units must be positive");
		ore += units;
	}

	public void RemoveWood(int units)
	{
		if (units < 0)
			throw new UnityException("Number of units must be positive");
		wood -= units;
	}

	public void RemoveOre(int units)
	{
		if (units < 0)
			throw new UnityException("Number of units must be positive");
		ore -= units;
	}


	void OnGUI()
	{
		//Ore and wood count
		GUIStyle font = new GUIStyle();
		font.fontSize = 24;
		font.alignment = TextAnchor.MiddleCenter;
		GUILayout.BeginArea(new Rect(0, Screen.height - 200, 200, 200));
		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal();
		GUILayout.Label(oreTexture);
		GUILayout.Label("" + ore, font);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label(woodTexture);
		GUILayout.Label("" + wood, font);
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
		GUILayout.EndArea();

		//HUD on bottom of screen with buttons
		GUIStyle buttonStyle = new GUIStyle();
		GUILayout.BeginArea(new Rect(Screen.width/2 - hudTexture.width/2, Screen.height - hudTexture.height, hudTexture.width, hudTexture.height), hudTexture);
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Button(buttonTexture1, buttonStyle);
		GUILayout.FlexibleSpace();
		GUILayout.Button(buttonTexture2, buttonStyle);
		GUILayout.FlexibleSpace();
		GUILayout.Button(buttonTexture3, buttonStyle);
		GUILayout.FlexibleSpace();
		GUILayout.Button(buttonTexture4, buttonStyle);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();

	}
}
