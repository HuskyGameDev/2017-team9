using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllTheColorsOfTheWind {
	/// <summary>
	/// Base class for ColorComponents. ColorComponents modify a Color in unique ways.
	/// </summary>
	public abstract class ColorComponent : MonoBehaviour {

		#region Static
		/// <summary>
		/// Used to ensure data components are setup correctly by restricting the number of inputs it has.
		/// </summary>
		public enum SocketCountType { None, Exact, AtLeast }
		/// <summary>
		/// A string array to conver the SocketCountType enum to a string
		/// </summary>
		public static readonly string[] socketCountTypeToString = new string[] { "None", "Exact", "AtLeast" };
		#endregion



		#region Properties
		#region Public
		public GridSquare square;
		public ColorTrigger[] triggers;

		#endregion
		#region Private
		private ColorBit _cache = new ColorBit(null);
		private ColorBit cache {
			get {
				return _cache;
			}
			set {
				_cache = value;
				foreach (ColorTrigger t in triggers) {
					t.Check(_cache);
				}
			}
		}
		#endregion
		#region Private
		private List<ColorBit> inputs;
		#endregion
		#endregion



		#region Methods
		#region Public

		public void Awake() {
			inputs = new List<ColorBit>();

			Setup();

			CheckOutput();
		}

		/// <summary>
		/// Returns the output of this data component.
		/// </summary>
		/// <returns></returns>
		public ColorBit GetOutput() {
			CheckOutput();
			return cache;
		}

		public bool CheckOutput() {
			Debug.Log("told to Check Output: " + GetString());
			GetInput();
			ColorBit newResult = CalculateOutput();
			if (newResult.Equals(cache) == false) {
				Debug.Log("And I actually could! " + ((newResult.nulled) ? "Null" : newResult.ToString()));
				//Update the result
				cache = newResult;
				UpdateConnectedComponents();
				return true;
			}
			return false;
		}

		public void UpdateConnectedComponents() {
			//Signal all output connections that we have changed our data.
			//For each of our squares lines

			for (int i = 0; i < square.socketState.Length; i++) {
				if (square.socketState[i] == GridSquare.SocketState.Output && square.lines[i] != null) {
					//get the line flowing from this direction
					LinkedList<KeyValuePair<GridSquare, GridSquare.GridDirection>> foundLine = this.square.GetLine((GridSquare.GridDirection)i);
					//If it is a component
					if (foundLine.Last.Value.Key.type != GridSquare.GridType.Empty) {
						//And if it is not us and and on an input socket 
						if (foundLine.Last.Value.Key != this.square && foundLine.Last.Value.Key.socketState[(int)foundLine.Last.Value.Value] == GridSquare.SocketState.Input) {
							//Tell them to check their output
							foundLine.Last.Value.Key.component.CheckOutput();
						}
					}
				}
			}
		}

		/// Searches down the lines for any connected inputs along valid input
		/// </summary>
		/// <returns></returns>
		public void GetInput() {
			//Set our inputs to blank
			inputs = new List<ColorBit>();

			for (int i = 0; i < square.socketState.Length; i++) {
				if (square.socketState[i] == GridSquare.SocketState.Input && square.lines[i] != null) {
					//get the line flowing from this direction
					LinkedList<KeyValuePair<GridSquare, GridSquare.GridDirection>> foundLine = this.square.GetLine((GridSquare.GridDirection)i);
					//If it is a component
					if (foundLine.Last.Value.Key.type != GridSquare.GridType.Empty) {
						//And if it is not us and and on an input socket 
						if (foundLine.Last.Value.Key != this.square && foundLine.Last.Value.Key.socketState[(int)foundLine.Last.Value.Value] == GridSquare.SocketState.Output) {
							//Tell them to check their output
							inputs.Add(foundLine.Last.Value.Key.component.GetOutput());
						}
					}
				}
			}
		}

		/// <summary>
		/// Formats an output string to represnt the color
		/// </summary>
		/// <returns></returns>
		public string GetOutputString() {
			return cache.ToString();
		}

		/// <summary>
		/// Checks the cached inputs to see if they match required numbers and returns a properly sized array
		/// </summary>
		/// <returns></returns>
		public ColorBit[] ValidateInput() {
			//First, check if we should even have input
			if (InputCountType() == SocketCountType.None) {
				Debug.LogWarning(GetString() + "Failed Input Validation by : No Input allowed");
				return new ColorBit[0];
			}
			else {
				//Keep track of things to return
				List<ColorBit> ret = new List<ColorBit>();
				//Find the valid inputs and track them
				for (int i = 0; i < inputs.Count; i++) {
					if (inputs[i].nulled == false) {
						Debug.Log(inputs[i]);
						ret.Add(inputs[i]);
					}
				}


				if (InputCountType() == SocketCountType.Exact && ret.Count == InputCount()) {
					//If we have to be exact and we are, return
					return ret.ToArray();
				}
				else if (InputCountType() == SocketCountType.AtLeast && ret.Count >= InputCount()) {
					//If we have to be at least and we are return
					return ret.ToArray();
				}
				else {
					//otherwise we have failed
					Debug.LogWarning(GetString() + " Failed Input Validation by : Not matching (" + ret.Count + "/" + InputCount()+ ")");

					return new ColorBit[0];
				}
			}
		}
		#endregion
		#endregion


		#region Abstract
		/// <summary>
		/// Calculates the output of this data component
		/// </summary>
		/// <returns></returns>
		public abstract ColorBit CalculateOutput();
		/// <summary>
		///Returns a text string of what this component does.
		/// </summary>
		/// <returns></returns>
		public abstract string GetString();
		/// <summary>
		/// Any setup that the components need to do. Called on awake.
		/// </summary>
		public abstract void Setup();

		/// <summary>
		/// Returns the socker count type for inputs. used to ensure proper grid creation
		/// </summary>
		/// <returns></returns>
		public abstract SocketCountType InputCountType();
		/// <summary>
		/// Returns the socker count type for outputs. used to ensure proper grid creation
		/// </summary>
		/// <returns></returns>
		public abstract SocketCountType OutputCountType();
		/// <summary>
		/// The number to use when caclulating SocketCountType for input.
		/// </summary>
		/// <returns></returns>
		public abstract int InputCount();
		/// <summary>
		/// The number to use when caclulating SocketCountType for output.
		/// </summary>
		/// <returns></returns>
		public abstract int OutputCount();
		#endregion
	}
}