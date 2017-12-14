using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Namespace to hold all of our strictly puzzle related code
namespace Puzzle {
	//This class represents a connection for data
	//[System.Serializable]
	public abstract class DataBase : MonoBehaviour {

		public bool visual = true;
		public bool playerMovable = false;
		//The interal data that this connection contains
		//Its use will change based on the inherited class
		//This data DOES NOT hold the collection of the input/output
		//It is used to set a potential modification on data in the editor
		public Data internalData;

		//The storage of connections
		public List<DataBase> input = new List<DataBase>();
		public List<GameObject> beams = new List<GameObject>();

		public void Awake() {
			UpdateVisual();
		}

		public void AddInput(DataBase c) {
			input.Add(c);
			UpdateVisual();
		}
		public void RemoveInput(DataBase c) {
			input.Remove(c);
			UpdateVisual();
		}


		//Find the aggregate amount of data beams needed
		public int GetBeamCountFromInput() {
			int ret = 0;
			foreach (DataBase c in input) {
				ret += c.GetOutputData().bits.Length;
			}
			return ret;
		}

		public void Update() {
			UpdateVisual();
		}

		public void UpdateVisual() {
			int BeamCount = GetBeamCountFromInput();
			//Override the beam count to be 0 if we do not want to display a visual
			if (BeamCount > 0 && visual == false) BeamCount = 0;
			//We need to make sure we have the correct count of beams
			//Doing it this way limits the amount of creation and destruction of beams
			if (BeamCount > beams.Count) {
				int startingBeamCount = beams.Count;
				for (int i = 0; i < BeamCount - startingBeamCount; i++) {
					//Create new beam
					GameObject newBeam = Instantiate(Resources.Load("DataBeam", typeof(GameObject))) as GameObject;
					beams.Add(newBeam);
				}
			}
			else {
				//If they are equal this for loop is checked once and doesnt run
				for (int i = 0; i < beams.Count - BeamCount; i++) {
					//Remove a beam
					GameObject temp = beams[0];
					beams.RemoveAt(0);
					Destroy(temp);
				}
			}
			//We are done if we have no input
			if (input.Count == 0) return;


			//Otherwise update the beams
			//track the current beam we are working with
			int beamCount = 0; ;
			for (int i = 0; i < input.Count || BeamCount > beams.Count; i++) {
				Data OutputData = input[i].GetOutputData();

				//Calucate the postion our visual indicators need to be
				List<Vector3> positions = DataBeamVisual.CalculateDataBeamCluster(OutputData.bits.Length, this.transform.position, input[i].transform.position);
				//Caluclate the midpoint
				Vector3 midPoint = Vector3.Lerp(this.transform.position, input[i].transform.position, 0.5f);
				//This is used to track wich color we need to apply to the beam, this basically goes through the bits of the current input
				int color = 0;
				foreach (Vector3 p in positions) {
					//Get a beam from the pool and increase counter
					GameObject beam = beams[beamCount++];
					//Set its posoition
					beam.transform.position = p;
					//Calculate the size it needs to be, by finding the distance and dividing it by 2. We divide by two because the beam is centered
					float length = Vector3.Distance(input[i].transform.position, this.transform.position) / 2.0f;
					//Set the transform using beam radius and the calcualted length
					beam.transform.localScale = new Vector3(DataBeamVisual.BEAM_RADIUS * 2, length, DataBeamVisual.BEAM_RADIUS * 2);

					//Make it faces the proper direction, we make it look at the transform position with the beam offset
					beam.transform.LookAt(this.transform.position + (beam.transform.position - midPoint));
					beam.transform.rotation = beam.transform.rotation * Quaternion.Euler(0.0f, 90.0f, 90.0f);

					//Set the beam color by using the bit color saved in data
					beam.GetComponent<Renderer>().material.color = Data.DataColor[(int)OutputData.bits[color++]];

					beam.transform.parent = this.transform;
				}
			}
		}


		//Below are required implemented methods from our decendants

		//Calculates the desired output based on inputs and internal implementation of the decendant
		public abstract Data GetOutputData();

		//Settings essentially on whether or not they can send/recieve data
		public abstract bool CanSend();
		public abstract bool CanRecieve();
	}
}