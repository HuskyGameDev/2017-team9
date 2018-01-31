using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleComponents;

public class GridSquare : MonoBehaviour {

	/// <summary>
	/// Shorthand refence to directions for array use
	/// </summary>
	public enum GridDirection { Up, Down, Left, Right}
	/// <summary>
	/// Shorthand array to access the opposite of a direction
	/// </summary>
	public static readonly GridDirection[] oppositeDirection = new GridDirection[] { GridDirection.Down, GridDirection.Up, GridDirection.Right, GridDirection.Left};
	/// <summary>
	/// Shorthand array to access the cross dirctions of a direction
	/// </summary>
	public static readonly GridDirection[][] crossDirections = new GridDirection[][] { new GridDirection[] { GridDirection.Left, GridDirection.Right}, new GridDirection[] { GridDirection.Left, GridDirection.Right }, new GridDirection[] { GridDirection.Down, GridDirection.Up }, new GridDirection[] { GridDirection.Down, GridDirection.Up } };

	/// <summary>
	/// Enum of all states a grid square can be
	/// </summary>
	public enum GridType { Empty, Unusable, Adder, Combiner, Connector, Deleter, Linker, Shifter, Source}

	public DataComponent component;

	/// <summary>
	/// The GridType this square is
	/// </summary>
	public GridType type;

	/// <summary>
	/// The states a socket can be in
	/// </summary>
	public enum SocketState { None, Input, Output, Omni}

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
					line[i].Trim(this);
				}
			}

			//Assign the connection and return
			line[(int)direction] = newLine;
			return true;
		}
		else {
			//Trim this line back and add to this socket.
			if (line[(int)direction] != null) {
				line[(int)direction].Trim(this);
			}
			line[(int)direction] = newLine;
			line[(int)direction].AddDataComponent(component);
			return true;
		}
	}

	/// <summary>
	/// Changes the component to a new type. This should be called automatically with an editor script.
	/// </summary>
	/// <param name="newType"></param>
	public void ChangeComponent(GridType newType) {
		//Clear all components
		//Add the proper one.

		foreach (DataComponent dc in this.gameObject.GetComponents<DataComponent>()) {
			DestroyImmediate(dc);
		}
		
		if (newType == GridType.Adder) {
			component = this.gameObject.AddComponent<DataAdder>();
		}
		else if (newType == GridType.Combiner) {
			component = this.gameObject.AddComponent<DataCombiner>();
		}
		else if (newType == GridType.Connector) {
			component = this.gameObject.AddComponent<DataConnector>();
		}
		else if (newType == GridType.Deleter) {
			component = this.gameObject.AddComponent<DataDeleter>();
		}
		else if (newType == GridType.Linker) {
			component = this.gameObject.AddComponent<DataLinker>();
		}
		else if (newType == GridType.Shifter) {
			component = this.gameObject.AddComponent<DataShifter>();
		}
		else if (newType == GridType.Source) {
			component = this.gameObject.AddComponent<DataSource>();
		}
	}


	public void AddLine(GridLine add, GridDirection dir) {
		if (type == GridType.Unusable) {
			return;
		}
		line[(int)dir] = add;
	}

	/// <summary>
	/// Removes a line from our square.
	/// </summary>
	/// <param name="remove"></param>
	public void RemoveLine(GridLine remove) {
		for (int i = 0; i < line.Length; i++) {
			if (line[i] == remove) {
				line[i] = null;
				if (type != GridType.Empty) {
					line[i].RemoveDataComponent(component);
				}
			}
		}
	}



	/// <summary>
	/// Static method that checks if two Squares are neighbors
	/// </summary>
	/// <returns></returns>
	public static bool AreNeighbors(GridSquare A, GridSquare B) {
		for (int i = 0; i < A.neighbors.Length; i++) {
			if (A.neighbors[i] == null || B.neighbors[i] == null)
				continue;
			if (A.neighbors[i] == B && B.neighbors[(int)oppositeDirection[i]] == A)
				return true;
		}
		return false;
	}

}
