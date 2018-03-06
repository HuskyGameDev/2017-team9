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
			return null;
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
