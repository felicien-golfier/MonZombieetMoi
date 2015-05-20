using UnityEngine;
using System.Collections;

public class CameraMouvementsScript : MonoBehaviour {
	
	public Vector3[] pos =new Vector3[] {new Vector3 (11f, 16f, 15f), new Vector3 (1.5f, 1.5f, 8f),new Vector3 (-2f, 2f, 1f),new Vector3 (1.43f, 0.9f, -4.6f)};
	private int posLength;
	private Vector3[] posInterpolate;
	private int  posInterpolateLength;
//	private Vector3 resetPosition;
	public Vector3 resetLook = new Vector3 (1.2f,0.9f, -3.7f);
	private int k = 0;
	public int n = 5; // nombre de points dont la caméra saute pour lookat
	public int smooth = 5;
	public float speed = 10f;

	private Vector3 lastVector;
	private Vector3 actualVector;
	private Vector3 translateVector;
	private bool stop = false;
	private Vector3 lastLook;
	private Vector3 nextLook;
	private bool CameraMvtEnd = false;

	private float tmp;

	void Start () {



		posLength = pos.Length;
//		resetPosition = pos [posLength - 1];
		interpolation (pos);
		posInterpolateLength = posInterpolate.Length;
		this.transform.position = posInterpolate [0];
		this.transform.LookAt(posInterpolate[n]);
		nextLook = posInterpolate [k + n];

	}
	
	// Update is called once per frame
	void Update (){
		if (!CameraMvtEnd){

			cameraMouvements ();
	}
}


	void cameraMouvements (){
		if (!stop) {
			translateVector = (posInterpolate [k] - this.transform.position);
			this.transform.position += (translateVector.normalized * Time.deltaTime * speed);

			nextLook += (posInterpolate [k + n] - nextLook).normalized * Time.deltaTime * speed;
			this.transform.LookAt (nextLook);
			Debug.DrawLine (this.transform.position, posInterpolate [k], Color.red, 10f);

			if (translateVector.magnitude < 0.2f) {
				k++;
				stop = k > (posInterpolateLength - n - 1);
			}
		} else if ((this.transform.position - pos [posLength - 1]).magnitude > 0.1f) {
			GameObject.Find ("gameController").GetComponent<FaderScript> ().BeginFade(1,2);

			translateVector = (pos [posLength - 1] - this.transform.position);
			this.transform.position += (translateVector.normalized * Time.deltaTime * speed);
			tmp = (this.transform.position - pos [posLength - 1]).magnitude;
			tmp = tmp > 1 ? 1 : tmp;
			tmp = tmp < 0 ? 0 : tmp;
			this.transform.LookAt (resetLook * (1 - tmp) + nextLook * (tmp));
			
		} else if (!CameraMvtEnd) {
			GameObject.Find("char_ethan").GetComponent<WakeUpScript>().getUp();
			CameraMvtEnd = true;
		}


	}

	// interpolate the différents points to have a perfect traveling.
	void interpolation (Vector3[] pos){
		int tmp = 0;
		int[] magnitudes = new int[posLength];

		// Initialisation du nombre de points à creer.
		for (int i=0; i < posLength - 1; i++) {
			tmp += Mathf.CeilToInt((pos[i+1]- pos[i]).magnitude);
			magnitudes[i]=tmp;
		}

		// Création du tableau de veteurs interpolés
		posInterpolate = new Vector3[tmp + 1];
		posInterpolate[0] = pos [0];

		int j = 0;
		actualVector = (pos[1] - pos[0]).normalized;
		// Instanciation des points.
		for (int i = 0; i < tmp; i++) {
			// Situation de la phase traversée par la caméra (entre quels points)
			actualVector = (pos[j+1] - posInterpolate[i]).normalized;
			if (i >= magnitudes[j]-1)
			{
				j ++;
			}
			// Création du vecteur qui va positionner le point suivant.
			Debug.DrawRay(posInterpolate[i], lastVector.normalized, Color.red, 10f);
			lastVector = (lastVector * (smooth*(magnitudes[j] - i)/(magnitudes[j] == 0 ? 0.01f:magnitudes[j])) + actualVector).normalized;

//			Debug.DrawRay(posInterpolate[i], lastVector.normalized, Color.red, 10f);
			Debug.DrawRay(posInterpolate[i], actualVector.normalized, Color.green, 10f);

			// On ajoute au point précédent le vecteur calculé ci dessus.
			posInterpolate[i+1] = posInterpolate[i] + lastVector;
			Debug.DrawLine(posInterpolate[i], posInterpolate[i+1], Color.blue, 10f);
		}

		posInterpolate [posInterpolate.Length - 1] = pos [posLength - 1];

	}
	
}
