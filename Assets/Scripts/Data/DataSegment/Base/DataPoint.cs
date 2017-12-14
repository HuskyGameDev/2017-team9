using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PuzzleComponents {
	/// <summary>
	/// A class that represents a connection between two data components
	/// </summary>
	public class DataPoint : MonoBehaviour {


		/// <summary>
		/// The data point this data point is connected to. Null means no connection
		/// </summary>
		public DataPoint partner = null;

		/// <summary>
		/// The data component that uses/owns this data point.
		/// </summary>
		public DataComponent owner;

		/// <summary>
		/// Create a connection between this data point and another. Cannot connect to self. Will remove old connections
		/// </summary>
		/// <param name="other"></param>
		public void CreateConnection(DataPoint other) {
			//Do not connect to ourself
			if (other == this) return;

			//Remove our old partner
			if (partner != null) {
				DisconnectPartner();
			}

			//Establish connection
			ChangePartner(other);
			other.ChangePartner(this);
		}


		/// <summary>
		/// Remove the current connection between these two data points. Returns the removed partner point.
		/// </summary>
		/// <returns></returns>
		public DataPoint DisconnectPartner() {
			//Store the partner to return later
			DataPoint t = partner;

			//Set our partners connection to null
			this.partner.partner = null;

			//Remove our partner
			this.partner = null;

			return t;
		}

		/// <summary>
		/// Change the partner to a new DataPoint. Calls ConnectionChange on the owner.
		/// </summary>
		/// <param name="other"></param>
		public void ChangePartner(DataPoint other) {
			//Do nothing if we already have this partner.
			if (other == partner)
				return;

			this.owner.ConnectionChange();
			this.partner = other;
		}

		public bool IsConnected() {
			return partner != null;
		}


	}
}
