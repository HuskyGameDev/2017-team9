using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Puzzle {
	//This class provides a bunch of code for calculating the mathmatics of data beam visuals
	public class DataBeamVisual : MonoBehaviour {
		public static float BEAM_RADIUS = 0.05f;



		//This will calculate a 'circle' of beams locations
		public static List<Vector3> CalculateDataBeamCluster(int beamCount, Vector3 start, Vector3 end) {
			List<Vector3> calculations = new List<Vector3>();

			//Do nothing if we only have 1
			if (beamCount == 0)
				return calculations;

			//Our calculation breaks if there is only one, but we know it goes on the midpoint
			if (beamCount == 1) {
				calculations.Add(Vector3.Lerp(start, end, 0.5f));
				return calculations;
			}


			//So this will be created by solving the placement of two adjacent nodes and the center as if they are
			//a Triangle. For this description, imagine a downward facing triangle. The two points at the top are two
			//beams, and the point at the bottom is the center of this 'cluster' we are creating.
			//We can determine the inner angle (the angle at the center) by dividing 360 by the number of beams
			float innerAngle = ( 360.0f / beamCount);
			//We also now know the other two angles, they are half of the remaining degrees in a triangle
			float outerAngles = (180.0f - innerAngle) / 2.0f;

			//We need to radians from here on out
			innerAngle  *= Mathf.Deg2Rad;
			outerAngles *= Mathf.Deg2Rad;

			//Next we can calculate the distance between two beams, or the 'top' of the triangle
			//We want the beams to be touching, so their distance should be two times their radius
			float topDistance = BEAM_RADIUS * 2;

			//With this information we can used a formula i found online to calculate the missing length
			float length = (topDistance / Mathf.Sin(innerAngle)) * Mathf.Sin(outerAngles);

			//So now we know the distance one beam needs to be from the center. Now we need to calculate the actual positions.
			//First we find the midpoint between the start and end.

			Vector3 midpoint = Vector3.Lerp(start, end, 0.5f);

			//Now for each beam we place it around the midpoint
			for (int i = 0; i < beamCount; i++) {
				//We do this by treating the length and inner angle as polar coordinates 
				//and translating them to Cartesian coordinates
				float x = length * Mathf.Cos(innerAngle * i);
				float y = length * Mathf.Sin(innerAngle * i);

				//So this gives us the 2D offset vector we need from the midpoint. Now we need to orient this.
				//First we calculate the forward vector
				Vector3 forward = end - start;
				//Then we normalize it
				forward.Normalize();
				//Now we need to calcuate a rightward and upward vector from this.
				//We use these defaults for a special case
				Vector3 right = Vector3.right;
				Vector3 up = Vector3.forward;

				//We have a special case were the calculation method wont work, but now we know the fixed values
				//This if fires in the normal case and leaves the special case defaults otherwise
				if (forward != Vector3.up) {
					//We calculate these in the normal case
					right = Vector3.Cross(forward, Vector3.up);
					up = Vector3.Cross(right, forward);
					right.Normalize();
					up.Normalize();
				}
				//We have all the informatiojn to calculate the placement
				//We multiply our cartesian coordinates by their related directional vectors and add this offset to the midpoint
				Vector3 placement = midpoint + (x * right) + (y * up);
				calculations.Add(placement);
			}

			return calculations;
		}
	}
}
