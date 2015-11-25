using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSyncPosition : NetworkBehaviour {

	[SyncVar] 
	private Vector3 syncPos;
	[SyncVar]
	private Quaternion syncRotate;

	[SerializeField] Transform myTransform;
	[SerializeField] float lerpRate = 15;


	// Update is called once per frame
	void FixedUpdate () {
		transmitPosition ();
		lerpPosition ();
		syncRotation ();
	}

	void lerpPosition() {
		if (!isLocalPlayer) {
			myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime*lerpRate);
		}
	}

	void syncRotation() {
		if (!isLocalPlayer) {
			myTransform.rotation = syncRotate;
		}
	}

	[Command]
	void CmdProvidePositionToServer(Vector3 pos) {
		syncPos = pos;
	}

	[Command]
	void CmdProvideRotationToServer(Quaternion rotation) {
		syncRotate = rotation;
	}

	[ClientCallback] 
	void transmitPosition() {
		if (isLocalPlayer) {
			CmdProvidePositionToServer (myTransform.position);
			CmdProvideRotationToServer(myTransform.rotation);
		}
	}
}
