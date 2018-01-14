using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleComponents {
	public abstract class DataTrigger : MonoBehaviour {
		public DataSegment[] triggerData;

		public bool playerVisibleTrigger = false;

		public void DataChange(DataSequence sequence) {
			if (sequence != null && DataSequence.Comparison(new DataSequence(triggerData), sequence) == true) {
				Trigger();
			}
		}

		public abstract void Trigger();
	}
}