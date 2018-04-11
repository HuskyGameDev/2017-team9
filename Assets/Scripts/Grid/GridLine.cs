using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLine {
	#region State
	#region Private
	private Color32 color = Color.white;
	#endregion
	#region Public
	public List<KeyValuePair<GridSquare,GridSquare.GridDirection>> squares = new List<KeyValuePair<GridSquare, GridSquare.GridDirection>>();
	#endregion
	#endregion


	#region Methods
	#region Public

	public void Add(GridSquare square, GridSquare.GridDirection dir) {
		square.sprites.lines[(int)dir].color = color;
		squares.Add(new KeyValuePair<GridSquare, GridSquare.GridDirection>(square, dir));
	}

	public void Remove(GridSquare square, GridSquare.GridDirection dir) {
		KeyValuePair<GridSquare, GridSquare.GridDirection> obj = squares.Find(delegate(KeyValuePair<GridSquare, GridSquare.GridDirection> pair) { return pair.Key == square && pair.Value == dir; });
		squares.Remove(obj);
	}

	public List<KeyValuePair<GridSquare, GridSquare.GridDirection>> GetOutputSockets() {
		return squares.FindAll(
			delegate (KeyValuePair<GridSquare, GridSquare.GridDirection> pair) {
				return pair.Key.type != GridSquare.GridType.Empty && pair.Key.socketState[(int)pair.Value] == GridSquare.SocketState.Output;
			});
	}

	public List<KeyValuePair<GridSquare, GridSquare.GridDirection>> GetInputSockets() {
		return squares.FindAll(
			delegate (KeyValuePair<GridSquare, GridSquare.GridDirection> pair) {
				return pair.Key.type != GridSquare.GridType.Empty && pair.Key.socketState[(int)pair.Value] == GridSquare.SocketState.Input;
			});
	}


	public void Ping() {
		//Debug.Log("PING! " + squares.Count);
		//Gather all inputs and outputs on this state.
		List<KeyValuePair<GridSquare, GridSquare.GridDirection>> inputs = GetInputSockets();
		List<KeyValuePair<GridSquare, GridSquare.GridDirection>> outputs = GetOutputSockets();

		//Update line color and components since there may have been a change.
		if (inputs.Count > 1 || outputs.Count > 1)
			Debug.Log("We have a line here with more than one kind of  socketstate(input/output)");
		else {
			if (inputs.Count == 1 && outputs.Count > 0) {
				//This line is inputting into this component, so we need to notify it of a change
				inputs[0].Key.component.CheckOutput();
			}
			UpdateColor(outputs);
		}
	}

	/// <summary>
	/// Updates the color of this line. If passed an output list it will use that to update the line color
	/// </summary>
	/// <param name="outputs"></param>
	public void UpdateColor(List<KeyValuePair<GridSquare, GridSquare.GridDirection>> outputs = null) {
		if (outputs == null)
			outputs = GetOutputSockets();
		//Debug.Log("Update color was called");
		Color32 tempColor = ((outputs.Count == 1) ? outputs[0].Key.component.GetOutput().color : (Color32)Color.white);
		//If the color is different update all of our line sections
		if (tempColor.Equals(color) == false) {
			//Debug.Log("Our Color is different!");
			foreach (KeyValuePair<GridSquare, GridSquare.GridDirection> pair in squares)
				pair.Key.sprites.lines[(int)(pair.Value)].color = tempColor;
			color = tempColor;
		}
	}
	/*
	public void SplitOnSquare(GridSquare square, GridSquare.GridDirection dir) {
		//Check if we go on in two different directions
		List<KeyValuePair<GridSquare, GridSquare.GridDirection>> otherSide = new List<KeyValuePair<GridSquare, GridSquare.GridDirection>>();
		//Starting with the node after dir, we follow that for as long as we can adding it to otherSide.
		//We then remove everying from otherside and the passed stuff from our current list
		//We make a new line using the otherSide information.

		GridSquare currentSquare = square;
		GridSquare.GridDirection currentDirection = dir;
		while (true) {
			otherSide.Add(new KeyValuePair<GridSquare, GridSquare.GridDirection>(currentSquare, currentDirection));
			Remove(currentSquare, currentDirection);
			GridSquare.GridDirection oppositeDirection = GridSquare.oppositeDirection[(int)dir];
			GridSquare nextSquare = currentSquare.neighbors[(int)currentDirection];
			if (nextSquare == null)
				break;
			if (nextSquare.lines[(int)oppositeDirection] == this) {
				otherSide.Add(new KeyValuePair<GridSquare, GridSquare.GridDirection>(nextSquare, oppositeDirection));
				Remove(nextSquare, oppositeDirection);
			}
			else
				break;
			//So now we have taken care of the opposite side of where we came from. Now we need to calculate were we are going
			for (int i = 0; i < nextSquare.lines.Length; i++) {
				if (nextSquare.lines[i] == this) {
					currentSquare = nextSquare;
					currentDirection = (GridSquare.GridDirection)i;
					continue;
				}
			}
			break;
		}

		GridLine newLine = new GridLine();
		KeyValuePair<GridSquare, GridSquare.GridDirection> obj = squares.Find(delegate (KeyValuePair<GridSquare, GridSquare.GridDirection> pair) { return pair.Key == square && pair.Value == dir; });
		otherSide.Remove(obj);
		for (int i = 0; i < otherSide.Count; i++) {
			otherSide[i].Key.AddLine(otherSide[i].Value, newLine);
		}

	}*/



	/*
	/// <summary>
	/// Adds a square to this line. Manages if there is another line on the square.
	/// </summary>
	/// <param name="square"></param>
	public void Add(GridSquare square) {
		Debug.Log("Add Called " + square.gameObject.name + " |" + square.lines.Count);
		if (lineNodes.Contains(square)) {
			Debug.Log("We already have this node on our list.");
			return;
		}
		if (square.type == GridSquare.GridType.Empty && square.lines.Count > 0) {
			//If we are trying to connect onto the tip of the line we can join them
			if (square.lines.Count > 1)
				Debug.LogError("An empty grid square has more than one line! This will break everything!");
			else if (square.lines.Count == 1)
				Debug.Log("We are adding onto a square with just one line already there");

			if (square.lines[0].IsTip(square)) {
				Debug.Log("We have decided to Join Lines");
				JoinLines(square.lines[0]);
				return;
			}
			else {
				//Otherwise, we need to split this line into two
				Debug.Log("We are breaking a line in front of us.");
				square.lines[0].BreakLine(square);
			}
		}
		//Add the node to our line
		lineNodes.AddLast(square);
		//Add a reference to this node on the square
		square.lines.Add(this);
		//Update Line Color

		UpdateLineColor();
		UpdateComponents();

		//Fire the visuals for drawing the line!
		if (lineNodes.Last.Previous != null)
			lineNodes.Last.Previous.Value.sprites.StartCoroutine(lineNodes.Last.Previous.Value.sprites.DrawLineInDirection(GetDirectionFromSquare(lineNodes.Last.Previous.Value), lineNodes.Last.Previous.Value, color));//Draw visuals on the grid
		//PingLine();
	}

	/// <summary>
	/// Combines two lines together, adding the other's nodes after this one.
	/// </summary>
	/// <param name="other"></param>
	public void JoinLines(GridLine other) {
		if (lineNodes.Count < 1) {
			Debug.Log("Join failed because we are an empty line.");
			return;
		}
		GridSquare currentLastSquare = lineNodes.Last.Value;
		//Since each line has to start at a component we know that the last is were we need to start adding.
		LinkedListNode<GridSquare> current = other.lineNodes.Last;
		while (current != null) {
			//add square to this line.
			lineNodes.AddLast(current.Value);
			//Remove the old line and add this one
			current.Value.lines.Remove(other);
			current.Value.lines.Add(this);
			//remove the last from the line
			other.lineNodes.RemoveLast();
			//set current to be the last of the line.
			current = other.lineNodes.Last;
		}
		
		//Fire visuals for the new connection.
		currentLastSquare.sprites.StartCoroutine(currentLastSquare.sprites.DrawLineInDirection(GetDirectionFromSquare(currentLastSquare), currentLastSquare, color));
	}

	/// <summary>
	/// Splits this line into two that does not include the parameter square
	/// </summary>
	public GridLine BreakLine(GridSquare breakPoint) {
		//[TODO] There are probably some edge cases here that need to be checked
		if (lineNodes.Count == 0 || lineNodes.Contains(breakPoint) == false) {
			return null;
		}

		//If this line is complete then we can break it into two lines
		if (IsComplete()) {
			GridLine newLine = new GridLine();
			LinkedListNode<GridSquare> current = lineNodes.Last;
			while (current.Value != breakPoint) {
				//add square to new line.
				newLine.Add(current.Value);
				//Remove this line from the square and add the new one
				current.Value.lines.Remove(this);
				current.Value.lines.Add(newLine);
				//remove the last from this line
				lineNodes.RemoveLast();
				//set current to be the last of this line.
				current = lineNodes.Last;

			}
			//We stopped our iteration wiht current.value == breakpoint, so breakpoint is still on our list. Remove it
			lineNodes.RemoveLast();
			// Do visuals for breaking this point
			//Figure out the directions we need to break. We know that the last node of each line were our previus connections
			GridSquare.GridDirection dir1, dir2;
			GridSquare.AreNeighbors(breakPoint, lineNodes.Last.Value, out dir1);
			GridSquare.AreNeighbors(breakPoint, newLine.lineNodes.Last.Value, out dir2);
			breakPoint.sprites.StartCoroutine(breakPoint.sprites.RemoveLineInDirection(dir1, breakPoint));
			breakPoint.sprites.StartCoroutine(breakPoint.sprites.RemoveLineInDirection(dir2, breakPoint));

			if (newLine.lineNodes.Count < 2)
				newLine.DestroyLine();
			if (lineNodes.Count < 2)
				DestroyLine();
			return newLine;
		}
		else {
			//Else this means that first is, so starting with break point we remove all other nodes.
			LinkedList<GridSquare> toRemove = new LinkedList<GridSquare>();
			LinkedListNode<GridSquare> current = lineNodes.Last;
			while (current.Value != breakPoint) {
				//add square to new line.
				toRemove.AddLast(current.Value);
				//override the square.line reference to be the new line;
				current.Value.lines.Remove(this);
				//remove the last from this line
				lineNodes.RemoveLast();
				//set current to be the last of this line.
				current = lineNodes.Last;

			}
			//We stopped our iteration wiht current.value == breakpoint, so breakpoint is still on our list. Remove it
			toRemove.AddLast(current.Value);
			//override the square.line reference to be the new line;
			current.Value.lines.Remove(this);
			//remove the last from this line
			lineNodes.RemoveLast();

			breakPoint.sprites.StartCoroutine(breakPoint.sprites.RemoveLine(toRemove));
		}

		if (lineNodes.Count < 2)
			DestroyLine();

		return null;
	}

	/// <summary>
	/// Returns true if the parameter is the first or last node
	/// </summary>
	/// <param name="square"></param>
	/// <returns></returns>
	public bool IsTip(GridSquare square) {
		if (lineNodes.Count < 1)
			return false;
		return lineNodes.First.Value == square || lineNodes.Last.Value == square;
	}

	/// <summary>
	/// A line is considered complete when its first and last node are a square where square.type != GridSquare.GridType.Empty
	/// </summary>
	/// <returns></returns>
	public bool IsComplete() {
		if (lineNodes.Count < 1)
			return false;
		return lineNodes.First.Value.type != GridSquare.GridType.Empty || lineNodes.Last.Value.type != GridSquare.GridType.Empty;
	}


	/// <summary>
	/// Removes the last node on this line
	/// </summary>
	/// <returns></returns>
	public GridSquare Pop() {
		GridSquare ret = lineNodes.Last.Value;

		//Do the visuals for a line removal
		lineNodes.Last.Value.sprites.StartCoroutine(lineNodes.Last.Value.sprites.RemoveLineInDirection(GridSquare.oppositeDirection[(int)GetDirectionFromSquare(lineNodes.Last.Previous.Value)], lineNodes.Last.Value));
		lineNodes.RemoveLast();

		return ret;
	}*/
	/*
	public void DestroyLine() {
		if (lineNodes.Count < 1) {
			Debug.Log("Destroying Empty Line!");
			return;
		}



		Debug.Log("Destroying Line!");
		LinkedList<GridSquare> removed = new LinkedList<GridSquare>();
		for (int i = 0; i < lineNodes.Count; i++) {
			removed.AddLast(lineNodes.First.Value);
			lineNodes.First.Value.lines.Remove(this);
			lineNodes.RemoveFirst();
		}
		// used removed to handle visuals
		removed.First.Value.sprites.StartCoroutine(removed.First.Value.sprites.RemoveLine(removed));

	}*/

	/*
	/// <summary>
	/// Checks our two endpoints for a component and set our color to the output.
	/// </summary>
	public void UpdateLineColor() {
		
		//Check our start point for our line. If it is on an output then we can get a color from it 
		foreach (GridLine line in lineNodes.First.Value.GetLinesOnSocketState(GridSquare.SocketState.Output)) {
			if (line == this) {
				color = lineNodes.First.Value.component.GetOutput().color;
			}
		}

		//Check our end point for our line. If it is on an output then we can get a color from it 
		foreach (GridLine line in lineNodes.Last.Value.GetLinesOnSocketState(GridSquare.SocketState.Output)) {
			if (line == this) {
				color = lineNodes.Last.Value.component.GetOutput().color;
			}
		}

		LinkedListNode<GridSquare> current = lineNodes.First;
		while (current != null) {
			if (current.Next != null) {
				GridSquare.GridDirection dir = GetDirectionFromSquare(current.Value);
				current.Value.sprites.lines[(int)dir].color = color;
				current.Next.Value.sprites.lines[(int)GridSquare.oppositeDirection[(int)dir]].color = color;
			}
			current = current.Next;
		}

	}

	public GridSquare.GridDirection GetDirectionFromSquare(GridSquare square) {
		LinkedListNode<GridSquare> current = lineNodes.Find(square);
		GridSquare.GridDirection dir;
		GridSquare.AreNeighbors(current.Value, current.Next.Value, out dir);
		return dir;
	}

	public void UpdateComponents() {
		foreach (GridLine line in lineNodes.First.Value.GetLinesOnSocketState(GridSquare.SocketState.Input)) {
			if (line == this) {
				lineNodes.First.Value.component.CheckOutput();
			}
		}

		//Check our end point for our line. If it is on an output then we can get a color from it 
		foreach (GridLine line in lineNodes.Last.Value.GetLinesOnSocketState(GridSquare.SocketState.Input)) {
			if (line == this) {
				lineNodes.Last.Value.component.CheckOutput();
			}
		}
	}*/

	#endregion
	#endregion
	/*
	public void PingLine() {
		UpdateLineColor();
		foreach (KeyValuePair<GridSquare, GridSquare.GridDirection> entry in linePoints) {
			entry.Key.sprites.lines[(int)entry.Value].color = color;
			if (entry.Key.component != null)
				entry.Key.component.CheckOutput();
		}
	}

	private void UpdateLineColor() {
		List<GridSquare> found = new List<GridSquare>();
		foreach (KeyValuePair<GridSquare, GridSquare.GridDirection> entry in linePoints) {
			if (entry.Key.component != null) {
				if (entry.Key.socketState[(int)entry.Value] == GridSquare.SocketState.Output) {
					found.Add(entry.Key);
				}
			}
		}
		Debug.Log("linePoints " + linePoints.Count);
		if (found.Count == 1) {
			color = found[0].component.GetOutput().color;
		}
		else {
			color = Color.white;
		}
	}

	//Takes the direction given and follows a line until it hits a component
	public KeyValuePair<GridSquare, GridSquare.GridDirection>[] FindDataComponents(List<GridSquare> ignoreList = null) {
		List<KeyValuePair<GridSquare, GridSquare.GridDirection>> found = new List<KeyValuePair<GridSquare, GridSquare.GridDirection>>();
		foreach (KeyValuePair<GridSquare, GridSquare.GridDirection> entry in linePoints) {
			if (ignoreList != null && ignoreList.Contains(entry.Key))
				continue;
			if (entry.Key.component != null) {
				found.Add(entry);
			}
		}

		return found.ToArray();
	}*/
}
