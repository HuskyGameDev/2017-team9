using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PuzzleComponents {
	public class DataCombiner : DataComponent {

		public enum CombineType { Left, Center, Right};

		public override DataSequence CalculateOutput() {
			throw new System.NotImplementedException();
		}

		public override string GetString() {
			return "Combiner";
			//throw new System.NotImplementedException();
		}

		public override void Setup() {
			throw new System.NotImplementedException();
		}

		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {

		}
	}
}
