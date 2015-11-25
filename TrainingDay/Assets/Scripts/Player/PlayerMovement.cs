using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {
    
	public float speed = 6f;
	public bool moveRelativeToPlayerRotation = false;
	
	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
	private CameraFollow playerCamera;
	int floorMask;
	float camRayLength = 100f;

	// called whether script is enabled or not
	void Awake() {

	}

	public override void OnStartLocalPlayer() {
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
		playerCamera = Camera.main.GetComponent<CameraFollow> ();
		playerCamera.setPlayerTransform (transform);
	}

	// fires every physics update
	[ClientCallback]
	void FixedUpdate() {
		if (isLocalPlayer) {
			float h = Input.GetAxisRaw ("Horizontal");
			float v = Input.GetAxisRaw ("Vertical");

			CmdMove (h, v);
			CmdTurn ();
			CmdAnimating (h, v);
		}
	}

	// moves the player object
	// var h: horizontal direction
	// var v: vertical direction

	void CmdMove(float h, float v) {
		// adjust x/z based on players orientation. 
		if (moveRelativeToPlayerRotation == true) {
			movement = adjustMovementForPlayerDirection (h, v);
		} else {
			movement = new Vector3(h, 0.0f, v);
		}
		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition (transform.position + movement);
	}

	// this always makes sure that move forward/left/right/bottom
	// keys are relative to the character, not the world.
	Vector3 adjustMovementForPlayerDirection(float h, float v) {
		float angle = playerRigidbody.rotation.eulerAngles.y;
		float rad = angle * Mathf.Deg2Rad;
		Vector3 adjustedMovement;

		// correctly move w/s forward backward
		//  v: Mathf.Cos(rad) * v
		// correctly move w/s base on rotation
		//  h: Mathf.Sin(rad) * v
		// correctly move a/d left/right
		// h: Mathf.Cos(rad) * h
		// correctly move a/d forward backward
		// v: Mathf.Sin(rad) * -(h)

		float adjustedVertical = ( (Mathf.Cos(rad) * v) + (Mathf.Sin(rad) * -(h))  );
		float adjustedHorizontal = ( (Mathf.Sin(rad) * v) + (Mathf.Cos(rad) * h) );
		adjustedMovement = new Vector3 (adjustedHorizontal, 0.0f, adjustedVertical);

		return adjustedMovement;
	}

	void CmdTurn() {
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit floorHit;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			playerRigidbody.MoveRotation(newRotation);
		}
	}


	void CmdAnimating(float h, float v) {
		bool walking = (h != 0f || v != 0f);
		anim.SetBool ("IsWalking", walking);
	}
}
