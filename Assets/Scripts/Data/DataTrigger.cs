using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Namespace to hold all of our strictly puzzle related code
namespace Puzzle {
	public class DataTrigger : MonoBehaviour {
		//the dataconnection to watch
		public DataConnection obvserve;
		public Data triggerState;


		public UnityEvent toTrigger;
		//public delegate void TriggerDelegate(Data oldData, Data currentData);
		//public TriggerDelegate Triggers;


		public void OnDataChange(Data newData) {
			//if (Triggers != null)
			//	Triggers(oldData, triggerState);
				toTrigger.Invoke();

		}
	}


}
