using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementController : MonoBehaviour {

	public enum GridMovementState { Unrestricted, Disabled }
	public GridMovementState state = GridMovementState.Disabled;

	public GridLine currentLine = null;
	public GridSquare currentSquare;


	// Use this for initialization
	void Start () {
		//Ensure that this is not doing anything at the start of the game
		state = GridMovementState.Disabled;
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerControls.instance.state != PlayerControls.PlayerState.GridInteraction)
			return;
		if (state == GridMovementState.Disabled)
			return;
		else if (state == GridMovementState.Unrestricted)
			UnrestrictedState();
	}

	/// <summary>
	/// Handles logic for when the GridMovementController.state == Unrestricted
	/// </summary>
	void UnrestrictedState() {
		if (currentSquare == null) {
			Debug.Log("We have an active GridMovementController but it was not provided a square to work on. Disabling.");
			state = GridMovementState.Disabled;
			return;
		}
		if (InputManager.GetGameButtonDown(InputManager.GameButton.Interact1)) {
			//We want to start drawing a line, so we either need to find the one on the grid or make one (if we can)
			if (currentSquare.type != GridSquare.GridType.Empty) {
				//Whenever we are on a component, we can just make a new line.
				GridLine newLine = new GridLine();
				newLine.Add(currentSquare);
				currentLine = newLine;
			}
			else {
				if (currentSquare.lines.Count < 1) {
					//There are no lines here, so we cannot set the current line to anything.
					//We also do not allow starting a line on an empty section at the moment
				}
				else if (currentSquare.lines.Count == 1) {
					//So now we need to check if we are clicking on the end of a line.
					if (currentSquare.lines[0].IsTip(currentSquare)) {
						//If we are, that means we can draw using this line
						currentLine = currentSquare.lines[0];
					}
				}
				else {
					Debug.Log(currentSquare.gameObject.name + " has more than one line as an empty square! Disabling.");
					state = GridMovementState.Disabled;
					return;
				}
			}
		}
		else if (InputManager.GetGameButtonUp(InputManager.GameButton.Interact1)) {
			//We are no longer drawing a line
			//If our line is only on this node then we want to delete it since it is not doing anything productive.
			if (currentLine != null && currentLine.lineNodes.Count < 2)
				currentLine.DestroyLine();
			//Make it so we are no longer drawing a line
			currentLine = null;
		}


		if (InputManager.GetGameButton(InputManager.GameButton.Interact1) && currentLine != null) {
			GridSquare.GridDirection movementDirection;
			//Continue if we have player input to move in a direction.
			if (GetMovementDirection(out movementDirection)) {
				//Now that we have the movement direction, we need to see if another line is on this connection between nodes.
				GridSquare neighbor = currentSquare.neighbors[(int)movementDirection];
				if (neighbor != null) {
					//If a connection already exists here we must break it
					if (CheckForLineOnConnection(currentSquare, neighbor, movementDirection)) {
						//This means another line is on the connection, so we need to break it.
						//We need to remove the current node, and neighbor from this line.
						//Based on ordering, we may have another line get generated and remove the second one from the generated line as well
						GridLine createdLine = currentLine.BreakLine(currentSquare);
						currentLine.BreakLine(neighbor);
						if (createdLine != null)
							createdLine.BreakLine(neighbor);
					}
					//If the node we are connecting to is type Empty, then we need to break every other line on it
					if (neighbor.type == GridSquare.GridType.Empty) {
						//We use a while loop here to avoid desync on the lines data structure
						while (neighbor.lines.Count > 0) {
							neighbor.lines[0].BreakLine(neighbor);
						}
					}
					//No we add this new node
					currentLine.Add(neighbor);
					//Update our current node
					currentSquare = neighbor;
				}
			}
		}
	}

	/// <summary>
	/// Returns true if there exists a line that connects the two given nodes.
	/// </summary>
	/// <param name="square"></param>
	/// <param name="neighbor"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	bool CheckForLineOnConnection(GridSquare square, GridSquare neighbor, GridSquare.GridDirection dir) {
		//Loop through all their lines
		for (int i = 0; i < square.lines.Count; i++) {
			for (int l = 0; l < neighbor.lines.Count; l++) {
				//If they are the same line
				if (square.lines[i] == neighbor.lines[l]) {
					//Check
					GridLine line =	square.lines[i];
					LinkedListNode<GridSquare> node = line.lineNodes.Find(square);
					if (node.Next.Value == neighbor || node.Previous.Value == neighbor) {
						return true;
					}
				}
			}
		}
		return false;
	}


	bool GetMovementDirection(out GridSquare.GridDirection transitionDirection) {
		//Setup a default direction to return
		transitionDirection = GridSquare.GridDirection.Right;

		//First check if the player wants to move in a direction
		float x = InputManager.GetAxis(InputManager.Axis.LeftHorizontal);
		float y = InputManager.GetAxis(InputManager.Axis.LeftVertical);

		if (Mathf.Abs(x) > 0 && Mathf.Abs(y) > 0) {
			//if we are holding down both inputs do nothing
		}
		else if (x > 0) {
			transitionDirection = GridSquare.GridDirection.Right;
			return true;
		}
		else if (x < 0) {
			transitionDirection = GridSquare.GridDirection.Left;
			return true;
		}
		else if (y > 0) {
			transitionDirection = GridSquare.GridDirection.Up;
			return true;
		}
		else if (y < 0) {
			transitionDirection = GridSquare.GridDirection.Down;
			return true;
		}
		//This means we do not want to move
		return false;
	}
}
