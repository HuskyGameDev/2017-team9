using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDataStructures;

namespace PuzzleComponents {
	/// <summary>
	/// An ordered set of data sequences, the top level representation of data.
	/// </summary>
	[System.Serializable]
	public class DataSequence {

		/// <summary>
		/// The list of ordered Sequences in this sequence.
		/// </summary>
		[SerializeField]
		public OrderedList<DataSegment> segments;


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="startSequences"></param>
		public DataSequence(DataSegment[] startSegments) {
			segments = new OrderedList<DataSegment>(startSegments);
		}

		/// <summary>
		/// Performs a comparision between two data segemetns and checks if they are equivilent
		/// </summary>
		/// <returns></returns>
		public static bool Comparison(DataSequence A, DataSequence B) {
			//If either of them is null, return null unless they are both null.
			if (A == null || B == null)
				return (A == null && B == null);

			//They cannot be the same if they have a different number of bits
			if (A.segments.Length != B.segments.Length)
				return false;

			//Check all of the bits
			for (int i = 0; i < A.segments.Length; i++) {
				if (DataSegment.Comparison(A.segments.Get(i), B.segments.Get(i)) == false)
					return false;
			}

			//If we have not hit a fail condition by this point, we assume they are equal.
			return true;
		}


		/// <summary>
		/// Create and return a deep copy of this data segement
		/// </summary>
		/// <returns>DataSegement</returns>
		public DataSequence CreateDeepCopy() {
			//create a new child array the size of ours
			DataSegment[] temp = new DataSegment[segments.Length];

			//populate the new child array with deep copys of our bits.
			for (int i = 0; i < segments.Length; i++) {
				temp[i] = this.segments.Get(i).CreateDeepCopy();
			}

			return new DataSequence(temp);
		}

		/// <summary>
		/// Returns a game usable string to represent the DataSequence
		/// </summary>
		/// <returns></returns>
		public string GetStringRepresentation() {
			//Collect the string representations of the bits and combine them
			string ret = "[" + GetBitCount() + "]{";
			for (int i = 0; i < segments.Length; i++) {
				ret += segments.Get(i).GetStringRepresentation();
			}
			
			return ret + "}";
		}

		/// <summary>
		/// Returns the aggregate bit count of all bits.
		/// </summary>
		/// <returns>Aggregate bit count of all bits</returns>
		public int GetBitCount() {
			int total = 0;
			//Debug.Log(segments.Length);
			for (int i = 0; i < segments.Length; i++) {
				if (segments.Get(i) == null) {
					Debug.Log("Uhoh, we have an incorrect OrderedList?");
					continue;
				}
				//Debug.Log(segments.Get(i).ToString());
				total += segments.Get(i).GetBitCount();
			}
			return total;
		}


		/// <summary>
		/// Returns only the bits of this sequence, with no underlying structure
		/// </summary>
		/// <returns></returns>
		public Bit[] GetOrderedBitSequence() {
			//the return variable
			Bit[] ret = new Bit[GetBitCount()];

			//Track the postion in the return variable
			int q = 0;

			//Loop through all segements, and all bits in that segment
			for (int i = 0; i < segments.Length; i++) {
				for (int k = 0; k < segments.Get(i).bits.Length; k++) {
					//Assign in return variable
					ret[q] = segments.Get(i).bits.Get(k);
					//Increment count
					q++;
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns a bit at an index of the rought bit sequence, ignoring segments and linked segments.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="isLinked"></param>
		/// <param name="segment"></param>
		/// <param name="segmentIndex"></param>
		/// <returns></returns>
		public Bit GetBitAtIndex(int index, out bool isLinked, out DataSegment segment, out int segmentIndex) {
			//setup a total tracker
			int count = 0;
			//Loop through all segments
			for (int i = 0; i < segments.Length; i++) {
				//Loop through all bits in the segment
				for (int k = 0; k < segments.Get(i).bits.Length; k++) {
					if (count == index) {
						// if we have reached our count, set isLinked and return this bit
						isLinked = segments.Get(i).linked;
						segment = segments.Get(i);
						segmentIndex = i;
						return segments.Get(i).bits.Get(k);
					}
					count++;
				}
			}

			//Default returns
			isLinked = false;
			segment = null;
			segmentIndex = 0;
			return null;
		}

		/// <summary>
		/// Combines all non-linked segments around linked segments to minimize segment count.
		/// </summary>
		public void Simplify() {
			OrderedList<DataSegment> newSegments = new OrderedList<DataSegment>(0);
			DataSegment combinedSegment = new DataSegment(new Bit[] { });
			//this loop works by creating a new segment to fill with bits. it gets added when we encounter the end or hit a linked segment
			for (int i = 0; i < segments.Length; i++) {
				if (segments.Get(i).linked) {
					//Check if the combined segment we are creating has any content.
					if (combinedSegment.bits.Length > 0) {
						//Add it
						newSegments.AddElementAtEnd(combinedSegment);
						//Create a new empy segment
						combinedSegment = new DataSegment(new Bit[] { });
					}
					//Add this segment to the new segments
					newSegments.AddElementAtEnd(segments.Get(i));
				}
				else {
					//Loop through this segment and add all its bits to the the combinedSegment
					for (int k = 0; k < segments.Get(i).bits.Length; k++) {
						combinedSegment.bits.AddElementAtEnd(segments.Get(i).bits.Get(k));
					}
				}
			}
			//If we have left over things to add make sure to add them
			if (combinedSegment.bits.Length > 0) {
				//Add it
				newSegments.AddElementAtEnd(combinedSegment);
			}

			//Assign the new segments.
			segments = newSegments;
		}

		/// <summary>
		/// Breaks all non-linked data segements down into single bit segments.
		/// </summary>
		public void Fracture() {
			OrderedList<DataSegment> newSegments = new OrderedList<DataSegment>(0);
			//loop through all our current segments
			for (int i = 0; i < segments.Length; i++) {
				//If we have no reason to modify it, just copy it over
				if (segments.Get(i).linked || segments.Get(i).bits.Length == 1) {
					newSegments.AddElementAtEnd(segments.Get(i));
				}
				else {
					//Otherwise break it down bit by bit and add it
					for (int k = 0; k < segments.Get(i).bits.Length; k++) {
						newSegments.AddElementAtEnd(new DataSegment(new Bit[] { segments.Get(i).bits.Get(k).CreateCopy()}));
					}
				}
			}
			//Assign new segments
			segments = newSegments;
		}

	}
}
