using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PuzzleComponents {
	public class DataDeleter : DataComponent {

		public enum DeleteType { Center, Outside }
		private static readonly string[] DeleteTypeToText = new string[] { "Center", "Outside" };
		public DeleteType type;

		public override DataSequence CalculateOutput() {
			if (this.GetInput().Length <= 0) {
				//We do not have any output
				//Debug.Log(this.input[0].owner.gameObject.name + " Did not calculate any valid input " + (this.input[0] == null) + " " + (this.input[0].IsConnected() == false) + " " + (this.input[0].owner.GetOutput() == null));
				return null;
			}

			//Otherwise get a copy of our inputs data
			DataSequence dataInput = this.GetInput()[0].GetOutput();
			if (type == DeleteType.Center || type == DeleteType.Outside) {
				int length = dataInput.GetBitCount();
				//Set the index we are trying to delete dependent on the delete type
				int indexA = (type == DeleteType.Outside) ? 0							: (length / 2) - 1;
				int indexB = (type == DeleteType.Outside) ? dataInput.GetBitCount() - 1	: indexA + 1;
				DataSegment segmentA;
				DataSegment segmentB;
				int segmentAIndex;
				int segmentBIndex;
				bool linkedA;
				bool linkedB;
				dataInput.GetBitAtIndex(indexA, out linkedA, out segmentA, out segmentAIndex);
				dataInput.GetBitAtIndex(indexB, out linkedB, out segmentB, out segmentBIndex);


				//Debug.Log(length + "|" + indexA + "|" + indexB);

				if (linkedB && linkedA && segmentA == segmentB) {
					//We delete this whole segment!
					dataInput.segments.Remove(segmentAIndex);
				}
				else {
					//We remove B first to prevent array issues
					if (linkedB) {
						dataInput.segments.Remove(segmentBIndex);
					}
					else {
						dataInput.segments.Get(segmentBIndex).bits.Remove(indexB);
					}
					if (linkedA) {
						dataInput.segments.Remove(segmentAIndex);
					}
					else {
						dataInput.segments.Get(segmentBIndex).bits.Remove(indexA);
					}

				}
			}


			return dataInput;
			//throw new System.NotImplementedException();
		}

		public override string GetString() {
			return DeleteTypeToText[(int)type] + " Deleter";
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
