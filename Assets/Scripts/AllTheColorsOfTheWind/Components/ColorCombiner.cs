using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllTheColorsOfTheWind {
	/// <summary>
	/// Adds together all input colors
	/// </summary>
	public class ColorCombiner : ColorComponent {
		#region Methods
		#region Public
		#region Override
		public override ColorBit CalculateOutput() {
			ColorBit[] inputs = ValidateInput();
			//If we do not have any input, this means we are not setup properly yet
			if (inputs.Length == 0)
				return new ColorBit(null);

			/*
			//Store the first one.
			byte r = 0;
			byte g = 0;
			byte b = 0;
				

			//And add the rest to it
			for (int i = 1; i < inputs.Length; i++) {
				r += inputs[i].color.r;
				g += inputs[i].color.g;
				b += inputs[i].color.b;

			}
			*/

			//Store the first one.
			byte r = inputs[0].color.r;
			byte g = inputs[0].color.g;
			byte b = inputs[0].color.b;


			//And add the rest to it
			for (int i = 1; i < inputs.Length; i++) {
				r += inputs[i].color.r;
				g += inputs[i].color.g;
				b += inputs[i].color.b;

			}

			return new ColorBit(new Color32(r,g,b, inputs[0].color.a));
		}

		public override string GetString() {
			return "Subtractor";
		}

		public override int InputCount() {
			return 2;
		}

		public override SocketCountType InputCountType() {
			return SocketCountType.AtLeast;
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
