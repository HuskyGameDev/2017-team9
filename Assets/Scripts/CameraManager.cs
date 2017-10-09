using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	//Culling masks for rendering only certain parts of the game. Calculated by manually selecting and logging mask.
	private static int _DefaultOnlyMask = -1825;
	private static int _TrueMask = -1569;
	private static int _VirtualMask = -1313;
	private static int _TotalMask = -1057;
	private static int _TrueOnly = 256;
	private static int _FalseOnly = 512;
	private static int _FalseAndTrueOnly = 768;

	private static int[] maskArray = { _DefaultOnlyMask, _TrueMask, _VirtualMask, _TotalMask, _TrueOnly, _FalseOnly, _FalseAndTrueOnly};
	public enum MaskMode { DefaultOnly=0, True=1, False=2, Total=3, TrueOnly=4, FalseOnly=5, NotDefault=5}

	private int cMask = 0;
	private void Update() {

	}

	public void NextRender() {
		cMask++;
		if (cMask > 6) cMask = 0;
		ChangeMask((MaskMode)cMask);
	}

	public void ChangeMask(MaskMode mask) {
		this.gameObject.GetComponent<Camera>().cullingMask = maskArray[(int)mask];
	}
}
