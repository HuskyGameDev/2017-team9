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
						stopEmiting();
					}
				} else {
					counter += (Time.deltaTime)/slideTime;
					target.transform.position = Vector3.Lerp(end, start, counter);
					if (target.transform.position == start) {
						sliding = false;
						counter = 0.0f;
						stopEmiting();
					}
				}
			}
		}

		// Use this for initialization
		void Start() {
			if (target == null) {
				target = this.gameObject;
			}
			start = target.transform.position + start;
			end = target.transform.position + end;
		}

		public override void Trigger() {
			if (sliding) {
				counter = 1 - counter;
			} else {
				startEmiting();
				sliding = true;
			}
			triggered = true;
		}

		public override void Untrigger() {
			if (sliding) {
				counter = 1 - counter;
			} else {
				startEmiting();
				sliding = true;
			}
			triggered = false;
		}

		public void startEmiting() {
			ParticleSystem[] parts = this.GetComponentsInChildren<ParticleSystem>();
			//Debug.Log("Activating " + parts.Length + " emmiters");
			for (int i = 0; i < parts.Length; i++) {
				//Debug.Log("Playing emitter " + i);
				parts[i].Play();
			}
		}

		public void stopEmiting() {
			ParticleSystem[] parts = this.GetComponentsInChildren<ParticleSystem>();
			//Debug.Log("Deactivating " + parts.Length + " emmiters");
			for (int i = 0; i < parts.Length; i++) {
				//Debug.Log("Stopping emitter " + i);
				parts[i].Stop();
			}
		}
	}
}
