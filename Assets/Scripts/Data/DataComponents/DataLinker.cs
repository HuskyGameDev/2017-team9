using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleComponents {
	/// <summary>
	/// This class modifes a DataSegement to be linked instead of free.
	/// </summary>
	public class DataLinker : DataComponent {

		/// <summary>
		/// Copy our one input data and transform it into a linked segment.
		/// </summary>
		/// <returns></returns>
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
			DataSequence unlinkedOutput = inputs[foundInput];

			//Create the unified data segment
			DataSegment singleSegment = new DataSegment(unlinkedOutput.GetOrderedBitSequence());

			//Link it all together
			singleSegment.linked = true;

			//Create our final output
			DataSequence newOutput = new DataSequence(new DataSegment[] { singleSegment });

			return newOutput;
		}

		public override string GetString() {
			return "Linker";
		}


		/// <summary>
		/// Set up the data linker
		/// </summary>
		public override void Setup() {
			
		}
	}
}
