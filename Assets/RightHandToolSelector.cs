using UnityEngine;
using System.Collections;

public class RightHandToolSelector : MonoBehaviour {

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
			t.renderer.enabled = false;
		}

		setCurrentTool ((int)Tool.Sword);

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("PrevTool")) {
			switchToPrev ();
		}
		if (Input.GetButtonDown ("NextTool")) {
			switchToNext ();
		}
	
	}

	private void switchToPrev () {
		currentTool.renderer.enabled = false;
		//loops around if first
		currentIndex = ((currentIndex != 0) ? currentIndex-1 : tools.Length-1);
		setCurrentTool (currentIndex);
	}

	private void switchToNext () {
		currentTool.renderer.enabled = false;
		//loops around if last
		currentIndex = ((currentIndex != tools.Length-1) ? currentIndex+1 : 0);
		setCurrentTool (currentIndex);
	}

	private void setCurrentTool (int index) {
		currentIndex = index;
		currentTool = tools[currentIndex];
		currentTool.renderer.enabled = true;
	}

	public Tool getCurrentTool () {
		return (Tool)currentIndex;
	}
}
