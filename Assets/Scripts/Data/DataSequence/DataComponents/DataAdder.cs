using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleComponents {
	public class DataAdder : DataComponent {


		public Bit leftAdd;
		public Bit rightAdd;

		public override DataSequence CalculateOutput() {
			if (this.GetInput().Length <= 0) {
				//We do not have any output
				//Debug.Log(this.input[0].owner.gameObject.name + " Did not calculate any valid input " + (this.input[0] == null) + " " + (this.input[0].IsConnected() == false) + " " + (this.input[0].owner.GetOutput() == null));
				return null;
			}

			//Otherwise get a copy of our inputs data
			DataSequence dataInput = this.GetInput()[0].GetOutput();

			DataSequence output = dataInput.CreateDeepCopy();
			output.segments.AddElementAtPosition(new DataSegment(new Bit[] { leftAdd.CreateCopy() }), 0);
			output.segments.AddElementAtEnd(new DataSegment(new Bit[] { rightAdd.CreateCopy()}));

			return output;
		}

		public override string GetString() {
			return "Adder";
		}

		public override void Setup() {
			//None needed
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
