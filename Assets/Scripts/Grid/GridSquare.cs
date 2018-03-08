using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllTheColorsOfTheWind;

[RequireComponent(typeof(GridSpriteVisuals))]
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
	public enum GridType { Empty, Source, Connector, Adder, Subtractor, Mixer, Combiner, Shifter}
	public static readonly string[] typeToString = new string[] { "Empty", "Source", "Connector", "Adder", "Subtractor", "Mixer", "Combiner", "Shifter" };

	/// <summary>
	/// The script that handles the visuals
	/// </summary>
	public GridSpriteVisuals sprites;

	/// <summary>
	/// The dataComponent that may exist on this square.
	/// </summary>
	public ColorComponent component;

	/// <summary>
	/// The GridType this square is
	/// </summary>
	public GridType type;

	/// <summary>
	/// The states a socket can be in
	/// </summary>
	public enum SocketState { None, Line, Input, Output}

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
	/// The puzzle that this square exists on
	/// </summary>
	public GridPuzzle puzzle;


	/// <summary>
	/// Creates a connection between two grid squares along the given direction with the specified line. A(->dir->)B.
	/// Also handles any issues we have with other lines on the square.
	/// Does not check if it is a valid connection.
	/// </summary>
	/// <param name="direction"></param>
	/// <param name="A"></param>
	/// <param name="B"></param>
	/// <param name="newLine"></param>
	/// <returns></returns>
	public bool Connect(GridDirection direction, GridLine newLine) {

		if (neighbors[(int)direction] == null) {
			Debug.Log("Connect failed");
			return false;
		}

		if (neighbors[(int)direction].type == GridType.Empty) {
			int similarLines = 0;
			int otherLines = 0;
			for (int i = 0; i < neighbors[(int)direction].line.Length; i++) {
				if (neighbors[(int)direction].line[i] == newLine)
					similarLines++;
				else if (neighbors[(int)direction].line[i] != null)
					otherLines++;
			}
			//If there is a single other type of line, or more than one other of use, remove all lines.
			//this lets us reconnect a broken line
			if (otherLines > 0 || similarLines > 1) {
				//Break all the connections.
				for (int i = 0; i < neighbors[(int)direction].line.Length; i++) {
					neighbors[(int)direction].BreakConnection((GridDirection)i);
				}
			}
		}
		line[(int)direction] = newLine;
		neighbors[(int)direction].line[(int)oppositeDirection[(int)direction]] = newLine;
		newLine.Add(this, direction);
		newLine.Add(neighbors[(int)direction], oppositeDirection[(int)direction]);
		newLine.PingLine();

		return true;

		/*
		//If we are trying to connect two non neighbors, we just abort!
		{
			GridDirection tdir;
			if (GridSquare.AreNeighbors(A, B, out tdir) == false) {
				Debug.LogError("Attempt to connect with non neightbors!");
				
				return false;
			}
		}

		if (B.type == GridType.Unusable) {
			Debug.Log("X->Unusable");
			//We can not use one of  these grid squares
			return false;
		}

		//First we need to see how many connections already exist on these squares. We take a count as well as track the line that exists on it.
		int countB = 0;
		GridLine foundB = null;
		for (int i = 0; i < A.line.Length; i++) {
			if (B.line[i] != null) {
				countB++;
				foundB = B.line[i];
			}
		}


		if (B.type == GridType.Empty) {
			Debug.Log("X->Empty");
			//This has three cases. We have looped back onto ourselves. We have collided with another line, or everything is fine

			if (countB > 0 && foundB == newLine) {
				//The case we have wrapped back on ourselves
				//So we need to trim the line.
				newLine.TrimInclusive(B);
				//Substitute A for what is left in the line.
				GridSquare sub = newLine.lineSegments.Last.Value.square;
				//Figure out the new connection direction
				GridSquare.GridDirection newDirection;
				if (GridSquare.AreNeighbors(sub, B, out newDirection) == false) {
					//If they are not neigbors, something has gone wrong and we need to abort.
					newLine.DeleteFromGrid();
					return false;
				}
				//Make the connection.
				//We know know the new connection to be made, so lets do it.
				sub.line[(int)newDirection] = newLine;
				B.line[(int)GridSquare.oppositeDirection[(int)newDirection]] = newLine;
			}
			else {
				//If we have encountered another line, it needs to get removed.
				if (countB > 0) {
					//Remove it
					for (int i = 0; i < B.line.Length; i++) {
						if (B.line[i].DeletionFlag == false)
							B.line[i].DeleteFromGrid();
					}
				}

				//Now we can just make the connection like normal!
				A.line[(int)direction] = newLine;
				B.line[(int)GridSquare.oppositeDirection[(int)direction]] = newLine;
			}
		}
		else if (B.type != GridType.Empty) {
			Debug.Log("X->Socket");
			//So know we know the endpoint is a socket, so we just need to make sure there is one
			if (B.socketState[(int)GridSquare.oppositeDirection[(int)direction]] == GridSquare.SocketState.Input || B.socketState[(int)GridSquare.oppositeDirection[(int)direction]] == GridSquare.SocketState.Output) {
			//if (B.socketState[(int)GridSquare.oppositeDirection[(int)direction]] != GridSquare.SocketState.None) { 
				//If there is, we can just make the connection!
				//Implicitly, for us to have made it this far into a connection with a socket, any line that could have existed in this spot has been trimmed.
				//so we can just make the connection!
				A.line[(int)direction] = newLine;
				B.line[(int)GridSquare.oppositeDirection[(int)direction]] = newLine;				
			}
			else {
				Debug.Log("Socket Connection Failed");
				return false;
			}
		}

		//If it has a dataComponent, notify it that it is on a line
		if (B.dataComponent != null)
			B.dataComponent.ConnectionChange();
		return true;*/
	}

	public void BreakConnection(GridDirection direction) {
		if (line[(int)direction] != null) {
			line[(int)direction].Remove(this, direction);
			line[(int)direction] = null;
			if (sprites.lines[(int)direction].sprite != null)
				StartCoroutine(sprites.RemoveLineInDirection(direction, this));//Remove visuals on the grid
		}

		GridDirection opp = oppositeDirection[(int)direction];
		if (neighbors[(int)direction] != null && neighbors[(int)direction].line[(int)opp] != null) {
			neighbors[(int)direction].line[(int)opp].Remove(neighbors[(int)direction], opp);
			neighbors[(int)direction].line[(int)opp] = null;
		}
	}

	


	/// <summary>
	/// Checks if you can make a connection on this square on the given direction
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	public bool CanConnect(GridDirection dir) {
		return socketState[(int)dir] != SocketState.None;
	}


	/// <summary>
	/// Static method that checks if two Squares are neighbors. Returns the dirction of A->B.
	/// </summary>
	/// <returns></returns>
	public static bool AreNeighbors(GridSquare A, GridSquare B, out GridDirection dir) {

		//Base cases
		if (A == null || B == null || A == B) {
			dir = GridDirection.Up;
			return false;
		}

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
		
		//Clear all old components (should only be one but this is more safe)
		foreach (ColorComponent dc in this.gameObject.GetComponents<ColorComponent>()) {
			DestroyImmediate(dc);
		}

		if (type == GridType.Adder) {
			component = this.gameObject.AddComponent<ColorAdder>();
			component.square = this;
		}
		else if (type == GridType.Combiner) {
			component = this.gameObject.AddComponent<ColorCombiner>();
			component.square = this;
		}
		else if (type == GridType.Connector) {
			component = this.gameObject.AddComponent<ColorConnector>();
			component.square = this;
		}
		else if (type == GridType.Mixer) {
			component = this.gameObject.AddComponent<ColorMixer>();
			component.square = this;
		}
		else if (type == GridType.Shifter) {
			component = this.gameObject.AddComponent<ColorShifter>();
			component.square = this;
		}
		else if (type == GridType.Source) {
			component = this.gameObject.AddComponent<ColorSource>();
			component.square = this;
		}
		else if (type == GridType.Subtractor) {
			component = this.gameObject.AddComponent<ColorSubtractor>();
			component.square = this;
		}

	}

	/// <summary>
	/// Method to update the state of the GridSquare based on the classes data
	/// </summary>
	public void RebuildSquare() {
		ChangeComponent(type);


		//Now we go through and make sure that our neighbors share None and Line states;
		//This is so we do not have to update adjacent lines manually when we  make a change
		for (int i = 0; i < socketState.Length; i++) {
			if (type != GridType.Empty) {
				if (socketState[i] == SocketState.Line)
					socketState[i] = SocketState.None;
			}


			if (neighbors[i] != null) {
				if (socketState[i] == SocketState.None) {
				//If we are none on this side, we need to be none on the other side
					//Change their state
					neighbors[i].socketState[(int)oppositeDirection[i]] = SocketState.None;
					//Tell them to redo visuals
					neighbors[i].sprites.UpdateVisuals();
				}
				else {
					//if it not-none, then we need to set it to line if it is none. We do not want to override if it is set to Input or output
					if (neighbors[i].socketState[(int)oppositeDirection[i]] == SocketState.None) {
						//Change their state
						neighbors[i].socketState[(int)oppositeDirection[i]] = SocketState.Line;
						//Tell them to redo visuals
						neighbors[i].sprites.UpdateVisuals();
					}
					if (type == GridType.Empty && socketState[i] != SocketState.Line && socketState[i] != SocketState.None) {
						//If we are not a line or none, but our type is empty we cannot have input/output so we change it to a line
						socketState[i] = SocketState.Line;
					}
				}
			}
			else {
				//If we have no neighbor we set this to none!
				socketState[i] = SocketState.None;
			}
		}

		sprites.UpdateVisuals();
	}
}
