using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class SkyboxRotator: MonoBehaviour {
		internal bool ShldUseUnscaled {
			get => shldUseUnscaled;
			set {
				shldUseUnscaled = value;
				ModifyMyUpdateDelegate();
			}
		}

		private float rotationFactor;

		private delegate void UpdateDelegate();

		private UpdateDelegate myUpdateDelegate;

		[SerializeField]
		private bool shldUseUnscaled;

		[SerializeField]
		private float rotationVel;

		[SerializeField]
		private Material skyboxMtl;

		private void OnValidate() {
			if(Application.isPlaying) {
				ModifyMyUpdateDelegate();
			}
		}

		private void Update() {
			myUpdateDelegate.Invoke();
		}

		private void ScaledUpdate() {
			rotationFactor += Time.deltaTime * rotationVel;
			skyboxMtl.SetFloat("_Rotation", rotationFactor);
		}

		private void UnscaledUpdate() {
			rotationFactor += Time.unscaledDeltaTime * rotationVel;
			skyboxMtl.SetFloat("_Rotation", rotationFactor);
		}

		private void OnDisable() {
			skyboxMtl.SetFloat("_Rotation", 0.0f);
		}

		private void ModifyMyUpdateDelegate() {
			if(shldUseUnscaled) {
				myUpdateDelegate = UnscaledUpdate;
			} else {
				myUpdateDelegate = ScaledUpdate;
			}
		}
	}
}