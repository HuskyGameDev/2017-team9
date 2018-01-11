using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PuzzleComponents {
	public class DataShifter : DataComponent {

		/// <summary>
		/// The type of shift to perfrom. Even is those in even positions (including zero). Odd is those in odd positions
		/// </summary>
		public enum ShiftType { Total, Even, Odd }
		private static readonly string[] ShiftTypeToText = new string[] { "Total", "Even", "Odd"};
		public ShiftType shiftType = ShiftType.Total;

		public bool moveRight = true;

		/// <summary>
		/// The amount of spaces to be shifted
		/// </summary>
		public int amount = 1;


		public override DataSequence CalculateOutput() {
			if (this.input[0] == null || this.input[0].IsConnected() == false || this.input[0].partner.owner.GetOutput() == null) {
				//We do not have any output
				//Debug.Log(this.input[0].owner.gameObject.name + " Did not calculate any valid input. HasPoint:" + (this.input[0] == null) + "|IsConnected:" + (this.input[0].IsConnected() == false) + "|HasOutput:" + (this.input[0].owner.GetOutput() == null));
				return null;
			}
			DataSequence input = this.input[0].partner.owner.GetOutput();
			//Break down the input so we can perform shifts easier.
			input.Fracture();
			//Debug.Log(input.GetStringRepresentation());
			//Branch on the different shifting types
			for (int i = 0; i < amount; i++) {
				SlideOperation(input.segments.internalArray, shiftType, moveRight);
			}
			//Debug.Log(input.GetStringRepresentation());

			//[TODO] This is not entirely necessary but helpful for getting strings easier. Consider removing for efficiency inthe future,
			input.Simplify();

			return input;
		}

		/// <summary>
		/// Performs a slide on this data segment sequence with respect to linked bits. It is recommended that the DataSequence is fractured first for proper results.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="type"></param>
		/// <param name="moveRight"></param>
		public static void SlideOperation(DataSegment[] input, ShiftType type, bool moveRight) {

			if (type == ShiftType.Total) {
				DataSegment temp;
				//Branch on the direction we are moving. 
				if (moveRight) {
					//Store the end element
					temp = input[input.Length - 1];
					//For each element in the array starting at the end, set it equal to the element before it
					for (int i = input.Length - 1; i > 0; i--) {
						input[i] = input[i - 1];
					}
					//put what was the end element at the beginning
					input[0] = temp;
				}
				else {
					//Store the first element
					temp = input[0];
					//For each element, set it equal to the element after it
					for (int i = 0; i < input.Length - 1; i++) {
						input[i] = input[i + 1];
					}
					//Put what was the first element at the beginning
					input[input.Length - 1] = temp;
				}
			}
			else if (type == ShiftType.Even || type == ShiftType.Odd) {
				//We can group the code for even and odd by using an offset.
				int offset = (type == ShiftType.Even) ? 0 : 1;
				//Create a temporary holder for the end element of the slide, the one that wraps around
				DataSegment temp;
				//Branch on the direction we are moving
				if (moveRight) {
					//If even, the second to last element is the one that wraps around, if odd the last element is the one that wraps around
					temp = input[input.Length - 2 + offset];
					//For each element in the array starting at the end (or second to the end in the case of even), assign it to the element two before it.
					for (int i = input.Length - 2 + offset; i > offset; i -= 2) {
						input[i] = input[i - 2];
					}
					//Wrap the saved element to the end
					input[offset] = temp;
				}
				else {
					//If even, the first element wraps around, if odd the second element wraps around
					temp = input[offset];
					for (int i = offset; i < input.Length - 2 + offset; i += 2) {
						input[i] = input[i + 2];
					}
					//Wrap the saved element to the end
					input[input.Length - 2 + offset] = temp;
				}
			}
		}

		public override string GetString() {
			return "Shifter " + ShiftTypeToText[(int)shiftType] + "| " + ((moveRight) ? "Right":"Left")+">" + amount;
		}

		public override void Setup() {
			//Not needed
			//throw new System.NotImplementedException();
		}

		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {

		}
	}
}
