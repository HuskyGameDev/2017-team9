using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDataStructures;

namespace PuzzleComponents {

	/// <summary>
	/// Represents a connected segment of Bits
	/// </summary>
	[System.Serializable]
	public class DataSegment {

		/// <summary>
		/// A list of bits in this Data Segment
		/// </summary>
		[SerializeField]
		public OrderedBits bits;

		/// <summary>
		/// Tracks if this collection of bits is linked together.
		/// </summary>
		public bool linked = false;


		/// <summary>
		/// Performs a comparision between two data segemetns and checks if they are equivilent
		/// </summary>
		/// <returns></returns>
		public static bool Comparison(DataSegment A, DataSegment B) {
			//If either of them is null, return null unless they are both null.
			if (A == null || B == null)
				return (A == null && B == null);

			//They cannot be the same if they have a different number of bits
			if (A.bits.Length != B.bits.Length)
				return false;

			//Check all of the bits
			for (int i = 0; i < A.bits.Length; i++) {
				if (A.bits.Get(i).state != B.bits.Get(i).state)
					return false;
			}

			//If we have not hit a fail condition by this point, we assume they are equal.
			return true;
		}


		/// <summary>
		/// Create and return a deep copy of this data segement
		/// </summary>
		/// <returns>DataSegement</returns>
		public DataSegment CreateDeepCopy() {
			//If we are a leaf we just return 

			//create a new child array the size of ours
			Bit[] temp = new Bit[this.bits.Length];

			//populate teh new child array with deep copys of our bits.
			for (int i = 0; i < bits.Length; i++) {
				temp[i] = this.bits.Get(i).CreateCopy();
			}

			//Create the return variable
			DataSegment ret = new DataSegment(temp);
			ret.linked = this.linked;

			return ret;
		}

		/// <summary>
		/// Returns a game usable string to represent the DataSegment
		/// </summary>
		/// <returns></returns>
		public string GetStringRepresentation() {

			//Collect the string representations of the bits and combine them
			string ret = "[" + GetBitCount() + "]" + ((linked) ? "<" : "(");
			for (int i = 0; i < bits.Length; i++) {
				ret += bits.Get(i).GetStringRepresentation();
			}

			return ret + ((linked) ? ">" : ")");
		}

		/// <summary>
		/// Returns the aggregate bit count of all bits.
		/// </summary>
		/// <returns>Aggregate bit count of all bits</returns>
		public int GetBitCount() {
			return bits.Length;
		}

		/// <summary>
		/// Create a DataSegment capable of holding bits with a starting set of bits.
		/// </summary>
		public DataSegment(Bit[] startBits) {
			this.bits = new OrderedBits(startBits);
		}

	}
}