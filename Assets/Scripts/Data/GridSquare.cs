using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleComponents;

[RequireComponent(typeof(GridSquareVisuals))]
public class GridSquare : MonoBehaviour {

	/// <summary>
	/// Shorthand refence to directions for array use
	/// </summary>
	public enum GridDirection { Up, Right, Down, Left }
	/// <summary>
	/// Shorthand array to access the opposite of a direction
	/// </summary>
	public static readonly GridDirection[] oppositeDirection = new GridDirection[] { GridDirection.Down, GridDirection.Left, GridDirection.Up, GridDirection.Right };
	/// <summary>
	/// Shorthand array to access the cross dirctions of a direction
	/// </summary>
	public static readonly GridDirection[][] crossDirections = new GridDirection[][] { new GridDirection[] { GridDirection.Left, GridDirection.Right}, new GridDirection[] { GridDirection.Down, GridDirection.Up }, new GridDirection[] { GridDirection.Left, GridDirection.Right }, new GridDirection[] { GridDirection.Down, GridDirection.Up } };

	/// <summary>
	/// Enum of all states a grid square can be
	/// </summary>
	public enum GridType { Empty, Unusable, Adder, Combiner, Connector, Deleter, Linker, Shifter, Source}

	public DataComponent dataComponent;

	/// <summary>
	/// The GridType this square is
	/// </summary>
	public GridType type;

	/// <summary>
	/// The states a socket can be in
	/// </summary>
	public enum SocketState { None, Input, Output}

	/// <summary>
	/// The states for each socket direction
	/// </summary>
	public SocketState[] socketState = new SocketState[4];
	/// <summary>
	/// The adjacent squares this square has. It should be auto-generated.
	/// </summary>
	public GridSquare[] neighbors = new GridSquare[4];

	/// <summary>
	/// The GridLine that is on this square.
	/// </summary>
	public GridLine[] line = new GridLine[4];


	/// <summary>
	/// Attempts to set the connection in the specified direction. Takes into account weather or not this is possible.
	/// </summary>
	/// <param name="direction"> The Direction from A to B</param>
	/// <returns>Returns true if the connection was sucessful</returns>
	public bool Connect(GridDirection direction, GridLine newLine) {

		if (type == GridType.Unusable) {
			//We can not use this grid square
			return false;
		}
		else if (type == GridType.Empty) {
			//We need to break cross connections and the opposite if it is not the same line
			//But how does this solve the 'loop-around' issue?
			//It is not a loop around if the other half off us never exits.
			//This doesnt account for turns tho

			//Check all other directions.
			//If there are no connections we can just assign and move on
			//If there is one, it has to to be a connection.
			//Else
			//We trim and then make the connection
			//Unless we are trimming the same line? then we need to reset the line back to that point.
			//The line will get reset back to that point on a trim, but how do we communicate that back to the caller?
			//In theory, the caller is attempting to operate on this square anyway, they will just be creating connections differently than intended.
			//These connections are made here in this method, so I think we can assume they will be handled correctly since we are the one doing the handling

			//First we need to see how many connections already exist
			int count = 0;
			GridLine found = null;
			for (int i = 0; i < line.Length; i++) {
				if (line[i] != null) {
					count++;
					found = line[i];
				}
			}

			//If there is one and it is the same line, that means we are just leaving the square
			if (count == 1 && found == newLine) {
				//So we can just assign the line and end

			}
			else {
				//Trim all other connections
				for (int i = 0; i < line.Length; i++) {
					if (line[i] != null)
						line[i].Trim(this);
				}
			}

			//Assign the connection and return
			line[(int)direction] = newLine;
		}
		else {
			//Trim this line back and add to this socket.
			if (line[(int)direction] != null) {
				line[(int)direction].Trim(this);
			}
			line[(int)direction] = newLine;
			line[(int)direction].AddDataComponent(dataComponent);
			
		}

		//Update the visuals and return sucess
		this.gameObject.GetComponent<GridSquareVisuals>().UpdateVisuals();
		return true;
	}


	/// <summary>
	/// Adds a line onto this square in the specified direction. Does not care about overriding
	/// </summary>
	/// <param name="add"></param>
	/// <param name="dir"></param>
	public void AddLine(GridLine add, GridDirection dir) {
		//the unusuable type cannot have any connections, ever
		if (type == GridType.Unusable) {
			return;
		}
		//Add the line
		line[(int)dir] = add;
	}

