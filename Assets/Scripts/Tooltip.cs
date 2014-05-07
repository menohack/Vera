﻿using UnityEngine;
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
	
	void Update ()
	{
		GameObject currentPlayer = Spawn.GetCurrentPlayer();
		if (toolTip)
		{
			Build buildScript = currentPlayer.GetComponent<Build>();
			GameObject item = buildScript.GetItemHeld();
			if (item != null)
			{
				this.guiTexture.enabled = true;
				this.guiTexture.texture = SetTexture(item);
			}
			else
				this.guiTexture.enabled = false;
		}
	}

	/// <summary>
	/// Sets the tooltip texture depending on the current item.
	/// </summary>
	/// <param name="item">The item by which to select the texture.</param>
	/// <returns>The texture corresponding to the current item.</returns>
	Texture SetTexture(GameObject item)
	{
		switch (item.name) {
		case "Torch":
			return torch;
		case "Campfire":
			return campfire;
		case "WoodGate":
			return gate;
		case "Wall": case "Wall 2":
			return woodWall;
		case "StoneWall":
			return stoneWall;
		case "Sentry":
			return sentry;
		default:
			return torch;
		}
	}
}
