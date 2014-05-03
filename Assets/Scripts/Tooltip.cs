using UnityEngine;
using System.Collections;

public class Tooltip : MonoBehaviour {

	/// <summary>
	/// Turns on ToolTips.
	/// </summary>
	public bool toolTip = true;

	public Texture torch;
	public Texture campfire;
	public Texture gate;
	public Texture woodWall;
	public Texture stoneWall;
	public Texture sentry;

	Texture[] t = new Texture[6];

	GameObject p;

	// Use this for initialization
	void Start () {
		t [0] = torch;
		t [1] = campfire;
		t [2] = gate;
		t [3] = woodWall;
		t [4] = stoneWall;
		t [5] = sentry;
	}
	
	// Update is called once per frame
	void Update () {
		if (toolTip) {
			Build b = Spawn.GetMyPlayer().GetComponent<Build>();
			GameObject item = b.GetItemHeld();
			if (item != null){
				//draw something
				this.guiTexture.enabled = true;
				this.guiTexture.texture = SetTexture(item);
			}
			else
			{
				//turn off component
				this.guiTexture.enabled = false;
			}
		}
	}

	Texture SetTexture(GameObject item) {
		switch (item.name) {
		case "Torch":
			return t[0];
		case "Campfire":
			return t[1];
		case "WoodGate":
			return t[2];
		case "Wall": case "Wall 2":
			return t[3];
		case "StoneWall":
			return t[4];
		case "Sentry":
			return t[5];
		default:
			return t[0];
		}
	}
}
