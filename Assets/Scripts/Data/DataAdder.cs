using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
	public class DataAdder : DataConnection {

		//We cannot send data so return null
		public override Data GetOutputData() {
			int[] counts = new int[Data.DataColor.Length];


			//Count the type of incomming bits
			foreach (DataConnection c in input) {
				foreach (Data.DataType d in c.GetOutputData().bits) {
					counts[(int)d]++;
				}
			}

			//Sum them up to higher bits
			{
				int t = 1;
				while (t < counts.Length - 1) {
					while (counts[t] > 1) {
						counts[t] -= 2;
						counts[t + 1] += 1;
					}
					t++;
				}
			}

			//Get the new total of bits
			int total = 0;
			for (int i = 0; i < counts.Length; i++) { total += counts[i]; }

			//Create the resulting bit arary
			Data.DataType[] returnBits = new Data.DataType[total];
			
			{
				int q = 0;
				int t = 0;
				while (t < counts.Length) {
					while (counts[t] > 0) {
						returnBits[q++] = (Data.DataType)t;
						counts[t]--;
					}
					t++;
				}
			}


			return new Data(returnBits);
		}

		//We can only recieve
		public override bool CanSend() { return true; }
		public override bool CanRecieve() { return true; }
	}
}
