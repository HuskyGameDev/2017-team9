using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PuzzleComponents {
	public class DataDeleter : DataComponent {


		public override DataSequence CalculateOutput() {
			if (this.input[0] == null || this.input[0].IsConnected() == false || this.input[0].partner.owner.GetOutput() == null) {
				//We do not have any output
				//Debug.Log(this.input[0].owner.gameObject.name + " Did not calculate any valid input " + (this.input[0] == null) + " " + (this.input[0].IsConnected() == false) + " " + (this.input[0].owner.GetOutput() == null));
				return null;
			}

			//Otherwise get a copy of our inputs data
			DataSequence input = this.input[0].partner.owner.GetOutput();

			int length = input.GetBitCount();
			int indexA = (length / 2) - 1;
			int indexB = indexA + 1;
			DataSegment segmentA;
			DataSegment segmentB;
			int segmentAIndex;
			int segmentBIndex;
			bool linkedA;
			bool linkedB;
			input.GetBitAtIndex(indexA, out linkedA, out segmentA, out segmentAIndex);
			input.GetBitAtIndex(indexB, out linkedB, out segmentB, out segmentBIndex);


			//Debug.Log(length + "|" + indexA + "|" + indexB);

			if (linkedB && linkedA && segmentA == segmentB) {
				//We delete this whole segment!
				input.segments.Remove(segmentAIndex);
			}
			else {
				//We remove B first to prevent array issues
				if (linkedB) {
					input.segments.Remove(segmentBIndex);
				}
				else {
					input.segments.Get(segmentBIndex).bits.Remove(indexB);
				}
				if (linkedA) {
					input.segments.Remove(segmentAIndex);
				}
				else {
					input.segments.Get(segmentBIndex).bits.Remove(indexA);
				}

			}



			return input;
			//throw new System.NotImplementedException();
		}

		public override string GetString() {
			return "Deleter";
			//throw new System.NotImplementedException();
		}

		public override void Setup() {
			//throw new System.NotImplementedException();
		}



		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {

		}
	}
}
