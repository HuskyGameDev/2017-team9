using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PuzzleComponents {
	public class DataCombiner : DataComponent {

		public enum CombineType { Left, Center, Right};

		public override DataSequence CalculateOutput() {
			DataSequence output = new DataSequence(new DataSegment[] { });

			for (int i = 0; i < this.GetInput().Length; i++) {
				DataSequence dataInput = this.GetInput()[i].GetOutput();
				for (int k = 0; k < dataInput.segments.Length; k++) {
					output.segments.AddElementAtEnd(dataInput.segments.Get(k));
				}
				
			}

			return output;
		}

		public override string GetString() {
			return "Combiner";
		}

		public override void Setup() {
			//none Needed
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
