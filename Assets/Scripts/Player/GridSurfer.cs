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

						{
							//Failcase: we just cant move in that direction.
							if (currentSquare.CanConnect(transitionDirection) == false) {
								failed = true;
							}

							//Failcase: We can only move on connections that exist
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
								//Failcase: we do not have a neighbor in that direction.
								failed = true;
							}
							else {
								//Failcase: We are moving onto a component not through an input/output socket. 
								GridSquare.SocketState othersSocket = other.socketState[(int)GridSquare.oppositeDirection[(int)transitionDirection]];
								if (other.type != GridSquare.GridType.Empty && (othersSocket != GridSquare.SocketState.Input && othersSocket != GridSquare.SocketState.Output)) {
									Debug.LogWarning("Failed by trying to move onto a component through not an input or output");
									failed = true;
								}
							}




							//Check if there is a line on here
							if (currentSquare.type == GridSquare.GridType.Empty) {
								if (currentSquare.lines.Count == 0) {
									//Failcase: We are trying to start drawing on an empty square with no lines already here.
									Debug.LogWarning("Failed move by there being no line on this empty socket.");
									failed = true;
								}
								else {
									//So there is a line here, so lets get the last direction that line moved in.
									if (currentSquare.lines[0].lineNodes.Count < 2)
										//Failcase: we cannot back down a line of size 1.
										failed = true;
									else {
										if (currentSquare.type == GridSquare.GridType.Empty) {
											//Calculate the previously traveled direction by using the Are Neighbors function. We pass in the node previous on the line and the current square
											GridSquare.GridDirection prevDir;
											GridSquare.AreNeighbors(currentSquare.lines[0].lineNodes.Find(currentSquare).Previous.Value, currentSquare, out prevDir);

											if (currentSquare.lines[0].IsTip(currentSquare) == true && GridSquare.oppositeDirection[(int)prevDir] == transitionDirection) {
												//Special Case: we are backing down a line. (our movement is the opposite of the last movement we did)
												failed = true; //Mark failed as true so we do not follow the normal movement
												Debug.LogWarning("Specialcase: Backing down a line");
												currentSquare.lines[0].Pop();
												state = GridSurferState.Disabled;
												StartCoroutine("TransitionToNewSquare");
												//If this line is size one or less we need to get rid of it now.
												if (currentSquare.lines[0].lineNodes.Count < 2)
													currentSquare.lines[0].DestroyLine();
											}
										}
									}
								}
							}
						}
						//Only continue if we have not found a failcase. The movement should now be 100% correct to do.
						if (failed == false) {
							//Finnally, if we can move in that drection.

							GridLine line;
							if (currentSquare.type != GridSquare.GridType.Empty) {
								//If we are not empty, that means we may need to create a line
								//If the line exists then we need to do something. I am unsure on what so for now we are destorying it.
								//We loop backwards here so that if we remove a line we do not desync from the iteration
								for (int i = currentSquare.lines.Count - 1; i >= 0; i--) {
									GridLine checkLine = currentSquare.lines[i];
									//If us and our neighbor are on the next line
									//[TODO] I think the logic of these next two statements needs to be cleaned up
									if (checkLine.lineNodes.Count < 1) {
										Debug.Log("Destrpyed an Orphan Line.");
										checkLine.DestroyLine();
									}
									if (checkLine.lineNodes.Contains(currentSquare) && checkLine.lineNodes.Contains(currentSquare.neighbors[(int)transitionDirection]) ) {
										//if this line leads directly to that node.
										if (checkLine.lineNodes.Find(currentSquare).Next != null && checkLine.lineNodes.Find(currentSquare).Next.Value == currentSquare.neighbors[(int)transitionDirection]) {
											//then what?
											Debug.Log("Found a line to destroy");
											checkLine.DestroyLine();
										}
									}
								}
								Debug.Log("Created new line");
								//otherwise we need to make a new one
								line = new GridLine();
								currentSquare.lines = new List<GridLine>();
								currentSquare.lines.Add(line);
								//Add this current node to this new line since it starts here.
								line.Add(currentSquare);
							}
							else {
								Debug.Log("using old line");
								line = currentSquare.lines[0];
							}

							Debug.Log(currentSquare.gameObject.name + "->" + currentSquare.neighbors[(int)transitionDirection].gameObject.name);
							line.Add(currentSquare.neighbors[(int)transitionDirection]);
							state = GridSurferState.Disabled;
							StartCoroutine("TransitionToNewSquare");
						}
						else {
							//[TODO] Make an Anim for a failed move
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


	private Vector3 posAboveSquare(GridSquare square) {
		return square.transform.position + square.transform.forward * -3.0f * square.puzzle.transform.localScale.x;
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
		currentSquare = null;
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
