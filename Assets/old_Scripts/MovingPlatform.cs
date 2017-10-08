using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	private enum State {
		A,
		Wait,
		B,
		Wait2
	}
	private State state = State.Wait2;
	private float progress = 0.0f;
	private Vector3 start;
	public Vector3 endDisplacemenmt;

	// Use this for initialization
	void Start () {
		start = this.transform.position;
	}

	private void Progress() {
		progress += Time.fixedDeltaTime * 0.5f;
		if (progress > 1.0f) {
			if (state == State.A)
				state = State.Wait;
			else if (state == State.B)
				state = State.Wait2;
			else if (state == State.Wait)
				state = State.B;
			else if (state == State.Wait2)
				state = State.A;
			progress = 0.0f;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		if (state == State.A) {
			this.transform.position = Vector3.Lerp(start, start + endDisplacemenmt, progress);
		} else if (state == State.B) {
			this.transform.position = Vector3.Lerp(start + endDisplacemenmt, start, progress);
		} 
		Progress();
	}
}
