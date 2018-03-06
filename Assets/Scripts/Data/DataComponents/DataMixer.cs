using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleComponents {
	public class DataMixer : DataComponent {

		public enum MixerType { Half_n_Half, All };
		public enum OrderType { In1_First, In2_First };
		public MixerType mixerType;
		public OrderType orderType;

		public override DataSequence CalculateOutput() {

			int inputCount = 0;
			int foundInput1 = -1;
			int foundInput2 = -1;
			for (int i = 0; i < inputs.Length; i++) {
				if (inputs[i] != null) {
					if (foundInput1 == -1) {
						foundInput1 = i;
					}
					else if (foundInput2 == -1) {
						foundInput2 = i;
					}
					inputCount++;
				}
			}
			
			if (inputCount == 0)
				return null;






			if (inputCount <= 0) {
				return null;
			} else if (inputCount == 1) {
				return inputs[foundInput1];
			} else if (inputCount >= 3) {
				throw new System.Exception("Mixer has a maximum of 2 inputs, " + inputs.Length + " are connected.");
			}

			DataSequence sequence1;
			DataSequence sequence2;
			
			int seq1Place;
			int seq2Place;
			int advanceRate;

			if (mixerType == MixerType.Half_n_Half) {
				seq1Place = 0;
				seq2Place = 1;
				advanceRate = 2;
			} else {    // mixerType == MixerType.All
				seq1Place = 0;
				seq2Place = 0;
				advanceRate = 1;
			}
			if (orderType == OrderType.In1_First) {
				sequence1 = inputs[foundInput1];
				sequence2 = inputs[foundInput2];
			} else {    // orderType == OrderType.In2_First
				sequence1 = inputs[foundInput2];
				sequence2 = inputs[foundInput1];
			}

			sequence1.Fracture();
			sequence2.Fracture();

			DataSequence outputSeq = new DataSequence(new DataSegment[] { });

			while ((seq1Place < sequence1.segments.Length) && (seq2Place < sequence2.segments.Length)) {
				outputSeq.segments.AddElementAtEnd(sequence1.segments.Get(seq1Place));
				outputSeq.segments.AddElementAtEnd(sequence2.segments.Get(seq2Place));
				seq1Place += advanceRate;
				seq2Place += advanceRate;
			}
			while (seq1Place < sequence1.segments.Length) {
				outputSeq.segments.AddElementAtEnd(sequence1.segments.Get(seq1Place));
				seq1Place += advanceRate;
			}
			while (seq2Place < sequence2.segments.Length) {
				outputSeq.segments.AddElementAtEnd(sequence2.segments.Get(seq2Place));
				seq2Place += advanceRate;
			}
			return outputSeq;
		}

		public override string GetString() {
			return "Mixer";
		}

		public override void Setup() {
		}
	}
}