	/// <summary>
	/// Removes a line from our square.
	/// </summary>
	/// <param name="remove"></param>
	public void RemoveLine(GridLine remove) {
		for (int i = 0; i < line.Length; i++) {
			if (line[i] == remove) {
				if (type != GridType.Empty) {
					line[i].RemoveDataComponent(dataComponent);
				line[i] = null;
				}
			}
		}
	}



	/// <summary>
	/// Static method that checks if two Squares are neighbors
	/// </summary>
	/// <returns></returns>
	public static bool AreNeighbors(GridSquare A, GridSquare B, out GridDirection dir) {
		//For each possible direction
		for (int i = 0; i < A.neighbors.Length; i++) {
			//If we actually have a square in that direction
			if (A.neighbors[i] == null || B.neighbors[(int)oppositeDirection[i]] == null)
				continue;
			//check if they are in fact next too eachother (checks both ways for safety, this will hopefully bring more noticibilty to grid generation issues)
			if (A.neighbors[i] == B && B.neighbors[(int)oppositeDirection[i]] == A) {
				dir = (GridDirection)i;
				return true;
			}
		}
		//Else we return false, and the default value for a direction.
		dir = GridDirection.Up;
		return false;
	}


	/// <summary>
	/// Changes the component to a new type. This should be called automatically with an editor script.
	/// </summary>
	/// <param name="newType"></param>
	public void ChangeComponent(GridType newType) {
		Debug.Log("Attempting to ChangeComponent");
		//Clear all old components (should only be one but this is more safe)
		foreach (DataComponent dc in this.gameObject.GetComponents<DataComponent>()) {
			StartCoroutine(DestroyEndOfFrame(dc));
		}

		//Add the new type of component
		if (newType == GridType.Adder) {
			dataComponent = this.gameObject.AddComponent<DataAdder>();
		}
		else if (newType == GridType.Combiner) {
			dataComponent = this.gameObject.AddComponent<DataCombiner>();
		}
		else if (newType == GridType.Connector) {
			dataComponent = this.gameObject.AddComponent<DataConnector>();
		}
		else if (newType == GridType.Deleter) {
			dataComponent = this.gameObject.AddComponent<DataDeleter>();
		}
		else if (newType == GridType.Linker) {
			dataComponent = this.gameObject.AddComponent<DataLinker>();
		}
		else if (newType == GridType.Shifter) {
			dataComponent = this.gameObject.AddComponent<DataShifter>();
		}
		else if (newType == GridType.Source) {
			dataComponent = this.gameObject.AddComponent<DataSource>();
		}
		else if (newType == GridType.Unusable) {
			dataComponent = this.gameObject.AddComponent<DataUnusable>();
		}

		//Make sure the new component knows about us!
		if (dataComponent != null)
			dataComponent.attachedSquare = this;
	}

	/// <summary>
	/// Validates that component is the same type as the enum. True if they are the same.
	/// </summary>
	public bool ValidateTypeToComponentIntegrity() {
		bool consistent = false;

		if (type == GridType.Empty && dataComponent == null) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (dataComponent == null) {
			//From here, if we do not have a component attached we implicitly know we need to make a chanmge
			consistent = false;
		}
		else if (type == GridType.Adder && dataComponent.GetType() == typeof(DataAdder)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (type == GridType.Combiner && dataComponent.GetType() == typeof(DataCombiner)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (type == GridType.Connector && dataComponent.GetType() == typeof(DataConnector)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (type == GridType.Deleter && dataComponent.GetType() == typeof(DataDeleter)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (type == GridType.Linker && dataComponent.GetType() == typeof(DataLinker)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (type == GridType.Shifter && dataComponent.GetType() == typeof(DataShifter)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (type == GridType.Source && dataComponent.GetType() == typeof(DataSource)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}
		else if (type == GridType.Unusable && dataComponent.GetType() == typeof(DataUnusable)) {
			//In this case, we are now marked as empty but we have a DataComponent attacked, so we need to change
			consistent = true;
		}

		//Debug.Log("Consistent: " + (consistent));
		return consistent;
	}


	/// <summary>
	/// this is an internal MonoBehavoir method that is called when data is changed in the editor
	/// </summary>
	private void OnValidate() {
		//If our Type is different from our component, fix that
		if (ValidateTypeToComponentIntegrity() == false)
			ChangeComponent(type);

		//Update the visuals when we make a change
		this.gameObject.GetComponent<GridSquareVisuals>().UpdateVisuals();
	}



	IEnumerator DestroyEndOfFrame(MonoBehaviour go) {
		yield return new WaitForEndOfFrame();
		DestroyImmediate(go);
	}
}
