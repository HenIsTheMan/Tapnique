using UnityEngine;

namespace Genesis.Creation.IslandCompleteScreen {
	internal sealed class CargoShip: MonoBehaviour {
		[SerializeField]
		private ParticleSystem smokeParticleSystem;

		[SerializeField]
		private LayerMask myLayerMask;

		private ParticleSystem.EmitParams emitParams;

		private void Awake() {
			emitParams = new ParticleSystem.EmitParams();
		}

		private void OnCollisionEnter(Collision collision) {
			if((myLayerMask.value & (1 << collision.gameObject.layer)) != 0) {
				smokeParticleSystem.Emit(emitParams, 14);
			}
		}
	}
}