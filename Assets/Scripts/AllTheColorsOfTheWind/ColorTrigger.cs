using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllTheColorsOfTheWind {
	public abstract class ColorTrigger : MonoBehaviour {
		#region Properties
		/// <summary>
		/// The color on which the trigger method is called.
		/// </summary>
		public ColorBit triggerColor;
		public bool puzzleCompleteNoise;
		protected bool triggered = false;
		public bool particles = true;
		#endregion

		#region Methods
		#region Public
		/// <summary>
		/// Checks if the input color and calls Trigger if they match
		/// </summary>
		/// <param name="color"></param>
		public bool Check(ColorBit color) {
			Debug.Log("Check Called | " + color + " | " + triggerColor);
			if (triggerColor.Equals(color)) {
				if (puzzleCompleteNoise) {
					Debug.Log("Puzzle Complete nouise");
					AkSoundEngine.PostEvent("PuzzleComplete", PlayerControls.instance.gameObject);
				}
				Trigger();
				//triggered = true;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Untriggers script if it has not already been untriggered. 
		/// Returns true if the script was not already untriggered
		/// </summary>
		/// <returns>true if the script was not already untriggered</returns>
		public bool CanUntrigger() {
			//Debug.Log("CanUnTrigger Called: " + triggered);
			if (triggered) {
				Untrigger();
				//triggered = false;
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Triggers script if it has not already been triggered. 
		/// Returns true if the script was not already triggered
		/// </summary>
		/// <returns>true if the script was not already triggered</returns>
		public bool CanTrigger() {
			//Debug.Log("CanTrigger Called: " + !triggered);
			if (triggered) {
				return false;
			} else {
				Trigger();
				//triggered = true;
				return true;
				
			}
		}

		/// <summary>
		/// Tests if internal variable triggered is true
		/// </summary>
		/// <returns>true if triggered is true, else false</returns>
		public bool GetTriggered() {
			return triggered;
		}

		/// <summary>
		/// Sets internal variable triggered to given boolean
		/// </summary>
		/// <returns>true if triggered is true, else false</returns>
		public void SetTriggered(bool b) {
			triggered = b;
		}

		
		public void Awake() {
			createParticleSystems();
		}

		public void createParticleSystems() {
			if (particles) {
				GameObject target = this.gameObject;
				ParticleSystem part_rect = Instantiate(Resources.Load<ParticleSystem>("Particles/PartSys_Rectangle"));
				part_rect.transform.parent = target.transform;
				part_rect.transform.localPosition = new Vector3(0, 0, 0);
				part_rect.transform.localScale = new Vector3(1, 1, 1);
				ParticleSystem.ShapeModule shapeMod = part_rect.shape;
				shapeMod.scale = target.transform.lossyScale + new Vector3(0.1f, 0.1f, 0.1f);
				ParticleSystem.MainModule mainMod = part_rect.main;
				float red = triggerColor.color.r;
				float green = triggerColor.color.g;
				float blue = triggerColor.color.b;
				mainMod.startColor = new Color(red / 255, green / 255, blue / 255);

				ParticleSystem part_circ = Instantiate(Resources.Load<ParticleSystem>("Particles/PartSys_Circle"));
				part_circ.transform.parent = target.transform;
				part_circ.transform.localPosition = new Vector3(0, 0, 0);
				part_circ.transform.localScale = new Vector3(1, 1, 1);
				shapeMod = part_circ.shape;
				shapeMod.scale = target.transform.lossyScale + new Vector3(0.1f, 0.1f, 0.1f);
				mainMod = part_circ.main;
				mainMod.startColor = new Color(red / 255, green / 255, blue / 255);
			}
		}
		#endregion

		#region Abstract
		/// <summary>
		/// Code for what happens when the colors match.
		/// </summary>
		public abstract void Trigger();

		/// <summary>
		/// Undoes whatever Trigger does, if applicable.
		/// </summary>
		public abstract void Untrigger();
		#endregion
		#endregion
	}
}