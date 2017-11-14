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
	}
}
