using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Namespace to hold all of our strictly puzzle related code
namespace Puzzle {
	
	//A class for something that only supplys data
	public class DataSource : DataConnection {

		//The data we are sending is set in the editor, and is an inherited member
		public override Data GetOutputData() {
			return this.internalData;
		}


		//We can only send
		public override bool CanSend() { return true; }
		public override bool CanRecieve() { return false; }
	}
}
