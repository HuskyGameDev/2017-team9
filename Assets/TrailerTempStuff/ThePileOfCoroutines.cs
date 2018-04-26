using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePileOfCoroutines : MonoBehaviour {
	public Color[] blockColor;
	public Light[] lights;
	public Light underLight;
	public PointFollower cam;
	public Color titleColor;

	public GameObject Enviroment;
	public GameObject LineHodler;
	public GridSquare[] square;

	private bool disabled = false;



	// Use this for initialization
	void Start () {
	}

	private void Update() {
		if (!disabled && Input.GetKeyDown(KeyCode.Space)) {
			disabled = true;
			StartCoroutine(DoTheThing());
		}
	}

	IEnumerator DrawR1() {
		yield return new WaitForSeconds(2.7f);
		yield return StartCoroutine(AdvanceLine(3, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(3, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(3, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(3, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(3, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(3, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(3, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(3, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(3, titleColor, GridSquare.GridDirection.Left));
		yield return StartCoroutine(AdvanceLine(3, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(3, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(3, titleColor, GridSquare.GridDirection.Down));


	}


	IEnumerator DrawE2() {
		square[5] = square[4];
		StartCoroutine(AdvanceLine(5, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(4, titleColor, GridSquare.GridDirection.Up));
		StartCoroutine(AdvanceLine(5, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(4, titleColor, GridSquare.GridDirection.Up));
		square[6] = square[4];
		StartCoroutine(AdvanceLine(6, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(4, titleColor, GridSquare.GridDirection.Up));
		StartCoroutine(AdvanceLine(6, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(4, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(4, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(4, titleColor, GridSquare.GridDirection.Right));
	}


	IEnumerator DrawC3() {
		yield return new WaitForSeconds(0.3f);
		square[8] = square[7];
		StartCoroutine(AdvanceLine(8, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(7, titleColor, GridSquare.GridDirection.Up));
		StartCoroutine(AdvanceLine(8, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(7, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(7, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(7, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(7, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(7, titleColor, GridSquare.GridDirection.Right));
	}


	IEnumerator DrawO4(GridSquare cSquare) {
		square[9] = cSquare;
		yield return StartCoroutine(AdvanceLine(9, blockColor[0], GridSquare.GridDirection.Left));
		square[10] = square[9];
		StartCoroutine(AdvanceLine(10, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(9, titleColor, GridSquare.GridDirection.Up));
		StartCoroutine(AdvanceLine(10, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(9, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(9, titleColor, GridSquare.GridDirection.Left));
		yield return StartCoroutine(AdvanceLine(9, titleColor, GridSquare.GridDirection.Left));
		yield return StartCoroutine(AdvanceLine(9, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(9, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(9, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(9, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(9, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(9, titleColor, GridSquare.GridDirection.Right));

	}


	IEnumerator DrawL5() {
		yield return StartCoroutine(AdvanceLine(11, titleColor, GridSquare.GridDirection.Left));
		yield return StartCoroutine(AdvanceLine(11, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(11, titleColor, GridSquare.GridDirection.Up));
		yield return new WaitForSeconds(2.76f);
		StartCoroutine(DrawO4(square[11]));
		yield return StartCoroutine(AdvanceLine(11, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(11, titleColor, GridSquare.GridDirection.Up));
	}


	IEnumerator DrawO6() {
		yield return new WaitForSeconds(0.2f);
		square[14] = square[13];
		StartCoroutine(AdvanceLine(14, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(13, titleColor, GridSquare.GridDirection.Up));
		StartCoroutine(AdvanceLine(14, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(13, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(13, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(13, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(13, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(13, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(13, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(13, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(13, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(13, titleColor, GridSquare.GridDirection.Down));
	}


	IEnumerator DrawR7() {
		square[21] = square[15];
		StartCoroutine(AdvanceLine(21, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(15, titleColor, GridSquare.GridDirection.Left));
		StartCoroutine(AdvanceLine(21, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(15, titleColor, GridSquare.GridDirection.Left));
		StartCoroutine(AdvanceLine(21, titleColor, GridSquare.GridDirection.Left));
		yield return StartCoroutine(AdvanceLine(15, titleColor, GridSquare.GridDirection.Down));
		StartCoroutine(AdvanceLine(21, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(15, titleColor, GridSquare.GridDirection.Down));
		StartCoroutine(AdvanceLine(21, titleColor, GridSquare.GridDirection.Right));

		yield return StartCoroutine(AdvanceLine(15, titleColor, GridSquare.GridDirection.Down));
		StartCoroutine(AdvanceLine(21, titleColor, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(15, titleColor, GridSquare.GridDirection.Down));
	}


	IEnumerator DrawE8() {
		square[17] = square[16];
		StartCoroutine(AdvanceLine(17, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(16, titleColor, GridSquare.GridDirection.Left));
		yield return StartCoroutine(AdvanceLine(16, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(16, titleColor, GridSquare.GridDirection.Up));
		StartCoroutine(DrawD9());
		square[18] = square[16];
		StartCoroutine(AdvanceLine(18, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(16, titleColor, GridSquare.GridDirection.Up));
		StartCoroutine(AdvanceLine(18, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(16, titleColor, GridSquare.GridDirection.Up));
		square[21] = square[16];
		StartCoroutine(AdvanceLine(21, blockColor[0], GridSquare.GridDirection.Left));
		yield return StartCoroutine(AdvanceLine(16, titleColor, GridSquare.GridDirection.Right));
		StartCoroutine(DrawR7());
		yield return StartCoroutine(AdvanceLine(16, titleColor, GridSquare.GridDirection.Right));
	}


	IEnumerator DrawD9() {
		square[20] = square[19];
		StartCoroutine(AdvanceLine(20, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(19, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(19, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(19, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(19, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(19, titleColor, GridSquare.GridDirection.Right));
		StartCoroutine(AdvanceLine(20, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(19, titleColor, GridSquare.GridDirection.Down));
		StartCoroutine(AdvanceLine(20, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(19, titleColor, GridSquare.GridDirection.Right));
		StartCoroutine(AdvanceLine(20, titleColor, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(19, titleColor, GridSquare.GridDirection.Down));




	}

	IEnumerator LineDrawer1() {

		yield return new WaitForSeconds(3.5f);
		yield return StartCoroutine(AdvanceLine(0, Color.magenta, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(0, Color.magenta, GridSquare.GridDirection.Down));
		square[1] = square[0];
		StartCoroutine(AdvanceLine(1, Color.yellow, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, Color.cyan, GridSquare.GridDirection.Down));
		StartCoroutine(AdvanceLine(1, Color.yellow, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, Color.cyan, GridSquare.GridDirection.Down));
		StartCoroutine(AdvanceLine(1, Color.yellow, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, Color.cyan, GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(0, Color.cyan, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, Color.cyan, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, Color.cyan, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, Color.cyan, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(0, Color.cyan, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(0, Color.cyan, GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(0, blockColor[0], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[0], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[0], GridSquare.GridDirection.Right));

		square[17] = square[0];
		StartCoroutine(AdvanceLine(17, blockColor[0], GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(0, blockColor[0], GridSquare.GridDirection.Right));
		StartCoroutine(DrawE2());
		yield return StartCoroutine(AdvanceLine(0, blockColor[0], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[0], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[0], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[0], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[0], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[0], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[0], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[0], GridSquare.GridDirection.Up));
		StartCoroutine(DrawL5());
		//StartCoroutine(DrawR1());
		yield return StartCoroutine(AdvanceLine(0, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Down));

		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Down));

		StartCoroutine(AdvanceLine(2, blockColor[0], GridSquare.GridDirection.Up));
		StartCoroutine(DrawO6());//
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Down));
		StartCoroutine(AdvanceLine(2, titleColor, GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Left));
		StartCoroutine(AdvanceLine(2, blockColor[2], GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Up));
		StartCoroutine(AdvanceLine(2, blockColor[2], GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(0, blockColor[1], GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(2, blockColor[2], GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(2, blockColor[2], GridSquare.GridDirection.Down));
		yield return StartCoroutine(AdvanceLine(2, blockColor[2], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(2, blockColor[2], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(2, blockColor[2], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(2, blockColor[2], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(2, blockColor[2], GridSquare.GridDirection.Right));
		yield return StartCoroutine(AdvanceLine(2, blockColor[2], GridSquare.GridDirection.Up));
		StartCoroutine(DrawC3());
		yield return StartCoroutine(AdvanceLine(2, blockColor[2], GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(2, blockColor[2], GridSquare.GridDirection.Up));
		yield return StartCoroutine(AdvanceLine(2, blockColor[2], GridSquare.GridDirection.Up));

		StartCoroutine(DrawE8());
		StartCoroutine(DrawR1());
	}


	IEnumerator AdvanceLine(int s, Color color, GridSquare.GridDirection dir) {
		square[s].sprites.lines[(int)dir].color = color;
		square[s].neighbors[(int)dir].sprites.lines[(int)GridSquare.oppositeDirection[(int) dir]].color = color;

		yield return square[s].sprites.StartCoroutine(square[s].sprites.DrawLineInDirection(dir, square[s]));
		square[s] = square[s].neighbors[(int)dir];
	}

	IEnumerator DoTheThing() {
		StartCoroutine(LineDrawer1());
		float tolerance = 0.02f;
		Debug.Log("Bring up the lights");
		yield return StartCoroutine(ModifyLightLevels(lights, 0.0f, 0.8f, 2.0f));
		Debug.Log("Wait before moving");
		yield return new WaitForSeconds(2.0f);


		//Rotate the camera to face a new direction.
		StartCoroutine(RotateObjects(cam.cam.transform, Quaternion.Euler(20.0f, 90.0f, 0.0f), 2.0f));
		Debug.Log("Back away from wall with source.");
		yield return StartCoroutine(NextPoint(tolerance));

		Debug.Log("Slide to front of first screen");
		yield return StartCoroutine(NextPoint(tolerance));
		//Rotate the camera to face a new direction.
		StartCoroutine(RotateObjects(cam.cam.transform, Quaternion.Euler(20.0f, 90.0f, 0.0f), 3.0f));

		Debug.Log("Wait for clip");
		//yield return new WaitForSeconds(3.0f);

		Debug.Log("Slide to second monitor");
		yield return StartCoroutine(NextPoint(tolerance));

		Debug.Log("Wait for second clop");
		//yield return new WaitForSeconds(5.0f);

		//Rotate the camera to face a new direction.
		StartCoroutine(RotateObjects(cam.cam.transform, Quaternion.Euler(0.0f,0.0f,0.0f), 3.0f ));
		Debug.Log("Move diagnol towards wall");
		yield return StartCoroutine(NextPoint(tolerance));

		Debug.Log("Move down wall");
		yield return StartCoroutine(NextPoint(tolerance));

		Debug.Log("Slide along to chair");
		yield return StartCoroutine(NextPoint(tolerance));
		//Rotate back
		StartCoroutine(RotateObjects(cam.cam.transform, Quaternion.Euler(0.0f, 90.0f, 0.0f), 5.0f));

		Debug.Log("Slide under table");
		yield return StartCoroutine(NextPoint(tolerance));

		Debug.Log("Back out from wall while under table");
		yield return StartCoroutine(NextPoint(tolerance));

		StartCoroutine(RotateObjects(cam.cam.transform, Quaternion.Euler(0.0f, 65.0f, 0.0f), 5.0f));

		Debug.Log("Move back a bit to float over empty space");
		yield return StartCoroutine(NextPoint(tolerance));

		StartCoroutine(RotateObjects(cam.cam.transform, Quaternion.Euler(0.0f, 90.0f, 0.0f), 8.6f));
		Debug.Log("Move towards the final view");
		yield return StartCoroutine(NextPoint(tolerance));


		//Let the viewer see the final product
		yield return new WaitForSeconds(1.0f);

		Debug.Log("Bring down the lights");
		StartCoroutine(ModifyLightLevels(new Light[] { underLight}, 0.05f, 0.0f, 2.0f));
		yield return StartCoroutine(ModifyLightLevels(lights, 0.8f, 0.0f, 2.0f));
		yield return new WaitForSeconds(2.0f);
		//StartCoroutine(DimSprits(10.0f, 0.35f));

		yield return new WaitForSeconds(3.0f);
		StartCoroutine(DimSprits(10.0f, 1f));
		Enviroment.SetActive(false);
	}

	IEnumerator DimSprits(float maxTime, float speed) {
		float timer = 0.0000001f;
		while (timer <= maxTime) {
			timer += Time.deltaTime;
			yield return null;
			foreach (SpriteRenderer s in LineHodler.GetComponentsInChildren<SpriteRenderer>()) {
				if ((s.color != titleColor) || s.gameObject.name == "OutputArrows" || s.gameObject.name == "InputArrows") // Fade it out if it is not a primary color
					s.color = new Color(s.color.r,s.color.g,s.color.b, (s.color.a - (speed * Time.deltaTime)));
			}
		}
	}
	IEnumerator NextPoint(float tolerance) {
		cam.MoveToNextPoint();
		yield return null;
		while ((cam.currentPoint.transform.position - cam.cam.transform.position).magnitude >= tolerance) yield return null;
	}

	IEnumerator ModifyLightLevels(Light[] Lights, float lightLow, float lightHigh, float maxTime) {
		float timer = 0.0000001f;
		while (timer <= maxTime) {
			timer += Time.deltaTime;
			yield return null;
			foreach( Light l in Lights) {
				l.intensity = Mathf.Lerp(lightLow, lightHigh, (timer/maxTime));
			}
		}
		yield return null;
		foreach (Light l in Lights) {
			l.intensity = lightHigh;
		}
	}
	IEnumerator RotateObjects(Transform trans, Quaternion r, float maxTime) {
		float timer = 0.0000001f;
		Quaternion startRot = trans.rotation;
		while (timer <= maxTime) {
			timer += Time.deltaTime;
			yield return null;
			trans.rotation = Quaternion.Slerp(startRot, r, (timer / maxTime));
		}
	}
	/// <summary>
	/// This one might not work correctly
	/// </summary>
	/// <param name="trans"></param>
	/// <param name="disp"></param>
	/// <param name="maxTime"></param>
	/// <returns></returns>
	IEnumerator MoveObjects(Transform trans, Vector3 disp, float maxTime) {
		float timer = 0.0000001f;
		Vector3 startpos = trans.transform.position;
		while (timer <= maxTime) {
			timer += Time.deltaTime;
			yield return null;
			trans.position = Vector3.Lerp(startpos,startpos + disp, (timer / maxTime));
		}
	}
}
