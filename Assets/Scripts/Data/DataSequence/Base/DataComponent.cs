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

		public void Awake() {

			//Setup the non connection based part of the component.
			Setup();

			//Make sure our output is appropriate now that everything is set up.
			ConnectionChange();

		}

		/// <summary>
		/// Recalculates output and signals output connections to update if the result is different from the cache.
		/// </summary>
		public void ConnectionChange() {
			//Debug.Log("I have been told to update " + this.gameObject.transform.name);
			//get the new output
			DataSequence newResult = CalculateOutput();

			//Compare it to the cached DataSequence
			if (DataSequence.Comparison(cache, newResult) == false) {
				//Debug.Log("I do need to update! " + this.gameObject.transform.name);
				//Update the result
				cache = newResult;

				//Signal all output connections that we have changed our data.
				for (int i = 0; i < attachedSquare.line.Length; i++) {
					if (attachedSquare.connected[i] && (attachedSquare.socketState[i] == GridSquare.SocketState.Output || attachedSquare.socketState[i] == GridSquare.SocketState.Omni) && attachedSquare.line[i].GetOther(this) != null)
						attachedSquare.line[i].GetOther(this).ConnectionChange();
				}
				
			}
			//Otherwise we dont have to do anything since nothing has changed
		}

		/// <summary>
		/// Returns the output of this data component.
		/// </summary>
		/// <returns></returns>
		public DataSequence GetOutput() {
			if (cache == null)
				return null;
			return cache.CreateDeepCopy();
		}

		/// <summary>
		/// Searches down the lines for any connected inputs along valid input
		/// </summary>
		/// <returns></returns>
		public DataComponent[] GetInput() {
			List<DataComponent> inputs = new List<DataComponent>();
			for (int i = 0; i < attachedSquare.line.Length; i++) {
				if ((attachedSquare.socketState[i] == GridSquare.SocketState.Input || attachedSquare.socketState[i] == GridSquare.SocketState.Omni) 
					&& attachedSquare.line[i] != null 
					&& attachedSquare.line[i].GetOther(this) != null) 
					{
					inputs.Add(attachedSquare.line[i].GetOther(this));
				}
			}
			return inputs.ToArray();
		}

		/// <summary>
		/// REturns the data segements string with protection against it being null
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
