using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleComponents {

	/// <summary>
	/// This component represents a source DataSegment for the puzzle
	/// </summary>
	public class DataSource : DataComponent {

		/// <summary>
		/// The source DataSegment for this Data Source (redundant sounding)
		/// </summary>
		private DataSequence source = new DataSequence(new DataSegment[0]);

		/// <summary>
		/// This stores the segments
		/// </summary>
		public DataSegment[] _source;

		/// <summary>
		/// The calculation for this component is simple, just return the segment as specified from the editor.
		/// </summary>
		/// <returns></returns>
		public override DataSequence CalculateOutput() {
			//Debug.Log(source.GetStringRepresentation());
			return source;
		}

		public override string GetString() {
			return "Source";
		}

		public override void Setup() {
			source = new DataSequence(_source);
		}
	}
}
