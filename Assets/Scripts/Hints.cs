using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The Hints class handles the display of hints on the GUI at the start of the game.
/// </summary>
public class Hints : MonoBehaviour
{
	/// <summary>
	/// Whether to show hints.
	/// </summary>
	public bool hints = true;

	/// <summary>
	/// List of images for hints.
	/// </summary>
	public List<Texture> hintList;

	/// <summary>
	/// Time to fade in a hint.
	/// </summary>
	public float fadeIn = 1f;

	/// <summary>
	/// The time over which the font pauses fully visible after the fade in.
	/// </summary>
	public float pause = 3f;

	/// <summary>
	/// Time to fade out a hint.
	/// </summary>
	public float fadeOut = 1f;
	
	/// <summary>
	/// Number of seconds between hints showing.
	/// </summary>
	public float delay = 15f;

	float timer;

	void Start ()
	{
		timer = 0;
		this.guiTexture.enabled = false;
		this.guiTexture.texture = ChooseTexture();
	}
	
	void Update ()
	{
		if (hintList.Count < 1)
		{
			Destroy(this);
			return;
		}

		if (hints && hintList.Count > 0)
		{
			timer += Time.deltaTime;
			if (timer >= delay) 
			{
				FadeHint(timer - delay);
				this.guiTexture.enabled = true;
				if (timer > delay + fadeIn + fadeOut + pause)
				{
					this.guiTexture.texture = ChooseTexture();
					timer = 0;
					this.guiTexture.enabled = false;
				}
			}
		}
	}

	/// <summary>
	/// Fades a hint in and out by setting its transparency.
	/// </summary>
	/// <param name="time">The time since this hint started.</param>
	void FadeHint(float time)
	{
		Color textureColor = this.guiTexture.color;
		float fadeAlpha;
		if (time < fadeIn)
			fadeAlpha = time / fadeIn;
		else if (time < fadeIn + pause)
			fadeAlpha = 1f;
		else
			fadeAlpha = (fadeOut - (time - (fadeIn + pause))) / fadeOut;
		textureColor.a = fadeAlpha;
		this.guiTexture.color = textureColor;
	}

	/// <summary>
	/// Randomly choose a hint to display, then removes it from the list.
	/// </summary>
	/// <returns></returns>
	Texture ChooseTexture()
	{
		int choice = Random.Range (0, hintList.Count);
		Texture result = hintList[choice];
		hintList.Remove(result);
		return result;
	}
}
