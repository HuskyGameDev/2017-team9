using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	/// Array signifying that there is supposed to be a connection between the adjacent squares.
	/// </summary>
	public bool[] connected = new bool[4];


	/// <summary>
	/// The GridLine that is on this square.
	/// </summary>
	public GridLine[] line = new GridLine[4];


	/// <summary>
	/// Marks a connection as connected between this square and the other one. Takes into account the types of connections of components
	/// </summary>
	/// <param name="direction"> The Direction from A to B</param>
	/// <param name="A">The first square</param>
	/// <param name="B">the Second Square</param>
	/// <returns>Returns true if the connection was sucessful</returns>
	public static bool Connect(GridDirection direction, GridSquare A, GridSquare B) {

		throw new System.NotImplementedException("This is not finished yet!");

		if (A.neighbors[(int)direction] == null || A.neighbors[(int)direction] == null) {
			return false;
		}

		if (A.type == GridType.Unusable || B.type == GridType.Unusable) {
			//Cant make a connection with an unusable section
			return false;
		}

		//Before we go on, we need to make sure that a connection is possible on the component
		if (A.type != GridType.Empty) {

		}



		if (A.type != GridType.Empty) {
			//Here we are a special type, so we allow all connections based on the component.

		}
		else {
			//Empty squares can only be part of two connections if then are across from eachother
			//We can enforce this by setting the two cross connections equal to false.
			A.connected[(int)crossDirections[(int)direction][0]] = false;
			A.connected[(int)crossDirections[(int)direction][1]] = false;

			//Make the connection
			A.connected[(int)direction] = true;
		}

		if (B.type != GridType.Empty) {
			//Here we are a special type, so we allow all connections based on the component
		}
		else {
			//Empty squares can only be part of two connections if then are across from eachother
			//We can enforce this by setting the two cross connections equal to false.
			//We do not have to access opposite direction here because opposite direction will have the same cross direction
			B.connected[(int)crossDirections[(int)direction][0]] = false;
			B.connected[(int)crossDirections[(int)direction][1]] = false;

			//Make the connection
			B.connected[(int)direction] = true;

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
			}
		}
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
