using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PuzzleComponents {
	public class DoorSlideDown : DataTrigger {

		public Vector3 offset;

		public bool triggered = false;
		public bool playedSound = false;
		public float progress = 0.0f;

		Vector3 start;

		public override void Trigger() {
			triggered = true;
		}

		// Use this for initialization
		void Start() {
			start = this.transform.position;
		}

		// Update is called once per frame
		void Update() {
			if (triggered && progress <= 1.0f) {
				if (playedSound == false) {
					AkSoundEngine.PostEvent("Sliding_Door_Open", this.gameObject);
					playedSound = true;
				}

				progress += Time.deltaTime;
				this.transform.position = Vector3.Lerp(start, start + offset, progress);
			}
			else if (progress > 1.0f) {
				this.gameObject.SetActive(false);
			}
			
		}
	}
}
