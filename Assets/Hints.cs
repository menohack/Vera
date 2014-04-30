using UnityEngine;
using System.Collections;

public class Hints : MonoBehaviour {

	/// <summary>
	/// Show hints.
	/// </summary>
	public bool hints = true;

	/// <summary>
	/// List of images for hints.
	/// </summary>
	public Texture[] hintList;

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
	public float delay = 10f;

	float timer;

	// Use this for initialization
	void Start () {
		timer = 0;
		this.guiTexture.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (hints){
			timer += Time.deltaTime;
			if (timer >= delay) 
			{
				this.guiTexture.enabled = true;
				fadeHint();
				timer = -fadeIn - fadeOut - pause;
			}
		}
	}

	void fadeHint(){
		float time = 0f;
		this.guiTexture.texture = chooseTexture ();
		Color textureColor = this.guiTexture.color;
		float fadeAlpha;
		do 
		{
			time += Time.deltaTime;
			if (time < fadeIn)
				fadeAlpha = time / fadeIn;
			else if (time < fadeIn + pause)
				fadeAlpha = 1f;
			else
				fadeAlpha = (fadeOut - (time - (fadeIn + pause))) / fadeOut;
			textureColor.a = fadeAlpha;
			this.guiTexture.color = textureColor;
		} while ( time < (pause + fadeOut + fadeIn) );
		this.guiTexture.enabled = false;
	}

	Texture chooseTexture() {
		int choice = Random.Range (0, hintList.Length);
		return hintList[choice];
	}
}
