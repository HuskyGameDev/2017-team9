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



			//If we are comparing no bits it is true.
			if (A.segments.Length == 0 && B.segments.Length == 0) {
				return true;
			}
			else if (A.segments.Length == 0 || B.segments.Length == 0) {
				//But if only one of them is empty we cant match
				return false;
			}

			//Now we operate on all bits
			//We keep track of the segment we are on
			int currentSegmentA = 0;
			int currentSegmentB = 0;
			//We keep track of the bits we are currently looking at
			int currentBitA = 0;
			int currentBitB = 0;
			Debug.Log("Comparing: ");
			Debug.Log(A.GetStringRepresentation());
			Debug.Log(B.GetStringRepresentation());

			while (true) {

				if (currentBitA >= A.segments.Get(currentSegmentA).bits.Length) {
					//We have reached the end of this segment, move on to the next one
					currentBitA = 0;
					currentSegmentA++;
				}
				if (currentBitB >= B.segments.Get(currentSegmentB).bits.Length) {
					//We have reached the end of this segment, move on to the next one
					currentBitB = 0;
					currentSegmentB++;
				}

				if (currentSegmentA >= A.segments.Length && currentSegmentB >= B.segments.Length) {
					//If we have made it to the end of all segments and have not hit a fail condition, that means we are equal.
					Debug.Log("Passed");
					return true;
				}

				if (currentSegmentA >= A.segments.Length || currentSegmentB >= B.segments.Length) {
					//We have reached the end of only one of them, we cannot be the same
					Debug.Log("Failed");
					return false;
				}

				if (A.segments.Get(currentSegmentA).linked != B.segments.Get(currentSegmentB).linked) {
					//Implicitly, we can assume that if both are not in a linked segment at the same time they are not equal
					Debug.Log("Failed");
					return false;
				}

				if (A.segments.Get(currentSegmentA).bits.Get(currentBitA).state != B.segments.Get(currentSegmentB).bits.Get(currentBitB).state) {
					//The sate of the two bits we are looking at doesnt match, these cant be equal.
					Debug.Log("Failed");
					return false;
				}



				//Advance the bits we are looking at
				currentBitA++;
				currentBitB++;
			}
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
