﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllTheColorsOfTheWind {
	/// <summary>
	/// Adds a flat color value to input
	/// </summary>
	public class ColorAdder : ColorComponent {

		#region Properties
		#region Public
		/// <summary>
		/// The color to subtract from our input
		/// </summary>
		public Color32 additionValue;
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
			/*
			byte r = (byte)Mathf.Min(additionValue.r + inputs[0].color.r, 255);	// this doesn't work correctly for some reason
			byte g = (byte)Mathf.Min(additionValue.g + inputs[0].color.b, 255);
			byte b = (byte)Mathf.Min(additionValue.b + inputs[0].color.b, 255);
			*/
			
			int addR = additionValue.r;	// this makes it work correctly for some reason
			int addG = additionValue.g;
			int addB = additionValue.b;

			byte r = (byte)Mathf.Min(addR + inputs[0].color.r, 255);
			byte g = (byte)Mathf.Min(addG + inputs[0].color.g, 255);
			byte b = (byte)Mathf.Min(addB + inputs[0].color.b, 255);

			ColorBit newOutput = new ColorBit(new Color32(r, g, b, inputs[0].color.a));
			//Debug.LogWarning("Adder outputting " + newOutput + " | " + additionValue + " | " + inputs[0].ToString());
			return newOutput;
		}

		public override string GetString() {
			return "Adder";
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
