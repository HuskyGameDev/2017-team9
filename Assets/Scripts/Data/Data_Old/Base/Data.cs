using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Namespace to hold all of our strictly puzzle related code
namespace Puzzle {


	// The data class is the abstract representation of 'data' needed to operate things in our game.
	[System.Serializable]
	public class Data {
		//These three lines are for represnetation info on the Data. This will likely change over time.
		public enum DataType { N = 0, A = 1, B = 2, C = 3 }
		public static string[] DataText = new string[] { "0x00", "0xeA", "0xeB", "0xeC" };
		public static Color[] DataColor = new Color[] { Color.clear, Color.blue, Color.green, Color.red };
		
		//The types of bits this data has
		public DataType[] bits = new DataType[3];

		public Data(DataType[] bits) {
			this.bits = bits;
		}

		//true if a is bigger, false if a is smaller, null if they are the same
		// N is the null type of data, it is never larger. We have two special conditions for this
		public static bool? CompareBit(DataType a, DataType b) {
			if (a == DataType.N) {
				return false;
			}
			if (b == DataType.N) {
				return false;
			}

			return ((int)a == (int)b) ? ((bool?)null) : ((int)a > (int)b);
		}
	}
}
