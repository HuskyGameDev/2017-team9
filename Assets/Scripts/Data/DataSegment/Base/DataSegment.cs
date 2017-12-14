using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleComponents {

	/// <summary>
	/// Represents a connected segment of Bits
	/// </summary>
	[System.Serializable]
	public class DataSegment {

		/// <summary>
		/// The internal bit for this DataSegment, cannot have a Bit and children.
		/// </summary>
		public Bit bit;

		/// <summary>
		/// Whether or not the DataSegment is linked, in other words if it can be modified.
		/// </summary>
		public bool linked = false;

		/// <summary>
		/// A list of subconnected DataSegments.
		/// </summary>
		public DataSegment[] children;


		/// <summary>
		/// Performs a deep comparision between two data segemetns and checks if they are equivilent DataSegments
		/// </summary>
		/// <returns></returns>
		public static bool DeepComparison(DataSegment A, DataSegment B) {
			//They cannot be the same if they have a different number of children
			if (A.children.Length != B.children.Length)
				return false;

			//If A is a leaf, and they have the same number of children we can assume B is a leaf
			if (A.isLeaf()) {
				//So we can compare their bits
				return Bit.Compare(A.bit, B.bit);
			}

			//Otherwise we need to make sure that all children are the same
			for (int i = 0; i < A.children.Length; i++) {
				//Check if these children are the same. If they are we continue checking, otherwise we return false.
				if (DeepComparison(A.children[i], B.children[i]) == false) {
					return false;
				}
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
			if (isLeaf()) {
				return new DataSegment(this.bit.CreateCopy(), this.linked);
			}

			//Create an empty datasegment
			DataSegment newData = new DataSegment();
			newData.linked = this.linked;
			//create a new child array the size of ours
			newData.children = new DataSegment[this.children.Length];

			//populate teh new child array with deep copys of our children.
			for (int i = 0; i < children.Length; i++) {
				newData.children[i] = this.children[i].CreateDeepCopy();
			}

			return newData;
		}


		/// <summary>
		/// Adds a child to the END of the list of children.
		/// </summary>
		/// <param name="child"></param>
		/// <returns>Returns true if add was sucessful </returns>
		public bool AddChildLast(DataSegment child) {
			if (linked == true) {
				return false;
			}
			//Decrease size by one, leaving an open space at the end
			Resize(1, 0);

			children[children.Length - 1] = child;

			return true;
		}

		/// <summary>
		/// Adds a child to the START of the list of children.
		/// </summary>
		/// <param name="child"></param>
		/// <returns>Returns true if add was sucessful </returns>
		public bool AddChildFirst(DataSegment child) {
			if (linked == true) {
				return false;
			}
			//Increase size by one, leaving an offset at the begginning
			Resize(1, 1);
			children[0] = child;

			return true;
		}

		/// <summary>
		/// Resizes the internal children array based off of some amount.
		/// A negative amount shrinks the array, and it is assumed that 
		/// the last children are able to be dropped from the list.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="offsetFromStart"></param>
		private void Resize(int amount, int offsetFromStart) {
			if (offsetFromStart > amount) {
				throw new System.Exception("We cannot offset from start that much!");
			}


			if (amount > 0) {
				//We are doing an increase

				//Create a new properly sized array and copy over the proper elements
				DataSegment[] newArray = new DataSegment[children.Length + amount];
				for (int i = 0; i < children.Length; i++) {
					newArray[i + offsetFromStart] = children[i];
				}
			}
			else {
				//We are doing a decrease
				
				//Create a new properly sized array and copy over the proper elements
				DataSegment[] newArray = new DataSegment[children.Length - amount];
				for (int i = 0; i < children.Length - amount; i++) {
					newArray[i + offsetFromStart] = children[i];
				}
			}
		}

		public bool isLeaf() {
			if (bit != null && children.Length == 0)
				return true;
			return false;
		}

		/// <summary>
		/// Remove a DataSegment from the list of children.
		/// </summary>
		/// <param name="index"></param>
		/// <returns>Null if index is out of range, otherwise the removed DataSegement</returns>
		public DataSegment Remove(int index) {
			//Dont seach out of range
			if (index >= children.Length) {
				return null;
			}

			//Store the child for returning.
			DataSegment ret;
			ret = children[index];

			//Perform a slide to remove this from the list
			for (int i = index; i < children.Length - 1; i++) {
				children[i] = children[i+1];
			}

			//Shrink the array
			Resize(-1,0);

			return ret;
		}


		/// <summary>
		/// Remove a DataSegement from the list of children.
		/// </summary>
		/// <param name="segment"></param>
		/// <returns>Null if not found or linked, otherwise the removed data segment </returns>
		public DataSegment Remove(DataSegment segment) {
			if (linked == true) {
				return null;
			}

			//Perform a slide to remove this from the list
			for (int i = 0; i < children.Length; i++) {
				if (children[i] == segment)
					return Remove(i);
			}

			return null;
		}

		/// <summary>
		/// Returns a game usable string to represent the DataSegment
		/// </summary>
		/// <returns></returns>
		public string GetStringRepresentation() {
			//We are a bit holder, so return the bits representation
			if (isLeaf())
				return bit.GetStringRepresentation();

			//Collect the string representations of the children and combine them
			string ret = "[" + GetBitCount() + "]<";
			for (int i = 0; i < children.Length; i++) {
				ret += children[i].GetStringRepresentation();
			}

			return ret + ">";
		}

		/// <summary>
		/// Returns the aggregate bit count of all children.
		/// </summary>
		/// <returns>Aggregate bit count of all children</returns>
		public int GetBitCount() {
			//We are a bit holder, so return 1.
			if (isLeaf())
				return 1;

			//Collect the total of all bits in the children
			int ret = 0;
			for (int i = 0; i < children.Length; i++) {
				ret += children[i].GetBitCount();
			}

			return ret;
		}


		/// <summary>
		/// Create a DataSegment that holds a bit
		/// </summary>
		/// <param name="bit"></param>
		public DataSegment(Bit bit, bool isLinked) {
			this.bit = bit;
			this.linked = isLinked;
		}

		/// <summary>
		/// Create a DataSegment capable of holding children
		/// </summary>
		public DataSegment() {
			//I do not this this needs to do anything.
		}

		/// <summary>
		/// Create a DataSegment capable of holding children with a starting set of children.
		/// </summary>
		public DataSegment(DataSegment[] startChildren, bool isLinked) {
			this.children = startChildren;
			this.linked = isLinked;
		}

	}
}