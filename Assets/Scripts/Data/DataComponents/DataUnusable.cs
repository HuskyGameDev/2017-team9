using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleComponents {
	//A component that simply marks unusable areas
	public class DataUnusable : DataComponent {
		public override DataSequence CalculateOutput() {
			return null;
		}

		public override string GetString() {
			return "Unusuable";
		}

		public override void Setup() {
			//None Needed
		}
	}
}