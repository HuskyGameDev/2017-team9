using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Puzzle {
	public class DataRemover : DataBase {

		public enum RemovalMode { LeftmostCut, RightmostCut, Highest, Lowest, Range, Exact }
		public RemovalMode mode;


		//public bool UseRange;

		public Data.DataType exact;
		public Data.DataType lowerBound;
		public Data.DataType upperBound;
		public int lowerRange;
		public int upperRange;


		public override bool CanRecieve() {
			return true;
		}

		public override bool CanSend() {
			return true;
		}

		public override Data GetOutputData() {

			//Gather all the input bits

			List<Data.DataType> list = new List<Data.DataType>();

			//Keep track of these so we do not have to loop through data again depending on our chosen mode
			Data.DataType smallest = Data.DataType.N;
			Data.DataType largest = Data.DataType.N;
			int count = 0;

			foreach (DataBase c in input) {
				foreach (Data.DataType bit in c.GetOutputData().bits) {
					list.Add(bit);
					count++;
					if (Data.CompareBit(smallest, bit) == true)  smallest = bit;
					if (Data.CompareBit(bit, largest)  == true)  largest  = bit;
				}
			}

			//Now we will go an execute code for the different removal modes. Some of them will modify list and some will create a new bit to return themselves
			if (mode == RemovalMode.LeftmostCut) {
				//This is set to cut off the whole thing
				if (count - lowerRange <= 0)
					return new Data(new Data.DataType[0]);
				
				//Create an array to hold the right size of bits
				Data.DataType[] bits = new Data.DataType[count - lowerRange];

				for (int i = lowerRange; i < count; i++) {
					bits[i] = list[i];
				}

				return new Data(bits);
			}
			else if (mode == RemovalMode.RightmostCut) {
				//This is set to cut off the whole thing
				if (count - upperRange <= 0)
					return new Data(new Data.DataType[0]);

				//Create an array to hold the right size of bits
				Data.DataType[] bits = new Data.DataType[count - upperRange];

				for (int i = 0; i < upperRange; i++) {
					bits[i] = list[i];
				}

				return new Data(bits);
			}
			else if (mode == RemovalMode.Highest) {
				//Find the highest bits and remove them
				for (int i = count - 1; i >= 0; i--) {
					Data.DataType bit = list[i];
					if (Data.CompareBit(bit,largest) == null) {
						list.Remove(bit);
					}
				}
			}
			else if (mode == RemovalMode.Lowest) {
				//Find the highest bits and remove them
				for (int i = count - 1; i >= 0; i--) {
					Data.DataType bit = list[i];
					if (Data.CompareBit(bit, smallest) == null) {
						list.Remove(bit);
					}
				}
			}
			else if (mode == RemovalMode.Range) {
				//Remove all bits within the specified range
				for (int i = count - 1; i >= 0; i--) {
					Data.DataType bit = list[i];
					if (Data.CompareBit(lowerBound, bit) == true || Data.CompareBit(bit, upperBound) == true) {
						list.Remove(bit);
					}
				}
			}
			else if (mode == RemovalMode.Exact) {
				//Remove all bits within the specified range
				for(int i = count - 1; i >= 0; i--) {
					Data.DataType bit = list[i];
					if (Data.CompareBit(exact, bit) == null) {
						list.Remove(bit);
					}
				}
			}


			return new Data(list.ToArray());
		}


	}
}
