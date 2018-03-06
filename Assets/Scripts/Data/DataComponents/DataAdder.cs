using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleComponents {
	public class DataAdder : DataComponent {


		public Bit leftAdd;
		public Bit rightAdd;

		public override DataSequence CalculateOutput() {

			//We can only take in one input
			int foundInput = -1;
			for (int i = 0; i < inputs.Length; i++) {
				if (inputs[i] != null) {
					foundInput = i;
				}
			}
			//If we found nothing, we return null
			if (foundInput == -1)
				return null;


			//Otherwise get a copy of our inputs data
			DataSequence dataInput = inputs[foundInput];

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
	}
}
