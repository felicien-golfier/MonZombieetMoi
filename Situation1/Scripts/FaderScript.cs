using UnityEngine;
using System.Collections;

public class FaderScript : MonoBehaviour {

	public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.8f;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;

	void OnGUI(){

		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		// force the number between 0 and 1
		alpha = Mathf.Clamp01 (alpha);

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);

	}

	public void BeginFade (int direction){
		fadeDir = direction;
	}

	public void BeginFade (int direction, float speed)
	{
		fadeDir = direction;
		fadeSpeed = speed;
	}

	void OnLevelWasLoaded (){
		BeginFade (-1);
	}

}
