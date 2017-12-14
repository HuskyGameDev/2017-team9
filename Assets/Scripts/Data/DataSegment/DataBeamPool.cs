using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PuzzleComponents {
	/// <summary>
	/// A singleton pool for data beam visuals.
	/// </summary>
	public class DataBeamPool : MonoBehaviour {

		/// <summary>
		/// The satic reference to the signleton instance of this class
		/// </summary>
		public static DataBeamPool instance;

		/// <summary>
		/// A list of all beams in the pool. It is assumed unused beams are grouped at the end of the list.
		/// </summary>
		public List<GameObject> beams;

		/// <summary>
		/// The radius of a data beam
		/// </summary>
		public static float BEAM_RADIUS = 0.05f;

		/// <summary>
		/// The count of used beams
		/// </summary>
		int used;

		void Awake() {
			if (instance == null) {
				instance = this;
				beams = new List<GameObject>();
				used = 0;
			}
			else {
				Debug.LogError("You have multiple data beam pools!");
				this.gameObject.SetActive(false);
			}
		}

		public static GameObject AcquireDataBeam() {
			Debug.Log("Acquiring.");
			if (instance.used >= instance.beams.Count) {
				//Load a new beam
				GameObject newBeam = Instantiate(Resources.Load("DataBeam", typeof(GameObject))) as GameObject;
				// Mark it as used and add it to the list.
				instance.used++;
				instance.beams.Add(newBeam);
				return newBeam;
			}
			else {
				//Enable the beam so it can be seen.
				instance.beams[instance.used].SetActive(true);
				//Return an unused beam and increase the used count.
				return instance.beams[instance.used++];
			}
		}

		public static void ReturnDataBeam(GameObject beam) {
			//Cannot return a nonexistant beam
			if (beam == null)
				return;
			beam.SetActive(false);
			//Shuffle this beam to the end of the list
			instance.beams.Remove(beam);
			instance.beams.Add(beam);

			//Disable the beam so it can no longer be seen.

			//Mark is as unused
			instance.used--;
		}
	}
}