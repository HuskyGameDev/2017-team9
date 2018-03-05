using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The grid surfer classes handles the state of the player drawing lines by being fixed on the grid.
/// </summary>
public class GridSurfer : MonoBehaviour {

	public enum GridSurferState { CanMove, Disabled}
	public GridSurferState state = GridSurferState.Disabled;

	public GridSquare currentSquare;

	public PlayerControls player;
	public GridSquare.GridDirection transitionDirection;

	// Use this for initialization
	void Start () {
		state = GridSurferState.Disabled;
	}

	// Update is called once per frame
	void Update() {
		if (player.state == PlayerControls.PlayerState.GridInteraction) {
			if (currentSquare == null) {
				Debug.LogWarning("Player is in Grid Interaction state, but the line is null.");
				return;
			}
			else {
				//check if this is a component.
				if (currentSquare.dataComponent != null) {
					for (int i = 0; i < currentSquare.socketState.Length; i++) {
						if (currentSquare.socketState[i] == GridSquare.SocketState.Input) {
							player.dataPanel.sections[i].gameObject.SetActive(true);
							player.dataPanel.sections[i].labelText.text = "Input";
							player.dataPanel.sections[i].dataText.text = "?";
						}
						else if (currentSquare.socketState[i] == GridSquare.SocketState.Output) {
							player.dataPanel.sections[i].gameObject.SetActive(true);
							player.dataPanel.sections[i].labelText.text = "Output";
							player.dataPanel.sections[i].dataText.text = currentSquare.dataComponent.GetOutputString();
						}
						else {
							player.dataPanel.sections[i].gameObject.SetActive(false);
						}
					}
				}
				else {
					DisableVisuals();
				}

			}
			if (state == GridSurferState.CanMove) {
				//First check if the player wants to move in a direction
				float x = InputManager.GetAxis(InputManager.Axis.LeftHorizontal);
				float y = InputManager.GetAxis(InputManager.Axis.LeftVertical);
				bool doMove = false;

				if (Mathf.Abs(x) > 0 && Mathf.Abs(y) > 0) {
					//if we are holding down both inputs do nothing
				}
				else if (x > 0) {
					transitionDirection = GridSquare.GridDirection.Right;
					doMove = true;
				}
				else if (x < 0) {
					transitionDirection = GridSquare.GridDirection.Left;
					doMove = true;
				}
				else if (y > 0) {
					transitionDirection = GridSquare.GridDirection.Up;
					doMove = true;
				}
				else if (y < 0) {
					transitionDirection = GridSquare.GridDirection.Down;
					doMove = true;
				}
				else {

				}

				if (doMove) {
					if (InputManager.GetGameButton(InputManager.GameButton.Interact1)) {
						//This means we are trying to draw a line.
						//THe directions we can move are now restricted
						
						//Frst we check if we are restricted in any way.
						bool failed = false;
						int foundPrevLine = 0; // foundPrevLine is used later to check for previous lines and then use it for drawing on empty sockets

						{
							//Failcase: We can only move on non empty sockets
							if (currentSquare.socketState[(int)transitionDirection] == GridSquare.SocketState.None)
								failed = true;

							//Failcase: we can only leave a component on a socket
							if (currentSquare.type != GridSquare.GridType.Empty) {
								if (currentSquare.socketState[(int)transitionDirection] != GridSquare.SocketState.Input &&
									currentSquare.socketState[(int)transitionDirection] != GridSquare.SocketState.Output) {
									//Fail Case
									failed = true;
								}
							}
							//Handle the failcases involving where we are going
							GridSquare other = currentSquare.neighbors[(int)transitionDirection];
							if (other == null) {
								failed = true;
							}
							else {
								//Failcase: We are moving onto a component not through an input/output socket. 
								GridSquare.SocketState othersSocket = other.socketState[(int)GridSquare.oppositeDirection[(int)transitionDirection]];
								if (other.type != GridSquare.GridType.Empty && (othersSocket != GridSquare.SocketState.Input && othersSocket != GridSquare.SocketState.Output)) {
									Debug.LogWarning("Failed by moving onto a socket");
									failed = true;
								}
							}



							//Failcase: We are trying to start drawing on an empty square with no lines already here.

							if (currentSquare.type == GridSquare.GridType.Empty) {

								for (int i = 0; i < currentSquare.line.Length; i++) {
									if (currentSquare.line[i] != 0) {
										foundPrevLine = currentSquare.line[i];
									}
								}
								if (foundPrevLine == 0) {
									Debug.LogWarning("Failed move no existing line on an empty socket.");
									failed = true;
								}
							}
						}
						//Only continue if we have not found a failcase
						if (failed == false) {
							//Second, we do prep work
							if (currentSquare.type != GridSquare.GridType.Empty) {
								if (currentSquare.line[(int)transitionDirection] == 0) {
									//There is no line, so we create a new one with this as its sparting point
									currentSquare.line[(int)transitionDirection] = currentSquare.puzzle.GetLine();
								}
							}
							else {
								currentSquare.line[(int)transitionDirection] = foundPrevLine;
							}

							//Finnaly, we make the connection
							if (currentSquare.Connect(transitionDirection, currentSquare.line[(int)transitionDirection])) {
								state = GridSurferState.Disabled;
								StartCoroutine(currentSquare.sprites.DrawLineInDirection(transitionDirection, currentSquare, GridPuzzle.usedLines[currentSquare.line[(int)transitionDirection]]));//Draw visuals on the grid
								StartCoroutine("TransitionToNewSquare");
							}
						}
					}
					else {
						//We can just move around regularly
						if (currentSquare.neighbors[(int)transitionDirection] != null && currentSquare.socketState[(int)transitionDirection] != GridSquare.SocketState.None) {
							state = GridSurferState.Disabled;
							StartCoroutine("TransitionToNewSquare");
						}
					}
				}

			}
		}
	}


