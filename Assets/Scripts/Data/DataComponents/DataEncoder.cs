﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleComponents {
	public class DataEncoder : DataComponent {

		public enum EncodeType { X_to_Y, X_to_Z };
		public EncodeType encodeType;

		public override DataSequence CalculateOutput() {
			//DataComponent[] inputComponents = GetInput();
			if (this.GetInput().Length <= 0) {
				return null;
			}

			DataSequence dataInput = this.GetInput()[0].GetOutput();
			DataSequence output = new DataSequence(new DataSegment[] { });
			//dataInput.Fracture();

			for (int i = 0; i < dataInput.segments.Length; i++) {
				DataSegment inputSegment = dataInput.segments.Get(i);
				DataSegment outputSegment = new DataSegment(new Bit[] { });
				if (encodeType == EncodeType.X_to_Y) {
					for (int k = 0; k < inputSegment.GetBitCount(); k ++) {
						if (inputSegment.bits.Get(k).state == Bit.State.X) {
							outputSegment.bits.AddElementAtEnd(new Bit(Bit.State.Y));
						} else if (inputSegment.bits.Get(k).state == Bit.State.Y) {
							outputSegment.bits.AddElementAtEnd(new Bit(Bit.State.Z));
						} else {	// state == Bit.State.Z
							outputSegment.bits.AddElementAtEnd(new Bit(Bit.State.X));
						}
					}
				} else {    // encodeType is X_to_Z
					for (int k = 0; k < inputSegment.GetBitCount(); k++) {
						if (inputSegment.bits.Get(k).state == Bit.State.X) {
							outputSegment.bits.AddElementAtEnd(new Bit(Bit.State.Z));
						} else if (inputSegment.bits.Get(k).state == Bit.State.Y) {
							outputSegment.bits.AddElementAtEnd(new Bit(Bit.State.X));
						} else {    // state == Bit.State.Z
							outputSegment.bits.AddElementAtEnd(new Bit(Bit.State.Y));
						}
					}
				}
				if (inputSegment.linked == true) {
					outputSegment.linked = true;
				}
				output.segments.AddElementAtEnd(outputSegment);
			}
			return output;
		}

		public override string GetString() {
			return "Combiner";
		}

		public override void Setup() {

		}

		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {

		}
	}
}