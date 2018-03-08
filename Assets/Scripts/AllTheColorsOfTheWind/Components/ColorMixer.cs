using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AllTheColorsOfTheWind {
	/// <summary>
	/// Averages together all input colors
	/// </summary>
	public class ColorMixer : ColorComponent {

		#region Methods
		#region Public
		#region Override
		public override ColorBit CalculateOutput() {
			ColorBit[] inputs = ValidateInput();
			//If we do not have any input, this means we are not setup properly yet
			if (inputs.Length == 0)
				return new ColorBit(null);

			//Store the RGB values
			int r = 0;
			int g = 0;
			int b = 0;
			int a = 0;

			for (int i = 0; i < inputs.Length; i++) {
				r += inputs[i].color.r;
				g += inputs[i].color.g;
				b += inputs[i].color.b;
				a += inputs[i].color.a;
			}

			//Average them
			r /= inputs.Length;
			g /= inputs.Length;
			b /= inputs.Length;
			a /= inputs.Length;
			r = Mathf.Min(255,Mathf.RoundToInt(r));
			g = Mathf.Min(255, Mathf.RoundToInt(g));
			b = Mathf.Min(255, Mathf.RoundToInt(b));
			a = Mathf.Min(255, Mathf.RoundToInt(a));



			return new ColorBit(new Color32((byte)r, (byte)g, (byte)b, (byte)a)); ;
		}

		public override string GetString() {
			return "Mixer";
		}

		public override int InputCount() {
			return 1;
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
