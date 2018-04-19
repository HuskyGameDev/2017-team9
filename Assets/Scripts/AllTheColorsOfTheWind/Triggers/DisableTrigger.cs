using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllTheColorsOfTheWind;

public class DisableTrigger : ColorTrigger {
	public int numberBurstParticles = 6;

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

	public void burstEmit() {
		ParticleSystem[] parts = this.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < parts.Length; i++) {
			parts[i].Emit(numberBurstParticles);
		}
	}
}
