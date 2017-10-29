using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
	[System.Serializable]
	public class Data {
		public enum DataType { N = 0, A = 1, B = 2, C = 3 }
		public string[] DataText = new string[] { "0x00", "0xeA", "0xeB", "0xeC" };
		public Color[] DataColor = new Color[] { Color.clear, Color.blue, Color.green, Color.red };
		
		public DataType[] bits;

		public Data(DataType[] bits) {
			this.bits = bits;
		}
	}
}
