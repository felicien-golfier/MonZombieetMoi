using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	// ENUM
	public enum ControlType {
		FIRST_PERSON,
		THIRD_PERSON
	}
	public enum BodyControl {
		XBOX,
		KEYS,
		XML,
		INFRARED
	}
	public enum HeadControl {
		XBOX,
		MOUSE,
		OCULUS
	}

	// PUBLIC MEMBERS
	public ControlType control_type = ControlType.THIRD_PERSON;
	public BodyControl body_controls = BodyControl.XBOX;
	public HeadControl head_controls = HeadControl.XBOX;
	
	// PRIVATE MEMBERS
	private MovementManager mover;
	private bool sneak = false;
	private Vector2 direction; // no jump
	private Vector2 rotation; 

	// STATIC
	private static float XBOX_SENSITIVITY = 0.6f;
	
	void Start () {
		mover = GetComponent<MovementManager>();
		initMainCamera ();
	}

	void initMainCamera () {

		Camera FPCamera = GameObject.FindWithTag ("FPCamera").GetComponent<Camera> ();
		Camera TPCamera = GameObject.FindWithTag ("TPCamera").GetComponent<Camera> ();

		switch (control_type) {
		case ControlType.FIRST_PERSON:
			FPCamera.enabled = true;
			TPCamera.enabled = false;
			break;
		case ControlType.THIRD_PERSON:
			FPCamera.enabled = false;
			TPCamera.enabled = true;
			break;
		default:
			Debug.LogError("You have to select a control type in the input manager");
			break;
		}

	}

	void FixedUpdate () {

		// Update directions of the body
		switch (body_controls) {
			case BodyControl.XBOX :
				UpdateXboxDirection ();
			break;
			case BodyControl.KEYS : 
				UpdateKeysDirection ();
			break;
			case BodyControl.XML :
				UpdateXmlDirection ();
			break;
			case BodyControl.INFRARED :
				UpdateInfraredDirection ();
			break;
			default :
				Debug.LogError("You have to select a body control in the input manager");
			break;
		}

		// Update rotations in case of fps controls
		if (control_type == ControlType.FIRST_PERSON) {
			switch (head_controls) {
				case HeadControl.XBOX:
					UpdateXboxRotation ();
				break;
				case HeadControl.MOUSE: 
					UpdateMouseRotation ();
				break;
				case HeadControl.OCULUS:
					UpdateOculusRotation ();
				break;
				default :
					Debug.LogError ("You have to select a head control in the input manager");
				break;
			}
		}

		mover.Move(control_type, direction, rotation);
	}
	
	void UpdateXboxDirection() {

		// Record Inputs
		float h =  Input.GetAxis("XboxHorizontal1");
		float v = -Input.GetAxis("XboxVertical1");

		if (Mathf.Abs (h) < XBOX_SENSITIVITY) {
			h = 0;
		}
		if (Mathf.Abs (v) < XBOX_SENSITIVITY) {
			v = 0;
		}

		// Compute for the mover
		direction = new Vector2 (h, v);
	}

	void UpdateKeysDirection() {

		// Record Inputs
		float h = Input.GetAxis("KeyHorizontal");
		float v = Input.GetAxis("KeyVertical");

		// Compute for the mover
		direction = new Vector2 (h, v);
	}

	void UpdateXmlDirection() {
		Debug.LogError("Xml Path in construction");
		// TO DO : the character follow a predefined path
	}

	void UpdateInfraredDirection() {
		Debug.LogError("Infrared in construction");
		// TO DO : the character follow our infrared movements
	}

	void UpdateXboxRotation() {
		
		// Record Inputs
		float x =  Input.GetAxis("XboxHorizontal2");
		float y = -Input.GetAxis("XboxVertical2");
		
		if (Mathf.Abs (x) < XBOX_SENSITIVITY) {
			x = 0;
		}
		if (Mathf.Abs (y) < XBOX_SENSITIVITY) {
			y = 0;
		}
		
		// Compute for the mover
		rotation = new Vector2 (x, y);
	}
	
	void UpdateMouseRotation() {
		
		// Record Inputs
		float x = Input.GetAxis("Mouse X");
		float y = -Input.GetAxis("Mouse Y");
		//		sneak = Input.GetButton("Sneak");
		
		// Compute for the mover
		rotation = new Vector2 (x, y);
	}
	
	void UpdateOculusRotation() {
		Debug.LogError("Oculus rotation in construction");
		// TO DO : oculus head rotations
	}



}
