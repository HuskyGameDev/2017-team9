using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AllTheColorsOfTheWind {
	/// <summary>
	/// A simple starting point for one color
	/// </summary>
	public class ColorSource : ColorComponent {

		#region Properties
		/// <summary>
		/// A color to use as our output
		/// </summary>
		[SerializeField]
		public Color32 sourceColor;
		#endregion

		#region Methods
		#region Public
		#region Override
		public override ColorBit CalculateOutput() {
			//Debug.Log(sourceColor + " | " + (new ColorBit(sourceColor)).ToString());
			return new ColorBit(sourceColor);
		}

		public override string GetString() {
			return "Source";
		}

		public override int InputCount() {
			return 0;
		}

		public override SocketCountType InputCountType() {
			return SocketCountType.None;
		}

		public override int OutputCount() {
			return 1;
		}

		public override SocketCountType OutputCountType() {
			return SocketCountType.AtLeast;
		}

		public override void Setup() {
			//None Needed
		}
		#endregion
		#endregion
		#endregion
	}
}