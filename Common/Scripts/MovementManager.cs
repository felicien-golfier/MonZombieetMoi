using UnityEngine;
using System.Collections;

public class MovementManager : MonoBehaviour
{
//	public AudioClip shoutingClip;		// Audio clip of the player shouting.
	public float turnSmoothing = 15f;	// A smoothing value for turning the player.
	public float turnSpeed = 6f;
	public float maxSpeed = 5.5f;
	public float speedDampTime = 0.1f;	// The damping for the speed parameter
	
	
	private Animator anim;				// Reference to the animator component.
	private DoneHashIDs hash;			// Reference to the HashIDs.
	private GameObject fpCamera;
	private GameObject tpCamera;

	private static float FP_MAX_EULER = 45f;
	private static float FP_MIN_EULER = -45f;
	
	void Awake ()
	{
		// Setting up the references.
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneHashIDs>();

		fpCamera = GameObject.FindGameObjectWithTag ("FPCamera");
		tpCamera = GameObject.FindGameObjectWithTag ("TPCamera");
		
		// Set the weight of the shouting layer to 1.
		anim.SetLayerWeight(1, 1f);

	}

	public void Move (InputManager.ControlType type, Vector2 direction, Vector2 rotation)
	{
		switch (type) {
		case InputManager.ControlType.FIRST_PERSON :
			FPMoving (direction, rotation);
			FPCameraLimits();
			break;
		case InputManager.ControlType.THIRD_PERSON :
			TPMoving (direction);
			break;
		default :
			Debug.LogError("Movement Management function doesn't know the control type.");
			break;
		}
	}

	void FPMoving ( Vector2 direction, Vector2 rotation) {

		// Input speed (direction.x or direction.y is between 0 and 1)
		float inputSpeedX = (Mathf.Abs(direction.x) * maxSpeed);
		float inputSpeedY = (Mathf.Abs(direction.y) * maxSpeed);

		// Rotate Body
		Vector3 moveRotation = new Vector3 (0, rotation.x, 0);
		transform.Rotate(moveRotation * turnSpeed);

		// Rotate Camera
		fpCamera.transform.Rotate (new Vector3 (rotation.y, 0, 0) * turnSpeed);

		// Moving along X local axis 
		Vector3 moveDirection = new Vector3 (direction.x, 0, 0);
		moveDirection = this.transform.localToWorldMatrix * moveDirection;
		GetComponent<Rigidbody> ().MovePosition (GetComponent<Rigidbody> ().position + moveDirection * inputSpeedX * Time.deltaTime );

		// If there is not axis input...
		if (direction.y == 0) {
			// Otherwise set the speed parameter to 0.
			anim.SetFloat (hash.speedFloat, 0);
			return;
		}

		// Set direction of walk : forward or backward
		anim.SetInteger (hash.directionInt, Mathf.RoundToInt(direction.y / Mathf.Abs (direction.y)));

		// Run animation
		anim.SetFloat (hash.speedFloat, inputSpeedY, speedDampTime, Time.deltaTime);


	}
	
	// Clamp first person camera euler angle
	void FPCameraLimits () {
		Vector3 lea = fpCamera.transform.eulerAngles;

		if (lea.x < 180) {
			if (-lea.x < FP_MIN_EULER) {
				lea.x = -FP_MIN_EULER;
			}
		} else {
			if (360 - lea.x > FP_MAX_EULER)
				lea.x = 360 - FP_MAX_EULER;
		}

		lea.z = 0; // Force the z-around angle to zero

		fpCamera.transform.eulerAngles = lea;
	}

	void TPMoving (Vector2 direction)
	{
		// If there is not axis input...
		if (direction.x == 0 && direction.y == 0) {
			// Otherwise set the speed parameter to 0.
			anim.SetFloat (hash.speedFloat, 0);
			return;
		}

		// In third person moving we can't move backward
		anim.SetInteger (hash.directionInt, 1);
		anim.SetFloat (hash.speedFloat, maxSpeed, speedDampTime, Time.deltaTime);
		
		// Create a new vector of the horizontal and vertical inputs.
		Vector3 targetDirection = new Vector3(direction.x, 0f, direction.y);
		
		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		
		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
		Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
		
		// Change the players rotation to this new rotation.
		GetComponent<Rigidbody>().MoveRotation(newRotation);
	}

}
