using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllTheColorsOfTheWind {
	/// <summary>
	/// Shifts RGB colors, so BRG is used as input to create a new color
	/// </summary>
	public class ColorShifter : ColorComponent {

		#region Methods
		#region Public
		#region Override
		public override ColorBit CalculateOutput() {
			ColorBit[] inputs = ValidateInput();
			//If we do not have any input, this means we are not setup properly yet
			if (inputs.Length == 0)
				return new ColorBit(null);

			//Shift the color values
			//R->B
			//G->R
			//B->G
			Color32 retColor = new Color32(inputs[0].color.b, inputs[0].color.r, inputs[0].color.g, inputs[0].color.a);

			return new ColorBit(retColor);
		}

		public override string GetString() {
			return "Shifter";
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
