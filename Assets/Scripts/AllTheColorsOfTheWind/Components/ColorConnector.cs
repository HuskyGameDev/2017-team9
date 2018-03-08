using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllTheColorsOfTheWind {
	/// <summary>
	/// Used as the end point of a puzzle to connect to Triggers.
	/// </summary>
	public class ColorConnector : ColorComponent {

		#region Methods
		#region Public
		#region Override
		public override ColorBit CalculateOutput() {
			ColorBit[] inputs = ValidateInput();
			//If we do not have any input, this means we are not setup properly yet
			if (inputs.Length == 0)
				return new ColorBit(null);

			return inputs[0];
		}

		public override string GetString() {
			return "Connector";
		}

		public override int InputCount() {
			return 1;
		}

		public override SocketCountType InputCountType() {
			return SocketCountType.Exact;
		}

		public override int OutputCount() {
			return 0;
		}

		public override SocketCountType OutputCountType() {
			return SocketCountType.None;
		}

		public override void Setup() {
			//None Needed
		}
		#endregion
		#endregion
		#endregion
	}
}
