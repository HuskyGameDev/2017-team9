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
	public enum SocketState {None, Line, Input, Output}

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
	//public List<GridLine> lines = new List<GridLine>();
	public LineHint[] lines = new LineHint[4];


	/// <summary>
	/// The puzzle that this square exists on
	/// </summary>
	public GridPuzzle puzzle;

	/// <summary>
	/// Checks if you can make a connection on this square on the given direction
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	public bool CanConnect(GridDirection dir) {
		return socketState[(int)dir] != SocketState.None;
	}

	/// <summary>
	/// Removes all lines in the line data structure
	/// </summary>
	public void ClearLines() {
		for (int i = 0; i < lines.Length; i++) {
			if (lines[i] != null) {
				ClearSingleLine((GridDirection)i);
			}
		}
	}

	/// <summary>
	/// Removes a line on this square in the current direction
	/// </summary>
	/// <param name="dir"></param>
	public void ClearSingleLine(GridDirection dir) {
		if (lines[(int)dir] == null)
			return;
		//Get this current edge.
		GridSquare other = neighbors[(int)dir];
		GridDirection otherDirection = oppositeDirection[(int)dir];
		LineHint hint = lines[(int)dir];

		//Clear this connection
		lines[(int)dir] = null;
		other.lines[(int)otherDirection] = null;
		//[TODO] Play removal animation here.
		/* This is how I was doing it before
		if (neighbors[(int)dir].type != GridType.Empty)
			sprites.StartCoroutine(sprites.RemoveLineInDirection(oppositeDirection[(int)dir], neighbors[(int)dir], tempLine));
		else
			sprites.StartCoroutine(sprites.RemoveLineInDirection(dir, this, tempLine));
		*/
		sprites.StartCoroutine(sprites.RemoveLineInDirection(dir, this));




		//We branch based on if it is a component or not
		if (other.type == GridType.Empty) {
			//Now that we know our edge, we need to see if this line continues and If it does call Update line on that line
			for (int i = 0; i < other.lines.Length; i++) {
				if (other.lines[i] == hint) {
					Debug.Log("Updating Orphaned Line.");
					other.UpdateLine((GridDirection)i);
				}
			}
		}

		/*
		if (lines[(int)dir] == null)
			return;

		//Store the lines for modification later
		GridLine tempLine;
		tempLine = lines[(int)dir];

		lines[(int)dir] = null;
		neighbors[(int)dir].lines[(int)oppositeDirection[(int)dir]] = null;


		lines[(int)dir] = null;
		neighbors[(int)dir].lines[(int)oppositeDirection[(int)dir]] = null;

		tempLine.Remove(this, dir);
		tempLine.Remove(neighbors[(int)dir], oppositeDirection[(int)dir]);


		//So for clearing a line we need to do something special
		//If we are clearing a line in a direction that belongs to a component then we have it shrink out of the component
		//[TODO] There is a visual bug that must be accounted for here. If you are backing down the very last point of this line the visuals will break
		if (neighbors[(int)dir].type != GridType.Empty)
			sprites.StartCoroutine(sprites.RemoveLineInDirection(oppositeDirection[(int)dir], neighbors[(int)dir], tempLine));
		else
			sprites.StartCoroutine(sprites.RemoveLineInDirection(dir, this, tempLine));*/
	}

	public LinkedList<KeyValuePair<GridSquare, GridDirection>> GetLine(GridDirection startDir) {
		//Track a list of all sections of this line we have found
		LinkedList<KeyValuePair<GridSquare, GridDirection>> foundList = new LinkedList<KeyValuePair<GridSquare, GridDirection>>();
		//We treat the passed direction as the end point, so we will being 

		GridSquare current = this;
		GridDirection currentDirection = startDir;
		int limiter = 0;
		while (current != null && limiter < 10000) {
			limiter++;
			//Store the grid square on the other side.
			GridSquare next = current.neighbors[(int)currentDirection];
			GridDirection prevDirection = oppositeDirection[(int)currentDirection];
			LineHint line = current.lines[(int)currentDirection];
			//Add this edge to teh list
			foundList.AddLast(new KeyValuePair<GridSquare, GridDirection>(current, currentDirection));
			foundList.AddLast(new KeyValuePair<GridSquare, GridDirection>(next, prevDirection));
			current = null; //We have to set it to null here to make sure that the loop will end
			//Find the next direction to move on.
			for (int i = 0; i < next.lines.Length; i++) {
				if ((GridDirection)i == prevDirection) continue; //Dont fold back on ourselves
				if (next.lines[i] == line) {
					current = next;
					currentDirection = (GridDirection)i;
					break;
				}
			}
		}
		if (limiter >= 10000) {
			Debug.Log("Hit Limit");
			return null;
		}
		return foundList;
	}


	public void UpdateLine(GridDirection startDir) {
		LinkedList<KeyValuePair<GridSquare, GridDirection>> foundList = GetLine(startDir);
		if (foundList == null) return;

		//Now we have the data structure for this line.
		//If either end is a component with an output socket on the direction, we are that color
		//Otherwise we are white.
		//If we have a complete connection, we need to notify components with input sockets to change their output.
		GridSquare first = foundList.First.Value.Key;
		GridSquare last = foundList.Last.Value.Key;


		Color32 color = Color.white;
		if (first.type != GridType.Empty && first.socketState[(int)foundList.First.Value.Value] == SocketState.Output) {
			//Debug.Log("Found a Color");
			color = first.component.GetOutput().color;
		}
		if (last.type != GridType.Empty && last.socketState[(int)foundList.Last.Value.Value] == SocketState.Output) {
			//Debug.Log("Found a Color");
			color = last.component.GetOutput().color;
		}

		//Update the color of all found segements
		foreach (KeyValuePair<GridSquare,GridDirection> pair in foundList) {
			pair.Key.sprites.lines[(int)pair.Value].color = color;
		}

		if (first.type != GridType.Empty && last.type != GridType.Empty) {
			//Notify the input sockets to update
			first.component.CheckOutput();
			last.component.CheckOutput();
		}

	}

	/// <summary>
	/// Adds a line to this node.
	/// </summary>
	/// <param name="dir"></param>
	/// <param name="line"></param>
	public void AddLine(GridDirection dir, LineHint line) {
		//Check other.
		GridSquare other = neighbors[(int)dir];
		if (other == null) //We cannot add in a direction we cannot go
			return;
		if (other.type == GridType.Empty)
			other.ClearLines();
		else
			other.ClearSingleLine(oppositeDirection[(int)dir]);

		//Create our connection
		lines[(int)dir] = line;
		neighbors[(int)dir].lines[(int)oppositeDirection[(int)dir]] = line;

		//Start the coroutine
		sprites.StartCoroutine(sprites.DrawLineInDirection(dir, this));
	}

	/// <summary>
	/// Checks if a line is on this square
	/// </summary>
	/// <param name="line"></param>
	public bool HasLine(LineHint line) {
		for (int i = 0; i < lines.Length; i++) {
			if (lines[i] == line)
				return true;
		}
		return false;
	}

	/// <summary>
	/// Returnes the number of lines and unique lines on this node
	/// </summary>
	/// <returns>Total,Unique</returns>
	public int[] CountLines() {
		int totalLines = 0;
		int uniqueLines = 0;

		List<LineHint> foundLine = new List<LineHint>();
		for (int i = 0; i < lines.Length; i++) {
			if (lines[i] != null) {
				totalLines++;
				if (foundLine.Contains(lines[i]) == false) {
					foundLine.Add(lines[i]);
					uniqueLines++;
				}
			}
		}
		return new int[] {totalLines, uniqueLines};
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

	public class LineHint { }
}
