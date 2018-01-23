using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PuzzleComponents {
	/// <summary>
	/// A data self resizing array that garuntees order is maintanted. This is Bits specifically instead of using the generic one because unity didt want to display the bits for some reason
	/// </summary>.
	[System.Serializable]
	public class OrderedBits {

		[SerializeField]
		public Bit[] internalArray;

		public int Length {
			get { return internalArray.Length; }
			set { }
		}

		/// <summary>
		/// Constructor, taking a starting size for optimization.
		/// </summary>
		/// <param name="startSize"></param>
		public OrderedBits(int startSize) {
			internalArray = new Bit[startSize];
		}

		/// <summary>
		/// Constructor, taking a starting size for optimization.
		/// </summary>
		/// <param name="startSize"></param>
		public OrderedBits(Bit[] startElements) {
			internalArray = startElements;
		}


		/// <summary>
		/// Adds a an element to the ordered list
		/// </summary>
		/// <param name="element"></param>
		/// <returns>Returns true if add was sucessful </returns>
		public bool AddElementAtPosition(Bit element, int index) {

			//Cannot add outside of our bounds, length is in bounds size we will be resizing
			if (index < 0 || index > Length)
				return false;

			//Decrease size by one, leaving an open space at the end
			Resize(1, 0);

			//perform a slide so the desired spot is open
			for (int i = Length - 1; i > index; i--) {
				internalArray[i] = internalArray[i - 1];
			}
			//Debug.Log(" " + Length + "|" + index);
			internalArray[index] = element;

			return true;
		}

		/// <summary>
		/// Adds a an element to the ordered list
		/// </summary>
		/// <param name="element"></param>
		/// <returns>Returns true if add was sucessful </returns>
		public bool AddElementAtEnd(Bit element) {
			return AddElementAtPosition(element, Length);
		}

		public Bit[] ToArray() {
			return internalArray;
		}

		/// <summary>
		/// Remove an object from the ordered list
		/// </summary>
		/// <param name="index"></param>
		/// <returns>Null if index is out of range, otherwise the removed DataSegement</returns>
		public Bit Remove(int index) {

			//Debug.Log("Remove Index: " + index);

			//Can only remove within bounds
			if (index < 0 || index > Length)
				return default(Bit);


			//Store the element for returning.
			Bit ret = internalArray[index];

			//Perform a slide to remove this from the list
			for (int i = index; i < Length - 1; i++) {
				internalArray[i] = internalArray[i + 1];
			}

			//Shrink the array
			Resize(-1, 0);

			return ret;
		}

		/// <summary>
		/// Returns the element at index, or the default if index is out of bounds.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Bit Get(int index) {
			if (index < 0 || index >= Length)
				return default(Bit);
			return internalArray[index];
		}


		/// <summary>
		/// Resizes the internal array based off of some amount.
		/// A negative amount shrinks the array, and it is assumed that 
		/// the last elements are able to be dropped from the list.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="offsetFromStart"></param>
		private void Resize(int amount, int offsetFromStart) {
			if ( offsetFromStart > 0 && offsetFromStart > amount) {
				throw new System.Exception("We cannot offset from start that much! " + amount + "|" + offsetFromStart);
			}

			//Debug.Log("Size: " + internalArray.Length + " |Delta: " + amount);

			Bit[] newArray;
				//Create a new properly sized array and copy over the proper elements
				newArray = new Bit[internalArray.Length + amount];
				for (int i = 0; i < newArray.Length && i < internalArray.Length; i++) {
					newArray[i + offsetFromStart] = internalArray[i];
				}

			internalArray = newArray;
		}

	}
}
