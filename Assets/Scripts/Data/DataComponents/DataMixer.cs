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
			if (this.GetInput().Length <= 0) {
				return null;
			} else if (this.GetInput().Length == 1) {
				return this.GetInput()[0].GetOutput();
			} else if (this.GetInput().Length >= 3) {
				throw new System.Exception("Mixer has a maximum of 3 inputs, " + this.GetInput().Length + " are connected.");
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
				sequence1 = this.GetInput()[0].GetOutput();
				sequence2 = this.GetInput()[1].GetOutput();
			} else {    // orderType == OrderType.In2_First
				sequence1 = this.GetInput()[1].GetOutput();
				sequence2 = this.GetInput()[0].GetOutput();
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

		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {

		}
	}
}
