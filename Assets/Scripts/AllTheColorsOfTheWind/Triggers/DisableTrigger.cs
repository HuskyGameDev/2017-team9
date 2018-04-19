using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllTheColorsOfTheWind;

public class DisableTrigger : ColorTrigger {
	public int numberBurstParticles = 6;
	private ParticleSystem[] particleSystems;

	public override void Trigger() {
		triggered = true;
		burstEmit();
		this.gameObject.SetActive(false);
	}
	public override void Untrigger() {
		triggered = false;
		this.gameObject.SetActive(true);
		burstEmit();
	}

	public new void Awake() {
		createParticleSystems();
		particleSystems = this.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < particleSystems.Length; i++) {
			particleSystems[i].transform.parent = this.gameObject.transform.parent;
			//particleSystems[i].transform.localPosition = this.gameObject.transform.localPosition;
			particleSystems[i].transform.localScale = new Vector3(1, 1, 1);
		}
	}

	public void burstEmit() {
		//ParticleSystem[] parts = this.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < particleSystems.Length; i++) {
			particleSystems[i].Emit(numberBurstParticles);
		}
	}
}
