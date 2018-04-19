using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllTheColorsOfTheWind;

public class EnableTrigger : ColorTrigger {
	public int numberBurstParticles = 6;

	public override void Trigger() {
		//Debug.Log("EnableTrigger Called: " + triggered);
		triggered = true;
		this.gameObject.SetActive(true);
		burstEmit();
	}
	public override void Untrigger() {
		//Debug.Log("EnableUntrigger Called: " + triggered);
		triggered = false;
		burstEmit();
		this.gameObject.SetActive(false);
	}

	public new void Awake() {
		createParticleSystems();
		if (!triggered) {
			this.gameObject.SetActive(false);
		}
	}
	
	public void burstEmit() {
		ParticleSystem[] parts = this.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < parts.Length; i++) {
			parts[i].Emit(numberBurstParticles);
		}
	}
}
