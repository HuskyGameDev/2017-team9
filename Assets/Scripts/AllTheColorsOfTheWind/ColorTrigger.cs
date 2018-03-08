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
		#endregion

		#region Methods
		#region Public
		/// <summary>
		/// Checks if the input color and calls Trigger if they match
		/// </summary>
		/// <param name="color"></param>
		public void Check(ColorBit color) {
			Debug.Log("Check Called | " + color + " | " + triggerColor);
			if (triggerColor.Equals(color))
				Trigger();
		}
		#endregion

		#region Abstract
		/// <summary>
		/// Code for what happens when the colors match
		/// </summary>
		public abstract void Trigger();
		#endregion
		#endregion
	}
}