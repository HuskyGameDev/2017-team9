using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllTheColorsOfTheWind {
	public abstract class ColorTrigger : MonoBehaviour {
		#region Properties
		/// <summary>
		/// The color on which the trigger method is called.
		/// </summary>
		public ColorBit triggerColor;
		public bool triggered;
		#endregion

		#region Methods
		#region Public
		/// <summary>
		/// Checks if the input color and calls Trigger if they match
		/// </summary>
		/// <param name="color"></param>
		public bool Check(ColorBit color) {
			Debug.Log("Check Called | " + color + " | " + triggerColor);
			if (triggerColor.Equals(color)) {
				Trigger();
				triggered = true;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Untriggers script if it has not already been untriggered. 
		/// Returns true if the script was not already untriggered
		/// </summary>
		/// <returns>true if the script was not already untriggered</returns>
		public bool CanUntrigger() {
			Debug.Log("CanUnTrigger Called: " + triggered);
			if (triggered) {
				Untrigger();
				triggered = false;
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Triggers script if it has not already been triggered. 
		/// Returns true if the script was not already triggered
		/// </summary>
		/// <returns>true if the script was not already triggered</returns>
		public bool CanTrigger() {
			Debug.Log("CanTrigger Called: " + !triggered);
			if (triggered) {
				return false;
			} else {
				Trigger();
				triggered = true;
				return true;
				
			}
		}
		#endregion

		#region Abstract
		/// <summary>
		/// Code for what happens when the colors match.
		/// If calling directly, must also set bool triggered to true.
		/// </summary>
		public abstract void Trigger();

		/// <summary>
		/// Undoes whatever Trigger does, if applicable.
		/// If calling directly, must also set bool triggered to false.
		/// </summary>
		public abstract void Untrigger();
		#endregion
		#endregion
	}
}