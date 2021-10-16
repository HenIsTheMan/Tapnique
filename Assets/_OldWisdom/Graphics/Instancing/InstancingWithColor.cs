using UnityEngine;

namespace IWP.General {
	internal sealed partial class Instancer: MonoBehaviour {
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0018:Inline variable declaration", Justification = "<Pending>")]
		public static void InstancingWithColor() {
			uint x;

			//* Pos 
			globalObj.posComputeBuffer = new ComputeBuffer((int)globalObj.instanceCount, 16);

			int posKernelIndex = globalObj.computeShader.FindKernel("PosMain");
			globalObj.computeShader.SetBuffer(posKernelIndex, "posRWStructuredBuffer", globalObj.posComputeBuffer);

			globalObj.computeShader.GetKernelThreadGroupSizes(posKernelIndex, out x, out _, out _);
			globalObj.computeShader.Dispatch(posKernelIndex, (int)globalObj.instanceCount / (int)x, 1, 1);

			globalObj.instanceMtl.SetBuffer("posStructuredBuffer", globalObj.posComputeBuffer);
			//*/

			//* Colors
			globalObj.colorComputeBuffer = new ComputeBuffer((int)globalObj.instanceCount, 12);

			int colorKernelIndex = globalObj.computeShader.FindKernel("ColorMain");
			globalObj.computeShader.SetBuffer(colorKernelIndex, "colorRWStructuredBuffer", globalObj.colorComputeBuffer);

			globalObj.computeShader.SetFloat("minPhi", 0.0f);
			globalObj.computeShader.SetFloat("maxPhi", 360.0f);

			globalObj.computeShader.SetFloat("minTheta", 0.0f);
			globalObj.computeShader.SetFloat("maxTheta", 360.0f);

			globalObj.computeShader.SetFloat("minRadius", 700.0f);
			globalObj.computeShader.SetFloat("maxRadius", 1400.0f);

			globalObj.computeShader.SetFloat("minW", 2.0f);
			globalObj.computeShader.SetFloat("maxW", 3.0f);

			globalObj.computeShader.SetFloat("minHue", 0.0f);
			globalObj.computeShader.SetFloat("maxHue", 360.0f);

			globalObj.computeShader.SetFloat("minSaturation", 0.0f);
			globalObj.computeShader.SetFloat("maxSaturation", 1.0f);

			globalObj.computeShader.SetFloat("minVal", 0.7f);
			globalObj.computeShader.SetFloat("maxVal", 1.0f);

			globalObj.computeShader.SetFloats("intensityVec", new float[]{14.0f, 14.0f, 14.0f});

			globalObj.computeShader.GetKernelThreadGroupSizes(colorKernelIndex, out x, out _, out _);
			globalObj.computeShader.Dispatch(colorKernelIndex, (int)globalObj.instanceCount / (int)x, 1, 1);

			globalObj.instanceMtl.SetBuffer("colorStructuredBuffer", globalObj.colorComputeBuffer);
			//*/
		}
	}
}