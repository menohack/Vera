using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	public int startingWood = 20;

	public int startingOre = 20;

	int wood = 0;
	int ore = 0;

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
		GUIStyle font = new GUIStyle();
		font.fontSize = 24;
		GUILayout.BeginArea(new Rect(0, Screen.height - 200, 200, 200));
		GUILayout.BeginVertical();
		GUILayout.Label("Ore:\t\t" + ore, font);
		GUILayout.Label("Wood:\t" + wood, font);
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}
