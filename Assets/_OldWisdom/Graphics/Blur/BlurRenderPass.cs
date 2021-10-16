using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace IWP.General {
    internal sealed class BlurRenderPass: ScriptableRenderPass {
		#region Fields

		private Material mtl;

		#endregion

		#region Properties

		internal RenderTargetIdentifier CurrTarget {
			get;
			set;
		}

		#endregion

		#region Ctors and Dtor

		internal BlurRenderPass(): base() {
			mtl = null;

			//CurrTarget;
		}

        static BlurRenderPass() {
		}

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		internal void InitMe(Shader shader) {
			if(shader == null) {
				Console.LogError("shader == null");
				return;
			}

			renderPassEvent = RenderPassEvent.AfterRenderingTransparents;

			mtl = CoreUtils.CreateEngineMaterial(shader);
			mtl.SetFloat("_BlurSize", 0.04f);

			mtl.EnableKeyword("_SAMPLES_LOW");
			mtl.DisableKeyword("_SAMPLES_MEDIUM");
			mtl.DisableKeyword("_SAMPLES_HIGH");
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
			if(mtl == null) {
				Console.LogError("mtl == null");
				return;
			}

			if(!renderingData.cameraData.postProcessEnabled) {
				Console.LogError("!renderingData.cameraData.postProcessEnabled");
				return;
			}

			CommandBuffer commandBuffer = CommandBufferPool.Get(string.Empty);

			Render(commandBuffer, ref renderingData);
			context.ExecuteCommandBuffer(commandBuffer);

			CommandBufferPool.Release(commandBuffer);
		}

		private void Render(CommandBuffer commandBuffer, ref RenderingData _) {
			commandBuffer.Blit(CurrTarget, BuiltinRenderTextureType.CurrentActive, mtl, 0);
			commandBuffer.Blit(CurrTarget, BuiltinRenderTextureType.CurrentActive, mtl, 1);
		}
	}
}