using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetFallenObjects : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		other.transform.position = new Vector3(0.0f, 10.0f, 0.0f);
	}
}
