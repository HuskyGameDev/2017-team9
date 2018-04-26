using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointFollower : MonoBehaviour {


	public bool disabled = true;
	public bool nextPoint;
	public bool nextFocus;

	public bool useFocus = true;


	public GameObject cam;


	public GameObject[] points;
	public GameObject[] foci;

	public GameObject currentPoint;
	public GameObject currentFocus;

	public int iF;
	public int iP;

	public float speed = 0.2f;
	public Vector3 startPos;
	public float counter = 0.00001f;
	public bool absoluteMode = true;

	// Use this for initialization
	void Start () {
		cam = this.gameObject;
		if (useFocus)
			currentFocus = foci[0];
		currentPoint = points[0];
	}
	
	// Update is called once per frame
	void Update () {
		if (disabled) return;
		if (nextFocus) {
			currentFocus = foci[++iF];
			nextFocus = false;
		}
		if (nextPoint) {
			MoveToNextPoint();
			nextPoint = false;
		}

		if (absoluteMode) {
			Vector3 heading = (currentPoint.transform.position - cam.transform.position);
			float distance = heading.magnitude;
			Vector3 normal = heading.normalized;
			cam.transform.position = cam.transform.position + (normal * ((distance < speed * Time.deltaTime) ? distance : (speed * Time.deltaTime)));
		}
		else {
			if (currentPoint != null)
				if (iP - 1 <= 0 || points[iP - 1] == null)
					cam.transform.position = Vector3.Lerp(cam.transform.position, currentPoint.transform.position, Time.deltaTime * speed);
				else {
					counter += Time.deltaTime;
					cam.transform.position = Vector3.Lerp(points[iP - 1].transform.position, currentPoint.transform.position, counter * speed);
				}
		}

		if (currentFocus != null && useFocus)
			cam.transform.LookAt(currentFocus.transform);
	}

	public void MoveToNextPoint() {
		counter = 0.00001f;
		startPos = cam.transform.position;
		currentPoint = points[++iP];
	}
}
