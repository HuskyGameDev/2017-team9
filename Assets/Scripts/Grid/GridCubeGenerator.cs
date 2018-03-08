using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCubeGenerator : MonoBehaviour {

	public float scale = 1.0f; // We use scale in this instance so that grid surfer will properly reference when placing the camera
	public int squaresOnFace = 3;
	public GameObject cube;
	public GridPuzzle[] faces = new GridPuzzle[6];

	private static Vector3[] cubeDir = new Vector3[] {
		new Vector3(0.0f,0.0f,-1.0f), // Front
		new Vector3(0.0f,-1.0f,0.0f), //Down
		new Vector3(1.0f,0.0f,0.0f), //Right
		new Vector3(-1.0f,0.0f,0.0f), //Left
		new Vector3(0.0f,1.0f,0.0f), //Top
		new Vector3(0.0f,0.0f,1.0f), //Back
	};


	public void GenerateCube() {
		cube.transform.localScale = squaresOnFace * Vector3.one * scale;

		//scale all of them
		for (int i = 0; i < faces.Length; i++) {
			faces[i].transform.localScale = Vector3.one * scale;
		}


		//Position all of them
		for (int i = 0; i < faces.Length; i++) {
			faces[i].transform.localPosition = GetOffsetFromFaceNumber(i) * scale;
		}

		//Trigger all of them to generate.
		for (int i = 0; i < faces.Length; i++) {
			faces[i].GenerateGrid(""+(i+1), squaresOnFace, squaresOnFace);
		}
			

		//Connect all of them

		//These have been written by looking at a d6, subtract 1 if accessing the array
		//2 down -> 1 down
		//2 right -> 3 down
		//2 left -> 4 down
		//2 up -> 6 down

		//5 down -> 1 up
		//5 right -> 3 up
		//5 left -> 4 up
		//5 up -> 6 up

		//1 right -> 3 left
		//3 right -> 6 left
		//6 right -> 4 left
		//4 right -> 1 left

		ConnectFaceEdges(faces[2 - 1], GridSquare.GridDirection.Down,	faces[6 - 1], GridSquare.GridDirection.Down, true);
		ConnectFaceEdges(faces[2 - 1], GridSquare.GridDirection.Right,	faces[3 - 1], GridSquare.GridDirection.Down, true);//There is a small wrapping issue that is solved by reveresing this edge
		ConnectFaceEdges(faces[2 - 1], GridSquare.GridDirection.Left,	faces[4 - 1], GridSquare.GridDirection.Down);
		ConnectFaceEdges(faces[2 - 1], GridSquare.GridDirection.Up,		faces[1 - 1], GridSquare.GridDirection.Down);

		ConnectFaceEdges(faces[5 - 1], GridSquare.GridDirection.Down,	faces[1 - 1], GridSquare.GridDirection.Up);
		ConnectFaceEdges(faces[5 - 1], GridSquare.GridDirection.Right,	faces[3 - 1], GridSquare.GridDirection.Up);
		ConnectFaceEdges(faces[5 - 1], GridSquare.GridDirection.Left,	faces[4 - 1], GridSquare.GridDirection.Up, true);
		ConnectFaceEdges(faces[5 - 1], GridSquare.GridDirection.Up,		faces[6 - 1], GridSquare.GridDirection.Up, true);


		ConnectFaceEdges(faces[1 - 1], GridSquare.GridDirection.Right, faces[3 - 1], GridSquare.GridDirection.Left);
		ConnectFaceEdges(faces[3 - 1], GridSquare.GridDirection.Right, faces[6 - 1], GridSquare.GridDirection.Left);
		ConnectFaceEdges(faces[6 - 1], GridSquare.GridDirection.Right, faces[4 - 1], GridSquare.GridDirection.Left);
		ConnectFaceEdges(faces[4 - 1], GridSquare.GridDirection.Right, faces[1 - 1], GridSquare.GridDirection.Left);




		//Rebuild all of their visuals
		foreach (GridPuzzle f in faces)
			f.RebuildGrid();

		
	}

	private void ConnectFaceEdges(GridPuzzle A, GridSquare.GridDirection aDir, GridPuzzle B, GridSquare.GridDirection bDir, bool reverseA = false, bool reverseB = false) {
		GridSquare[] aEdge = A.GetEdge(aDir);
		GridSquare[] bEdge = B.GetEdge(bDir);

		if (reverseA)
			System.Array.Reverse(aEdge);

		if (reverseB)
			System.Array.Reverse(bEdge);

		for (int i = 0; i < aEdge.Length; i++) {
			aEdge[i].neighbors[(int)aDir] = bEdge[i];
			bEdge[i].neighbors[(int)bDir] = aEdge[i];
			aEdge[i].socketState[(int)aDir] = GridSquare.SocketState.Line;
			bEdge[i].socketState[(int)bDir] = GridSquare.SocketState.Line;
		}
	}


	private Vector3 GetOffsetFromFaceNumber(int i) {
		//Offset plus small number so that the grid is above the generated cube
		return (cubeDir[i] * squaresOnFace * 0.5f) + (cubeDir[i] * 0.001f);
	}

	public void ClearCube() {
		cube.transform.localScale = Vector3.zero;
		for (int i = 0; i < faces.Length; i++) {
			faces[i].transform.localPosition = Vector3.zero;
			faces[i].DestroyGrid();
		}
	}
}
