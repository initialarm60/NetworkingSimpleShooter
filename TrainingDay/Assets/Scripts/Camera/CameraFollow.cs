using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float smoothing = 5f;

	Vector3 offset;

	void Start() {
		if (target != null) {
			offset = transform.position - target.position;
		}

	}

	public void setPlayerTransform(Transform playerTransform) {
		target = playerTransform;
		setOffset ();
	}

	private void setOffset() {
		offset = transform.position - target.position;
	}

	void FixedUpdate() {
		if (target != null) {
			Vector3 targetCamPos = target.position + offset;
			transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
		}

	}
}
