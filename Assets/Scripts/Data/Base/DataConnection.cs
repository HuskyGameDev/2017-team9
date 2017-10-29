using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
	//This class represents a connection for data, this class can also be used as an end reciever for data
	[System.Serializable]
	public abstract class DataConnection {

		public const int CONNECTIONLIMIT = 3;
		public Data data;
		private DataConnection[] input = new DataConnection[CONNECTIONLIMIT];
		private DataConnection[] output = new DataConnection[CONNECTIONLIMIT];
		private int inputCount = 0;
		private int outputCount = 0;

		//Returns true if add was sucessful
		public bool AddInput(DataConnection source) {
			if (inputCount >= CONNECTIONLIMIT) {
				return false;
			}
			else {
				input[inputCount++] = source;
				return true;
			}
		}
		public bool AddOutput(DataConnection source) {
			if (outputCount >= CONNECTIONLIMIT) {
				return false;
			}
			else {
				output[outputCount++] = source;
				return true;
			}
		}

		public bool RemoveInput(DataConnection source) {
			for (int i = 0; i < CONNECTIONLIMIT; i++) {
				if (source == input[i]) {
					for (int k = i; k < CONNECTIONLIMIT -1; k++) {
						input[k] = input[k + 1];
					}
					return true;
				}
			}
			return false;
		}

		public bool RemoveOutput(DataConnection source) {
			for (int i = 0; i < CONNECTIONLIMIT; i++) {
				if (source == output[i]) {
					for (int k = i; k < CONNECTIONLIMIT - 1; k++) {
						output[k] = output[k + 1];
					}
					return true;
				}
			}
			return false;
		}

		public abstract Data GetOutputData();
	}
}