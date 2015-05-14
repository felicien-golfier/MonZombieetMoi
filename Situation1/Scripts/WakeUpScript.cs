using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class WakeUpScript : MonoBehaviour {
	private Animator anim;
	private FaderScript fader;
	private DoneHashIDs hash;
	private bool cheat = false;
	private bool suiteCheat = false;
	private Vector3 tmp;
	
	void Awake(){

		anim = GetComponent<Animator> ();
		fader = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<FaderScript> ();
		hash = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneHashIDs>();

	}

	void Start(){
		GameObject.Find("bed").GetComponent<BoxCollider>().enabled = false;
		StartCoroutine (StartLounging ());

	}

	void Update(){
		if (cheat) {
			tmp = new Vector3 (1.3f, this.transform.position.y, -2.0f) - this.transform.position;
			this.transform.position += tmp * Time.deltaTime * 1f;
		} else if (suiteCheat) {
			this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation; 
//			tmp = new Vector3 (this.transform.position.x,0f, this.transform.position.z) - this.transform.position;
//			this.transform.position += tmp * Time.deltaTime * 5f;
//			suiteCheat = tmp.magnitude < 0.05f ? false : suiteCheat;
		}
	}
	// Use this for initialization
	public void getUp (){
		StartCoroutine(OpenEyes ());
	}
	private IEnumerator StartLounging(){

		anim.SetBool (hash.loungeBool, true);
		yield return new WaitForSeconds (0.1f);
		anim.SetBool (hash.loungeBool, false);
		yield return new WaitForSeconds (0.1f);
		this.transform.position = new Vector3 (1.2f,0.55f,-3.9f);
		this.transform.eulerAngles = new Vector3 (5f, -10f, 0f);

	}
	
	private IEnumerator OpenEyes (){

		yield return new WaitForSeconds (2f);
		GameObject.Find ("Main Camera").GetComponent<Blur>().enabled = true;
		fader.BeginFade (-1, 0.2f);// Première ouverture. Flou
		yield return new WaitForSeconds (0.8f);
		fader.BeginFade (1, 0.2f);// referme lentement
		yield return new WaitForSeconds (1f);
		fader.BeginFade (-1, 2f);
		GameObject.Find ("Main Camera").GetComponent<Blur>().enabled = false;
		yield return new WaitForSeconds (0.1f);
		fader.BeginFade (1, 2f);// Clignement
		yield return new WaitForSeconds (0.1f);
		fader.BeginFade (-1, 1.5f);
		wakeUp ();
			
	}

	private void wakeUp (){
		anim.SetBool (hash.deadBool, true);
		StartCoroutine(cheatCode ());
	}

	private IEnumerator cheatCode (){

		yield return new WaitForSeconds (1.2f);
		cheat= true;
		yield return new WaitForSeconds (0.2f);
		suiteCheat = true;
		yield return new WaitForSeconds (0.2f);
		cheat= false;

		GameObject.Find("bed").GetComponent<BoxCollider>().enabled = true;
	}
}
