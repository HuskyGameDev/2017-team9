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

			//If our one input is not connected, or if its output is null
			if (this.input[0] == null || this.input[0].IsConnected() == false || this.input[0].partner.owner.GetOutput() == null) {
				//We do not have any output
				//Debug.Log(this.input[0].owner.gameObject.name + " Did not calculate any valid input " + (this.input[0] == null) + " " + (this.input[0].IsConnected() == false) + " " + (this.input[0].owner.GetOutput() == null));
				return null;
			}

			//Otherwise get a copy of our inputs data
			DataSequence unlinkedOutput = this.input[0].partner.owner.GetOutput();

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
