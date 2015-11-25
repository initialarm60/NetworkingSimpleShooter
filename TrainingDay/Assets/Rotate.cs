using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public Transform playerTransfrom;
	public float rotationSpeed = 2;
	// Update is called once per frame
	void Update () {
		playerTransfrom.Rotate (Vector3.up* rotationSpeed * Time.deltaTime);
	}
}
