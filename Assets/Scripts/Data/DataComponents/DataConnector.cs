using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PuzzleComponents {
	
	/// <summary>
	/// A data component used to connect two data points or as an end point for a 
	/// </summary>
	public class DataConnector : DataComponent {

		//The connector does not have output
		public override DataSequence CalculateOutput() {
			//We can only take in one input
			int foundInput = -1;
			for (int i = 0; i < inputs.Length; i++) {
				if (inputs[i] != null) {
					Debug.Log(inputs[i]);
					foundInput = i;
				}
			}
			//If we found nothing, we return null
			if (foundInput == -1)
				return null;

			return inputs[foundInput];
		}

		public override string GetString() {
			return "Connector";
		}

		public override void Setup() {
			//None needed
			//throw new System.NotImplementedException();
		}
	}
}
