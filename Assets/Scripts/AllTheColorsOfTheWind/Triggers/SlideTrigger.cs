using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllTheColorsOfTheWind {
	public class SlideTrigger : ColorTrigger {

		public Vector3 start = new Vector3(0, 0, 0);
		public Vector3 end = new Vector3(0, 0, -1);
		public float slideTime = 1.0f;

		private GameObject target;
		private bool sliding = false;
		private float counter = 0.0f;

		void Update() {
			if (sliding) {
				if (triggered) {
					counter += (Time.deltaTime)/slideTime;
					target.transform.position = Vector3.Lerp(start, end, counter);
					if (target.transform.position == end) {
						sliding = false;
						counter = 0.0f;
					}
				} else {
					counter += (Time.deltaTime)/slideTime;
					target.transform.position = Vector3.Lerp(end, start, counter);
					if (target.transform.position == start) {
						sliding = false;
						counter = 0.0f;
					}
				}
			}
		}

		// Use this for initialization
		void Start() {
			if (target == null) {
				target = this.gameObject;
			}
			start.x = target.transform.position.x + start.x;
			start.y = target.transform.position.y + start.y;
			start.z = target.transform.position.z + start.z;
			end.x = target.transform.position.x + end.x;
			end.y = target.transform.position.y + end.y;
			end.z = target.transform.position.z + end.z;
		}

		public override void Trigger() {
			sliding = true;
			triggered = true;
		}

		public override void Untrigger() {
			sliding = true;
			triggered = false;
		}
	}
}
