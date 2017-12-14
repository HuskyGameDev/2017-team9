using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleComponents {

	/// <summary>
	/// A collection of DataPoints and a way in which they are modified.
	/// This class containts the base level setup for all components.
	/// It includes the caching system and the way in which data is 
	/// </summary>
	public abstract class DataComponent : MonoBehaviour {

		/// <summary>
		/// The list of input points that this component uses
		/// </summary>
		public DataPoint[] input;

		/// <summary>
		/// The list of output points that this component uses
		/// </summary>
		public DataPoint[] output;


		/// <summary>
		/// Stores the previously calculated input.
		/// </summary>
		private DataSegment cache = null;

		public void Awake() {
			//Make sure we own all of our datapoints
			foreach (DataPoint p in input) {
				p.owner = this;
			}
			foreach (DataPoint p in output) {
				p.owner = this;
			}


			//Make sure our output is appropriate.
			ConnectionChange();
		}

		/// <summary>
		/// Recalculates output and signals output connections to update if the result is different from the cache.
		/// </summary>
		public void ConnectionChange() {
			Debug.Log("I have been told to update " + this.gameObject.transform.name);
			//get the new output
			DataSegment newResult = CalculateOutput();

			//Compare it to the cached DataSegment
			if (DataSegment.DeepComparison(cache, newResult) == false) {
				Debug.Log("I do need to update! " + this.gameObject.transform.name);
				//Update the result
				cache = newResult;

				//Signal all output connections that we have changed our data.
				for (int i = 0; i < output.Length; i++) {
					if (output[i].IsConnected())
						output[i].partner.owner.ConnectionChange();
				}
			}
			//Otherwise we dont have to do anything since nothing has changed
		}

		/// <summary>
		/// Returns the output of this data component.
		/// </summary>
		/// <returns></returns>
		public DataSegment GetOutput() {
			if (cache == null)
				return null;
			return cache.CreateDeepCopy();
		}

		/// <summary>
		/// REturns the data segements string with protection against it being null
		/// </summary>
		/// <returns></returns>
		public string GetOutputString() {
			if (cache == null)
				return "[N]<NULL>";
			else
				return cache.GetStringRepresentation();
		}

		/// <summary>
		/// Calculates the output of this data component
		/// </summary>
		/// <returns></returns>
		public abstract DataSegment CalculateOutput();

		public abstract string GetString();

	}	
}