	private void DisableVisuals() {
		for (int i = 0; i < player.dataPanel.sections.Length; i++) {
			player.dataPanel.sections[i].gameObject.SetActive(false);
		}
	}


	private Vector3 posAboveSquare(GridSquare square) {
		return square.transform.position + square.transform.forward * -2.0f * square.puzzle.transform.localScale.x;
	}

	/// <summary>
	/// Move to a new gridsquare. It is assumed that the move is valid before this coroutine is called.
	/// </summary>
	/// <returns></returns>
	IEnumerator TransitionToNewSquare() {
		GridSquare newSquare = currentSquare.neighbors[(int)transitionDirection];
		Vector3 startPosition = player.playerCamera.transform.position;
		Vector3 goalPosition = posAboveSquare(newSquare);
		Quaternion startRotation = player.playerCamera.transform.rotation;
		//Debug.Log("Move " + currentSquare.name + " -> " + newSquare.name);


		float cTime = 0.0f;
		//The transition takes 2 seconds
		float maxTime = 0.4f;

		while (cTime < maxTime) {
			player.playerCamera.transform.position = Vector3.Slerp(startPosition, goalPosition, cTime / maxTime);
			player.playerCamera.transform.rotation = Quaternion.Slerp(startRotation, Quaternion.LookRotation(currentSquare.transform.forward), cTime / maxTime);
			cTime += Time.deltaTime;
			yield return null;
		}


		player.playerCamera.transform.position = goalPosition;
		player.playerCamera.transform.LookAt(newSquare.gameObject.transform);

		yield return new WaitForSeconds(0.05f);

		yield return null;
		currentSquare = newSquare;

		yield return null;
		state = GridSurferState.CanMove;
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


		state = GridSurferState.CanMove;
		yield return null;

		player.state = PlayerControls.PlayerState.GridInteraction;
	}


	IEnumerator TransitionToPlayer() {
		DisableVisuals();
		state = GridSurferState.Disabled;
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
