using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour {

	Animator anim;
	private bool door_open;
	public Camera cam;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		door_open = false;
	}
	
	// Update is called once per frame
	void Update () {

		int x = Screen.width / 2;
		int y = Screen.height / 2;

		if(Input.GetKeyDown(KeyCode.E)){
			Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)){
				Openable p = hit.collider.GetComponent<Openable>();
				if(p != null){
					if(door_open){
						anim.Play("DoorClose");
						door_open = false;
					} else {
						anim.Play("DoorOpen");
						door_open = true;
					}
				}
			}
		}
	}
}
