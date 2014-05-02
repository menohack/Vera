using UnityEngine;
using System.Collections;

public class EquipGUI : MonoBehaviour {

	/// <summary>
	/// The list of textures for each item.
	/// </summary>
	public Texture[] equipment;

	/// <summary>
	/// Is this the center item?
	/// </summary>
	public bool central;

	RightHandToolSelector t;

	// Use this for initialization
	void Start () {
		GameObject g = GameObject.FindGameObjectWithTag ("Player");
		t = g.GetComponent<RightHandToolSelector>();
	}
	
	// Update is called once per frame
	void Update () {
		changeTexture ();
	}

	/// <summary>
	/// Hacky!
	/// </summary>
	void changeTexture()
	{
		switch(t.getCurrentTool())
		{
		case RightHandToolSelector.Tool.Sword:
			this.guiTexture.texture = equipment[0];
			break;
		case RightHandToolSelector.Tool.Axe:
			this.guiTexture.texture = equipment[1];
			break;
		case RightHandToolSelector.Tool.Pickaxe:
			this.guiTexture.texture = equipment[2];
			break;
		default:
			break;
		}
	}
}
