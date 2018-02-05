using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CustomDataStructures {
	/// <summary>
	/// A data self resizing array that garuntees order is maintanted.
	/// </summary>.
	[System.Serializable]
	public class OrderedList<T> {

		[SerializeField]
		public T[] internalArray;

		public int Length {
			get { return internalArray.Length; }
			set { }
		}

		/// <summary>
		/// Constructor, taking a starting size for optimization.
		/// </summary>
		/// <param name="startSize"></param>
		public OrderedList(int startSize) {
			internalArray = new T[startSize];
		}

		/// <summary>
		/// Constructor, taking a starting size for optimization.
		/// </summary>
		/// <param name="startSize"></param>
		public OrderedList(T[] startElements) {
			internalArray = startElements;
		}


		/// <summary>
		/// Adds a an element to the ordered list
		/// </summary>
		/// <param name="element"></param>
		/// <returns>Returns true if add was sucessful </returns>
		public bool AddElementAtPosition(T element, int index) {

			//Cannot add outside of our bounds, length is in bounds size we will be resizing
			if (index < 0 || index > Length)
				return false;

			//Decrease size by one, leaving an open space at the end
			Resize(1, 0);

			//perform a slide so the desired spot is open
			for (int i = Length - 1; i > index; i--) {
				internalArray[i] = internalArray[i - 1];
			}
			//Debug.Log(" " +Length+ "|" + index);
			internalArray[index] = element;

			return true;
		}

		/// <summary>
		/// Adds a an element to the ordered list
		/// </summary>
		/// <param name="element"></param>
		/// <returns>Returns true if add was sucessful </returns>
		public bool AddElementAtEnd(T element) { 
			return AddElementAtPosition(element, Length);
		}

		public T[] ToArray() {
			return internalArray;
		}

		/// <summary>
		/// Remove an object from the ordered list
		/// </summary>
		/// <param name="index"></param>
		/// <returns>Null if index is out of range, otherwise the removed DataSegement</returns>
		public T Remove(int index) {

			//Can only remove within bounds
			if (index < 0 || index > Length)
				return default(T);


			//Store the element for returning.
			T ret = internalArray[index];

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
		public T Get(int index) {
			if (index < 0 || index >= Length)
				return default(T);
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
			if (offsetFromStart > amount) {
				throw new System.Exception("We cannot offset from start that much!");
			}

			T[] newArray;
			if (amount > 0) {
				//We are doing an increase

				//Create a new properly sized array and copy over the proper elements
				newArray = new T[internalArray.Length + amount];
				for (int i = 0; i < internalArray.Length; i++) {
					newArray[i + offsetFromStart] = internalArray[i];
				}
			}
			else {
				//We are doing a decrease

				//Create a new properly sized array and copy over the proper elements
				newArray = new T[internalArray.Length - amount];
				for (int i = 0; i < internalArray.Length - amount; i++) {
					newArray[i + offsetFromStart] = internalArray[i];
				}
			}
			internalArray = newArray;
		}

	}
}
