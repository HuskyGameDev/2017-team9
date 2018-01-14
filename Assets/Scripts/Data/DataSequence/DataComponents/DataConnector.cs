using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PuzzleComponents {
	
	/// <summary>
	/// A data component used to connect two data points or as an end point for a 
	/// </summary>
	public class DataConnector : DataComponent {


		public override DataSequence CalculateOutput() {
			if (this.input == null || this.input.Length <= 0 || this.input[0] == null || this.input[0].IsConnected() == false || this.input[0].partner.owner.GetOutput() == null) {
				//We do not have any output
				//Debug.Log(this.input[0].owner.gameObject.name + " Did not calculate any valid input " + (this.input[0] == null) + " " + (this.input[0].IsConnected() == false) + " " + (this.input[0].owner.GetOutput() == null));
				return null;
			}


			return this.input[0].partner.owner.GetOutput();
			//throw new System.NotImplementedException();
		}

		public override string GetString() {
			return "Connector";
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
