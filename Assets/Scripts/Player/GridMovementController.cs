using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementController : MonoBehaviour {

	public enum GridMovementState { Unrestricted, Disabled }
	public GridMovementState state = GridMovementState.Disabled;

	public GridSquare.LineHint currentLine = null;
	public GridSquare currentSquare;
	private PlayerControls player;
	private bool firstUnrestrictedState = true;

	// Use this for initialization
	void Start () {
		//Ensure that this is not doing anything at the start of the game
		state = GridMovementState.Disabled;
		player = PlayerControls.instance;
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerControls.instance.state != PlayerControls.PlayerState.GridInteraction)
			return;
		if (state == GridMovementState.Disabled) {
			firstUnrestrictedState = true;
			return;
		}
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
		//Get the current line if we click down on the mouse. SPECIAL CASE: in order to make the game feel a bit better, we also check if it is the first frame we are in this unrestricted state and the player is holding the mouse button down.
		if (InputManager.GetGameButtonDown(InputManager.GameButton.Interact1) || (currentLine == null && firstUnrestrictedState && InputManager.GetGameButton(InputManager.GameButton.Interact1))) {
			Debug.Log(currentSquare.CountLines()[0] + " | " + currentSquare.CountLines()[1]);
			//We want to start drawing a line, so we either need to find the one on the grid or make one (if we can)
			if (currentSquare.type != GridSquare.GridType.Empty) {
				Debug.Log("Creating a new Line!");
				//Whenever we are on a component, we can just make a new line.
				currentLine = new GridSquare.LineHint();
			}
			else {
				if (currentSquare.CountLines()[0] < 1) {
					//There are no lines here, so we cannot set the current line to anything.
					//We also do not allow starting a line on an empty section at the moment
				}
				else if (currentSquare.CountLines()[0] == 1) {
					//So now we need to check if we are clicking on the end of a line. (Total is 1 and unique is 1)
					if (currentSquare.CountLines()[1] == 1) {
						//If we are, that means we can draw using this line
						for (int i = 0; i < currentSquare.lines.Length; i++) {
							if (currentSquare.lines[i] != null) {
								currentLine = currentSquare.lines[i];
								break;
							}
						}
					}
				}
			}
		}
		else if (InputManager.GetGameButtonUp(InputManager.GameButton.Interact1)) {
			//Make it so we are no longer drawing a line
			Debug.Log("Mouse Released");
			currentLine = null;
		}


		if (InputManager.GetGameButton(InputManager.GameButton.Interact1) && currentLine != null) {
			GridSquare.GridDirection movementDirection;
			//Continue if we have player input to move in a direction.
			if (GetMovementDirection(out movementDirection)) {
				GridSquare neighbor;
				if (CheckLegalRegularMove(movementDirection, out neighbor)) {
					//Now that we have the movement direction, we need to see if another line is on this connection between nodes.

					//If the node we are connecting to is type Empty, then we need to break every other line on it
					if (neighbor.type == GridSquare.GridType.Empty) {
						//We use a while loop here to avoid desync on the lines data structure
						neighbor.ClearLines();
					}
					else {
						//Now we just clear the line we are going on
						neighbor.ClearSingleLine(GridSquare.oppositeDirection[(int)movementDirection]);
					}

					currentSquare.AddLine(movementDirection, currentLine);
					StartCoroutine(TransitionToNewSquare(movementDirection));
				}
				else if (CheckLegalBackDownLineMove(movementDirection, out neighbor)) {
					currentSquare.ClearSingleLine(movementDirection);
					StartCoroutine(TransitionToNewSquare(movementDirection));
				}
				else {
					Debug.Log("Fail move while trying to draw");
					//[TODO]play movement failure animation
				}
			}
		}
		else {
			GridSquare.GridDirection movementDirection;
			if (GetMovementDirection(out movementDirection)) {
				if (CheckLegalMovementBase(movementDirection)) {
					//We are not trying to draw so we can just move
					Debug.Log("Fail move");
					StartCoroutine(TransitionToNewSquare(movementDirection));
				}
				else {
					//[TODO] Play movement failure animation
				}
			}
		}

		firstUnrestrictedState = false;
	}//End Unrestricted State

	/// <summary>
	/// Checks if there is a basic connection on the specified movement direction
	/// </summary>
	/// <returns></returns>
	bool CheckLegalMovementBase(GridSquare.GridDirection movementDirection) {
		return currentSquare.neighbors[(int)movementDirection] != null && currentSquare.socketState[(int)movementDirection] != GridSquare.SocketState.None;
	}

	bool CheckLegalBackDownLineMove(GridSquare.GridDirection movementDirection, out GridSquare neighbor) {
		neighbor = null;
		if (CheckLegalMovementBase(movementDirection) == false)
			return false;
		neighbor = currentSquare.neighbors[(int)movementDirection];

		//If we are on our neighbor already
		if (neighbor.HasLine(currentLine)) {
			//If we are moving down a direction the current line already exists on.
			if (currentSquare.lines[(int)movementDirection] == currentLine) {
				//Branch behavoir on if we are doing this on an empty square or not
				if (currentSquare.type == GridSquare.GridType.Empty) {
					//If there is only one line on this bit, and it is us
					if (currentSquare.CountLines()[0] == 1) {
						return true;
					}
				}
				else {
					//We do not care about our other sides on a component
					return true;
				}
			}
		}

		return false;
	}

	/// <summary>
	/// Checks if a move from the current square in the specified direction is legal. Returns true it is and out returns the resulting square
	/// </summary>
	/// <param name="movementDirection"></param>
	/// <param name="neighbor"></param>
	/// <returns></returns>
	bool CheckLegalRegularMove(GridSquare.GridDirection movementDirection, out GridSquare neighbor) {
		neighbor = null;

		if (CheckLegalMovementBase(movementDirection) == false)
			return false;


		GridSquare.GridDirection directionRelativetoNeighbor = GridSquare.oppositeDirection[(int)movementDirection];
		neighbor = currentSquare.neighbors[(int)movementDirection];
		if (neighbor == null)
			return false;

		//We cannot move there if there is no channel
		if (neighbor.socketState[(int)directionRelativetoNeighbor] == GridSquare.SocketState.None)
			return false;

		if (neighbor.type != GridSquare.GridType.Empty) {
			//If we are trying to move onto a component, only move onto inputs and oututs
			if (neighbor.socketState[(int)directionRelativetoNeighbor] != GridSquare.SocketState.Input && neighbor.socketState[(int)directionRelativetoNeighbor] != GridSquare.SocketState.Output)
				return false;
		}
		else {
			if (neighbor.CountLines()[0] > 0) {
				if (neighbor.HasLine(currentLine))//[TODO] add a check for something here maybe?
					return false;
			}
		}
		Debug.Log("Passed regular movement check.");
		return true;
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

	private Vector3 posAboveSquare(GridSquare square) {
		return square.transform.position + square.transform.forward * -3.0f * square.puzzle.transform.localScale.x;
	}

	IEnumerator TransitionToNewSquare(GridSquare.GridDirection transitionDirection) {
		state = GridMovementState.Disabled;
		GridSquare newSquare = currentSquare.neighbors[(int)transitionDirection];
		Vector3 startPosition = player.playerCamera.transform.position;
		Vector3 goalPosition = posAboveSquare(newSquare);
		Quaternion startRotation = player.playerCamera.transform.rotation;
		//Debug.Log("Move " + currentSquare.name + " -> " + newSquare.name);


		float cTime = 0.0f;
		//The transition takes 2 seconds
		float maxTime = 0.25f;

		while (cTime < maxTime) {
			player.playerCamera.transform.position = Vector3.Lerp(startPosition, goalPosition, cTime / maxTime);
			player.playerCamera.transform.rotation = Quaternion.Lerp(startRotation, Quaternion.LookRotation(newSquare.transform.forward), cTime / maxTime);
			cTime += Time.deltaTime;
			yield return null;
		}


		player.playerCamera.transform.position = goalPosition;
		player.playerCamera.transform.LookAt(newSquare.gameObject.transform);

		yield return new WaitForSeconds(0.01f);


		yield return null;
		currentSquare = newSquare;

		yield return null;
		state = GridMovementState.Unrestricted;
	}

	IEnumerator TransitionToGrid() {
		Vector3 startPosition = player.playerCamera.transform.position;
		Vector3 goalPosition = posAboveSquare(currentSquare);
		Quaternion startRotation = player.playerCamera.transform.rotation;

		float cTime = 0.0f;
		//The transition takes 2 seconds
		float maxTime = 0.5f;

		while (cTime < maxTime) {
			player.playerCamera.transform.position = Vector3.Slerp(startPosition, goalPosition, cTime / maxTime);

			player.playerCamera.transform.rotation = Quaternion.Slerp(startRotation, Quaternion.LookRotation(currentSquare.transform.forward), cTime / maxTime);

			cTime += Time.deltaTime;
			yield return null;
		}

		//enforce the final values
		player.playerCamera.transform.position = goalPosition;
		player.playerCamera.transform.LookAt(currentSquare.gameObject.transform);


		state = GridMovementState.Unrestricted;

		yield return null;
		player.state = PlayerControls.PlayerState.GridInteraction;
	}


	IEnumerator TransitionToPlayer() {
		currentSquare = null;
		state = GridMovementState.Disabled;
		//For best results, this should use local position and local rotation so we can use vector.zero and player.cameraPitch to line things up right
		Vector3 startPosition = player.playerCamera.transform.localPosition;
		Quaternion startRotation = player.playerCamera.transform.localRotation;

		float cTime = 0.0f;
		//The transition takes 2 seconds
		float maxTime = 0.5f;

		while (cTime < maxTime) {
			player.playerCamera.transform.localPosition = Vector3.Slerp(startPosition, Vector3.zero, cTime / maxTime);

			player.playerCamera.transform.localRotation = Quaternion.Slerp(startRotation, Quaternion.Euler(player.GetCamPitch(), 0.0f, 0.0f), cTime / maxTime);

			cTime += Time.deltaTime;
			yield return null;
		}

		//Enfore the final values
		player.playerCamera.transform.localPosition = Vector3.zero;
		player.playerCamera.transform.localRotation = Quaternion.Euler(player.GetCamPitch(), 0.0f, 0.0f);

		yield return null;
		player.state = PlayerControls.PlayerState.Freemove;
	}
}
