using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour {

	public GameObject cam;
	bool carrying;
	GameObject carried_object;
	public float distance;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(carrying){
			carry(carried_object);
			checkDrop();
		} else {
			pickup();
		}
	}

	void carry(GameObject obj){
		obj.GetComponent<Rigidbody>().MovePosition(cam.transform.position + cam.transform.forward * distance);
		obj.transform.rotation = Quaternion.identity;
	}

	void pickup(){
		if(Input.GetMouseButtonDown(0)){
			Debug.Log("REEEEEEE");
			// Getting middle of screen
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			// Shooting ray
			Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)){
				Pickup p = hit.collider.GetComponent<Pickup>();
				if(p != null){
					Debug.Log("HIT");
					carrying = true;
					carried_object = p.gameObject;
					p.gameObject.GetComponent<Rigidbody>().useGravity = false;
				}
			}
		}
	}

	void checkDrop(){
		if(Input.GetMouseButtonDown(0)){
			carrying = false;
			carried_object.gameObject.GetComponent<Rigidbody>().useGravity = true;
			carried_object = null;
		}
	}

}
