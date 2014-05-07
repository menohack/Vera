using UnityEngine;
using System.Collections;

public class RightHandToolSelector : MonoBehaviour {

	public bool DEBUG = false;

	public GameObject[] tools;
	private GameObject currentTool;
	private int currentIndex;
	//add to this/make sure corresponds to the array as tools grow
	//I'm aware this is terrible design
	public enum Tool {Sword, Axe, Pickaxe};

	// Use this for initialization
	void Start () {
		foreach (GameObject t in tools) 
		{
			disableTool(t);
		}

		setCurrentTool ((int)Tool.Sword);

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("PrevTool")) {
			switchToPrev ();
			if (DEBUG) printStatus (); 
		}
		if (Input.GetButtonDown ("NextTool")) {
			switchToNext ();
			if (DEBUG) printStatus (); 
		}
	
	}

	public Tool getCurrentTool () {
		return (Tool)currentIndex;
	}

	private void switchToPrev () {
		disableTool (currentTool);
		//loops around if first
		currentIndex = ((currentIndex != 0) ? currentIndex-1 : tools.Length-1);
		setCurrentTool (currentIndex);
	}

	private void switchToNext () {
		disableTool (currentTool);		
		//loops around if last
		currentIndex = ((currentIndex != tools.Length-1) ? currentIndex+1 : 0);
		setCurrentTool (currentIndex);
	}

	private void setCurrentTool (int index) {
		currentIndex = index;
		currentTool = tools[currentIndex];
		enableTool (currentTool);
	}

	private void disableTool (GameObject tool) {
		tool.renderer.enabled = false;
		tool.SetActive(false);
	}

	private void enableTool (GameObject tool) {
		tool.renderer.enabled = true;
		tool.SetActive(true);
	}

	private void printStatus () {
		Debug.Log ("Current tool is " + currentTool + " (" + (Tool)currentIndex + ")");
		foreach (GameObject t in tools) 
		{
			Debug.Log ("Tool " + t + " is active: " + t.activeSelf);
			Debug.Log ("Tool " + t + " is visible: " + t.renderer.enabled);
		}
	}
	

}
