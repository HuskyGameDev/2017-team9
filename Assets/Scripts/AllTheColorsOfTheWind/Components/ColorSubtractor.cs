using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllTheColorsOfTheWind {
	/// <summary>
	/// Subtracts a flat color value from the input
	/// </summary>
	public class ColorSubtractor : ColorComponent {

		#region Properties
		#region Public
		/// <summary>
		/// The color to subtract from our input
		/// </summary>
		public Color32 subtractionValue;
		#endregion
		#endregion

		#region Methods
		#region Public
		#region Override
		public override ColorBit CalculateOutput() {
			ColorBit[] inputs = ValidateInput();
			//If we do not have any input, this means we are not setup properly yet
			if (inputs.Length == 0)
				return new ColorBit(null);

			/*	// subtract input from value
			byte r = (byte)Mathf.Max(subtractionValue.r - inputs[0].color.r, 0);
			byte g = (byte)Mathf.Max(subtractionValue.g - inputs[0].color.b, 0);
			byte b = (byte)Mathf.Max(subtractionValue.b - inputs[0].color.b, 0);
			*/

			int inR = inputs[0].color.r; // this makes it work correctly for some reason
			int inG = inputs[0].color.g;
			int inB = inputs[0].color.b;

			// subtract value from input
			byte r = (byte)Mathf.Max(inR - subtractionValue.r, 0);
			byte g = (byte)Mathf.Max(inG - subtractionValue.g, 0);
			byte b = (byte)Mathf.Max(inB - subtractionValue.b, 0);
			


			return new ColorBit(new Color32(r, g, b, inputs[0].color.a));
		}

		public override string GetString() {
			return "Subtractor";
		}

		public override int InputCount() {
			return 1;
		}

		public override SocketCountType InputCountType() {
			return SocketCountType.Exact;
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
