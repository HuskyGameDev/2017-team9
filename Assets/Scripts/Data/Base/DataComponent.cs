﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleComponents {

	/// <summary>
	/// A collection of DataPoints and a way in which they are modified.
	/// This class containts the base level setup for all components.
	/// It includes the caching system and the way in which data is 
	/// </summary>
	public abstract class DataComponent : MonoBehaviour {

		public GridSquare attachedSquare;

		public DataTrigger[] triggers;


		/// <summary>
		/// Stores the previously calculated input.
		/// </summary>
		private DataSequence cache {
			get {
				return _cache;
			}
			set {
				_cache = value;
				foreach (DataTrigger t in triggers) {
					t.DataChange(_cache);
				}
			}
		}

		private DataSequence _cache = null;


		public DataSequence[] inputs;

		public void Awake() {
			inputs = new DataSequence[] { null, null, null, null };
			//Setup the non connection based part of the component.
			Setup();

			//Make sure our output is appropriate now that everything is set up.
			CheckOutput();

		}

		public bool CheckOutput() {
			GetInput();
			DataSequence newResult = CalculateOutput();
			if (DataSequence.Comparison(cache, newResult) == false) {
				Debug.Log("And I actually could! " + ((newResult == null) ? "Null" : newResult.GetStringRepresentation()));
				//Update the result
				cache = newResult;
				UpdateConnectedComponents();
				return true;
			}
			return false;
		}

		public void UpdateConnectedComponents() {
			//Signal all output connections that we have changed our data.
			for (int i = 0; i < attachedSquare.line.Length; i++) {
				if (attachedSquare.line[i] != 0 && attachedSquare.socketState[i] == GridSquare.SocketState.Output) {

					//Use the GridLine method to find the other socket
					GridSquare.GridDirection otherSocketDirection;
					GridSquare other = attachedSquare.FindDataComponentInDirection((GridSquare.GridDirection)i, out otherSocketDirection);
					//If it exists, and that socket is set as an input.
					if (other != null && other.dataComponent != null && other.socketState[(int)otherSocketDirection] == GridSquare.SocketState.Input) {
						//Let them know things have changed.
						other.dataComponent.CheckOutput();
					}
				}
			}
		}

		/// <summary>
		/// Returns the output of this data component.
		/// </summary>
		/// <returns></returns>
		public DataSequence GetOutput() {
			CheckOutput();
			if (cache == null)
				return null;
			return cache.CreateDeepCopy();
		}

		/// <summary>
		/// Searches down the lines for any connected inputs along valid input
		/// </summary>
		/// <returns></returns>
		public void GetInput() {

			for (int i = 0; i < attachedSquare.line.Length; i++) {
				inputs[i] = null;
				//If this is an input socket
				if (attachedSquare.socketState[i] == GridSquare.SocketState.Input) {
					//Check if we have a line
					if (attachedSquare.line[i] != 0) {

						//Call the line method to get the other socket
						GridSquare.GridDirection otherSocketDirection;
						GridSquare other = attachedSquare.FindDataComponentInDirection((GridSquare.GridDirection)i, out otherSocketDirection);

						Debug.Log(other);
						//If it is an output, we can include it
						if (other != null && other.dataComponent != null && other.socketState[(int)otherSocketDirection] == GridSquare.SocketState.Output) {
							Debug.Log("Found an Input!");
							inputs[i] = other.dataComponent.GetOutput();
						}
					}
				}
			}
		}

		/// <summary>
		/// Returns the data segements string with protection against it being null
		/// </summary>
		/// <returns></returns>
		public string GetOutputString() {
			if (cache == null)
				return "[N]<NULL>";
			else {
				DataSequence t = GetOutput();
				t.Simplify();
				return t.GetStringRepresentation();
			}
		}

		/// <summary>
		/// Calculates the output of this data component
		/// </summary>
		/// <returns></returns>
		public abstract DataSequence CalculateOutput();


		/// <summary>
		/// Returns a game usable string of information about the component
		/// </summary>
		/// <returns></returns>
		public abstract string GetString();

		/// <summary>
		/// Called in awake so the comonent can do any setup procedures. This happens before any connections are established. So it cannot be used for preparing data relying on connections.
		/// </summary>
		public abstract void Setup();

	}	
}
