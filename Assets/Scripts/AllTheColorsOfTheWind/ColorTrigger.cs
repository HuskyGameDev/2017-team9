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
		protected bool triggered = false;
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
				//triggered = true;
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
			//Debug.Log("CanUnTrigger Called: " + triggered);
			if (triggered) {
				Untrigger();
				//triggered = false;
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
			//Debug.Log("CanTrigger Called: " + !triggered);
			if (triggered) {
				return false;
			} else {
				Trigger();
				//triggered = true;
				return true;
				
			}
		}

		/// <summary>
		/// Tests if internal variable triggered is true
		/// </summary>
		/// <returns>true if triggered is true, else false</returns>
		public bool GetTriggered() {
			return triggered;
		}

		/// <summary>
		/// Sets internal variable triggered to given boolean
		/// </summary>
		/// <returns>true if triggered is true, else false</returns>
		public void SetTriggered(bool b) {
			triggered = b;
		}
		#endregion

		#region Abstract
		/// <summary>
		/// Code for what happens when the colors match.
		/// </summary>
		public abstract void Trigger();

		/// <summary>
		/// Undoes whatever Trigger does, if applicable.
		/// </summary>
		public abstract void Untrigger();
		#endregion
		#endregion
	}
}