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

		public enum State { X, Y, Z}
		public static readonly string[] StateToString = new string[] { "X", "Y", "Z"};

		[SerializeField]
		public State state;

		/// <summary>
		/// Create a Bit with the given state.
		/// </summary>
		/// <param name="state"></param>
		public Bit(State state) {
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
			return StateToString[(int)state];
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
