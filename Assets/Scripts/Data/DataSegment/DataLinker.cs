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
		public override DataSegment CalculateOutput() {

			//If our one input is not connected, or if its output is null
			if (this.input[0] == null || this.input[0].IsConnected() == false || this.input[0].owner.GetOutput() == null) {
				//We do not have any output
				return null;
			}

			//Otherwise get a copy of our inputs data and set it to linked
			DataSegment newOutput = this.input[0].owner.GetOutput();

			newOutput.linked = true;

			return newOutput;
		}
	}
}
