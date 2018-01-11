using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleComponents {
	public abstract class DataTrigger : MonoBehaviour {
		public abstract void Trigger(DataSequence sequence);
	}
}