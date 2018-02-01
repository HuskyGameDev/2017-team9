using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalInteraction : MonoBehaviour {

	//Tracks the last GridSquare the mouse was over for TerminalInteraction Method
	public GridSquare lastGridSquare = null;
	//Tracks the current line TerminalInteraction is working on.
	public GridLine currentLine = null;

	/// <summary>
	/// Handles the player intraction with a terminal.
	/// </summary>
	public void Interact() {
		//Shoot a ray so we know what we are hovering over this frame
		RaycastHit rayInfo;

		//Shoot a ray depending on weather or not the camera is locked.
		if (InputManager.GetGameButton(InputManager.GameButton.CameraLock) == false) 
			Physics.Raycast(PlayerControls.instance.playerCamera.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f)), out rayInfo, PlayerControls.instance.ignoreMask);
		else
			Physics.Raycast(PlayerControls.instance.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out rayInfo, PlayerControls.instance.ignoreMask);

		if (InputManager.GetGameButtonDown(InputManager.GameButton.Interact1)) {

			if (rayInfo.transform != null && rayInfo.transform.gameObject.tag == "GridSocket") {

				if (currentLine != null)
					Debug.LogError("CurrentLine is nonNull when it should be!");

				Debug.Log("Down on Socket");

				GridSocket socket = rayInfo.transform.gameObject.GetComponent<GridSocket>();
				if (socket != null) {
					//So we have clicked down on a socket.
					//First we need to delete the previous line if it existed
					if (socket.gridSquare.line[(int)socket.direction] != null)
						socket.gridSquare.line[(int)socket.direction].DeleteFromGrid();

					currentLine = new GridLine();
					currentLine.AddSquare(socket.gridSquare);
					//Implicitly, we know that sockets only exist on gridSquares that are components, so we know that component has to exist
					currentLine.AddDataComponent(socket.gridSquare.dataComponent);
				}
			}
		}
		else if (InputManager.GetGameButton(InputManager.GameButton.Interact1)) {
			//Interact1 stayDown
			//Try and create connections across lastGridSquare and Current
			//We can create a connection if we are hovering over a GridSocket or a GridSquare
			if (currentLine != null && rayInfo.transform != null && (rayInfo.transform.gameObject.tag == "GridSocket" || rayInfo.transform.gameObject.tag == "GridSquare")) {
				//If the line has been destroyed for some reason, we need to ditch it.
				if (currentLine.DeletionFlag == true) {
					currentLine = null;
				}
				else {
					//Get the proper square depending on what we were hovering over.
					GridSquare square = null;
					if (rayInfo.transform.gameObject.tag == "GridSocket")
						square = rayInfo.transform.gameObject.GetComponent<GridSocket>().gridSquare;
					else
						square = rayInfo.transform.gameObject.GetComponent<GridSquare>();

					//If we have a last GridSquare and it is different
					if (lastGridSquare != null && lastGridSquare != square) {

						Debug.Log("We need to make a connection!");

						//Then we try and make a connection.
						//We can only do this if they are neighbors, both can connect, and the socket types are not equal (implicilty not 'none')
						GridSquare.GridDirection dir;
						if (GridSquare.AreNeighbors(lastGridSquare, square, out dir)
							&& lastGridSquare.CanConnect(dir)
							&& square.CanConnect(GridSquare.oppositeDirection[(int)dir])) {

							Debug.Log(lastGridSquare.transform.name + "->" + square.transform.name);

							GridSquare.Connect(dir, lastGridSquare, square, currentLine);

							lastGridSquare.gameObject.GetComponent<GridSquareVisuals>().UpdateVisuals();
							square.gameObject.GetComponent<GridSquareVisuals>().UpdateVisuals();
							Debug.Log((lastGridSquare.line[0] != null) + "|" + (lastGridSquare.line[1] != null) + "|" + (lastGridSquare.line[2] != null) + "|" + (lastGridSquare.line[3] != null));

							currentLine.AddSquare(square);
						}
						else {
							//This means we have jumped ship for some reason.
							currentLine.DeleteFromGrid();
						}
					}
					lastGridSquare = square;
				}
			}
		}


		if (InputManager.GetGameButtonUp(InputManager.GameButton.Interact1)) {
			//Interact1 Up, This needs to be done outside of a gridsquare check
			//Check if the ray hit a Socket
			//Finish Line if so, 
			//otherwise delete line (remove it and make sure it is stripped from )

			if (rayInfo.transform != null && rayInfo.transform.gameObject.tag == "GridSocket" && currentLine != null) {

				Debug.Log("Up on Socket");

				//We assume the proper connection has already been made since that is handled in the gameButtonStayDown section of the interaction.
				//So we can just add the component and forget about the line.
				currentLine.AddDataComponent(rayInfo.transform.gameObject.GetComponent<GridSocket>().gridSquare.dataComponent);
				//We need to make sure this is a good connection, if it is not we need to ditch it.
				if (currentLine.ValidatePathBetweenDataComponents() == false)
					//If we cannot prove a good line, delete it from the grid
					currentLine.DeleteFromGrid();
				else {
					//Otherwise notify the lines that a connection changes has occured.
					currentLine.A.ConnectionChange();
					currentLine.B.ConnectionChange();
				}

				currentLine = null;
			}
			else {
				Debug.Log("Up Elsewhere");
				//The player has released the interaction key without completing a proper line, so we destroy the line
				if (currentLine != null)
					currentLine.DeleteFromGrid();
				currentLine = null;
			}
		}

		//This handles the UI
		if (rayInfo.transform != null && (rayInfo.transform.gameObject.tag == "GridSocket" || rayInfo.transform.gameObject.tag == "GridSquare")) {
			GridSquare square = null;
			if (rayInfo.transform.gameObject.tag == "GridSocket")
				square = rayInfo.transform.gameObject.GetComponent<GridSocket>().gridSquare;
			else
				square = rayInfo.transform.gameObject.GetComponent<GridSquare>();

			if (square.dataComponent != null) {
				//Show the panel
				PlayerControls.instance.PlayerUI.wholePanel.gameObject.SetActive(true);
				PlayerControls.instance.PlayerUI.type.text = square.dataComponent.GetString();
				PlayerControls.instance.PlayerUI.input.text = "Looking at this here is not Implemented Yet :(";// square.dataComponent.GetInput();
				PlayerControls.instance.PlayerUI.output.text = square.dataComponent.GetOutputString();
				PlayerControls.instance.PlayerUI.trigger.text = "Goal:";
				foreach (PuzzleComponents.DataTrigger dt in square.gameObject.GetComponents<PuzzleComponents.DataTrigger>()) {
					if (dt.playerVisibleTrigger) {
						PlayerControls.instance.PlayerUI.trigger.text = PlayerControls.instance.PlayerUI.trigger.text + " " + dt.triggerData.ToString();
					}
				}
			}
			else {
				//Hide the panel
				PlayerControls.instance.PlayerUI.wholePanel.gameObject.SetActive(false);
			}
		}
		else {
			//Hide the panel
			PlayerControls.instance.PlayerUI.wholePanel.gameObject.SetActive(false);
		}
	}
}
