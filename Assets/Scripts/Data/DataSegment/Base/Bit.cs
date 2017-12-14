using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleComponents {

	/// <summary>
	/// The base class for Puzzle Bit representation
	/// </summary>
	/// 
	[System.Serializable]
	public class Bit {
		public bool state;

		/// <summary>
		/// Create a Bit with default 'False' state.
		/// </summary>
		public Bit() {
			state = false;
		}

		/// <summary>
		/// Create a Bit with the given state.
		/// </summary>
		/// <param name="state"></param>
		public Bit(bool state) {
			this.state = state;
		}

		/// <summary>
		/// Compares two bits to see if they have the same state
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool Compare(Bit a, Bit b) {

			return a.state == b.state;
		}


		/// <summary>
		/// Get a string reprensentation of the bit for Game purposes
		/// </summary>
		/// <returns></returns>
		public string GetStringRepresentation() {
			return (state) ? "1" : "0";
		}

		/// <summary>
		/// Create and return a copy of this bit
		/// </summary>
		/// <returns></returns>
		public Bit CreateCopy() {
			return new Bit(this.state);
		}
	}
}
