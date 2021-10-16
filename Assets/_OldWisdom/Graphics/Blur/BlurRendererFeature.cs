using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace IWP.General {
    internal sealed class BlurRendererFeature: ScriptableRendererFeature {
		#region Fields

		private BlurRenderPass renderPass;

		[SerializeField]
		private Shader shader;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal BlurRendererFeature(): base() {
			renderPass = null;
			shader = null;
        }

        static BlurRendererFeature() {
        }

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		public override void Create() {
			renderPass = new BlurRenderPass();
			renderPass.InitMe(shader);
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
			renderPass.CurrTarget = renderer.cameraColorTarget;
			renderer.EnqueuePass(renderPass);
		}

		protected override void Dispose(bool disposing) {
			base.Dispose(disposing);
		}
	}
}