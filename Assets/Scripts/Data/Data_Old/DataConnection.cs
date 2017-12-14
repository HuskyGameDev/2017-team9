using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Namespace to hold all of our strictly puzzle related code
namespace Puzzle {
	//This class represents a connection for data
	//[System.Serializable]
	public class DataConnection : DataBase {

		//Calculates the desired output based on inputs and internal implementation of the decendant
		public override Data GetOutputData() {
			List<Data.DataType> list = new List<Data.DataType>();

			foreach (DataBase c in input) {
				foreach (Data.DataType bit in c.GetOutputData().bits) {
					list.Add(bit);
				}
			}

			return new Data(list.ToArray());
		}

		//Settings essentially on whether or not they can send/recieve data
		public override bool CanSend() { return true; }
		public override bool CanRecieve() { return true;  }
	}
}