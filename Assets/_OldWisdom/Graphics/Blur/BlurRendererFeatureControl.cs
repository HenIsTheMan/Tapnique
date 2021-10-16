using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace IWP.General {
    internal sealed class BlurRendererFeatureControl: MonoBehaviour {
        #region Fields

		[SerializeField]
		private ForwardRendererData forwardRendererData;

		private BlurRendererFeature blurRendererFeature;

		private bool isActiveOG;

		internal static BlurRendererFeatureControl globalObj;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal BlurRendererFeatureControl(): base() {
			forwardRendererData = null;

			blurRendererFeature = null;

			isActiveOG = false;
		}

        static BlurRendererFeatureControl() {
			globalObj = null;
        }

		#endregion

		#region Unity User Callback Event Funcs

		private void Awake() {
			globalObj = this;

			foreach(ScriptableRendererFeature scriptableRendererFeature in forwardRendererData.rendererFeatures) {
				if(scriptableRendererFeature.name == "Blur") {
					blurRendererFeature = (BlurRendererFeature)scriptableRendererFeature;
					isActiveOG = blurRendererFeature.isActive;
					return;
				}
			}

			UnityEngine.Assertions.Assert.IsTrue(false);
		}

		private void OnEnable() {
			blurRendererFeature.SetActive(false);
		}

		private void OnDisable() {
			blurRendererFeature.SetActive(isActiveOG);
		}

		#endregion

		internal void BlurRendererFeatureSetActive(bool active) {
			blurRendererFeature.SetActive(active);
		}
    }
}