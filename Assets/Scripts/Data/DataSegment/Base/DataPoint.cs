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

		public GameObject beam;

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

			this.beam = DataBeamPool.AcquireDataBeam();
			this.partner.beam = this.beam;

			UpdateVisual();
		}


		/// <summary>
		/// Remove the current connection between these two data points. Returns the removed partner point.
		/// </summary>
		/// <returns></returns>
		public DataPoint DisconnectPartner() {
			//Store the partner to return later
			DataPoint t = partner;

			//Set our partners connection to null
			if (this.partner != null)
				this.partner.partner = null;

			//Remove our partner
			this.partner = null;

			DataBeamPool.ReturnDataBeam(beam);

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

			this.partner = other;
			this.owner.ConnectionChange();
		}

		/// <summary>
		/// Checks if a partner exists.
		/// </summary>
		/// <returns></returns>
		public bool IsConnected() {
			return partner != null;
		}

		/// <summary>
		/// Updates the beam visual
		/// </summary>
		public void UpdateVisual() {
			//We cannot draw a visual unless there is a connection.
			if (IsConnected() == false)
				return;

			//Get a beam from the pool. this is done here for the case when connections are made in the editor. It is normally handled on connection creation
			if (beam == null) {
				this.beam = DataBeamPool.AcquireDataBeam();
				this.partner.beam = this.beam;
			}

			Debug.DrawLine(this.transform.position, this.partner.transform.position);

			//Calculate the midpoint between our two points
			Vector3 midPoint = Vector3.Lerp(this.transform.position, this.partner.transform.position, 0.5f);

			//Set its posoition
			beam.transform.position = midPoint;
			//Calculate the size it needs to be, by finding the distance and dividing it by 2. We divide by two because the beam is centered
			float length = Vector3.Distance(this.partner.transform.position, this.transform.position) / 2.0f;
			//Set the transform using beam radius and the calcualted length
			beam.transform.localScale = new Vector3(DataBeamPool.BEAM_RADIUS * 2, length, DataBeamPool.BEAM_RADIUS * 2);

			//Make it faces the proper direction, we make it look at the transform position with the beam offset
			beam.transform.LookAt(this.transform.position + (beam.transform.position - midPoint));
			//the desired rotation multiplied by an offset to get the model lined up.
			beam.transform.rotation = beam.transform.rotation * Quaternion.Euler(0.0f, 90.0f, 90.0f);
		}


		/// <summary>
		/// The default unity update method
		/// </summary>
		public void Update() {
			UpdateVisual();
		}

	}
}
