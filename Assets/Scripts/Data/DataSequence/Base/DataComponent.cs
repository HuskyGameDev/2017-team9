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

			//Make sure we own all of our datapoints
			foreach (DataPoint p in input) {
				p.state = DataPoint.State.Input;
				p.owner = this;
				//This means we have a partner predefined by us, either by weird spawning or manual connection in the editor
				if (p.partner != null) {
					//So we need to store the partner temporarily and call CreateConnection with it. The partner has to be null so CreateConnection will execute properly 
					DataPoint temp = p.partner;
					p.partner = null;
					p.CreateConnection(temp);
				}

			}
			foreach (DataPoint p in output) {
				p.state = DataPoint.State.Output;
				p.owner = this;
				//This means we have a partner predefined by us, either by weird spawning or manual connection in the editor
				if (p.partner != null) {
					//So we need to store the partner temporarily and call CreateConnection with it. The partner has to be null so CreateConnection will execute properly 
					DataPoint temp = p.partner;
					p.partner = null;
					p.CreateConnection(temp);
				}

			}


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
		public DataSequence GetOutput() {
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
