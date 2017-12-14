using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Namespace to hold all of our strictly puzzle related code
namespace Puzzle {

	//A class for a data reciever that cannot send anything out
	//Essentially, an end point
	public class DataReceiver : DataBase {

		//We cannot send data so return null
		public override Data GetOutputData() {
			return null;
		}

		//We can only recieve
		public override bool CanSend() { return false; }
		public override bool CanRecieve() { return true; }
	}
}