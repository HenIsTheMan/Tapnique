using UnityEngine;

namespace IWP.General {
    internal sealed partial class Instancer: MonoBehaviour {
		#region Fields

		private uint[] args;
		private Bounds bounds;
		private ComputeBuffer posComputeBuffer;
		private ComputeBuffer colorComputeBuffer;
		private ComputeBuffer argComputeBuffer;

		[SerializeField]
		private string instancingMethodName;

		[SerializeField]
		private float sizeFactor;

		[SerializeField]
		private ComputeShader computeShader;

		[SerializeField]
		private int subMeshIndex;

		[SerializeField]
		private uint instanceCount;

		[SerializeField]
		private Mesh instanceMesh;

		[SerializeField]
		private Material instanceMtl;

		[SerializeField]
		internal bool shldDraw;

		private static Instancer globalObj;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal Instancer(): base() {
			args = System.Array.Empty<uint>();
			//bounds;
			posComputeBuffer = null;
			colorComputeBuffer = null;
			argComputeBuffer = null;

			instancingMethodName = string.Empty;
			sizeFactor = 0.0f;
			computeShader = null;
			subMeshIndex = 0;
			instanceCount = 0u;
			instanceMesh = null;
			instanceMtl = null;

			shldDraw = false;
		}

        static Instancer() {
			globalObj = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void OnValidate() {
			if(instanceMesh != null) {
				subMeshIndex = Mathf.Clamp(subMeshIndex, 0, instanceMesh.subMeshCount - 1);
			}
        }

		private void Awake() {
			globalObj = this;

			instanceMtl.SetFloat("instanceCount", instanceCount);

			args = new uint[5]{
				instanceMesh.GetIndexCount(subMeshIndex),
				instanceCount,
				instanceMesh.GetIndexStart(subMeshIndex),
				instanceMesh.GetBaseVertex(subMeshIndex),
				0u
			};

			argComputeBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
			argComputeBuffer.SetData(args);

			bounds = new Bounds(Vector3.zero, Vector3.one * sizeFactor);

			if(instancingMethodName != string.Empty) {
				_ = GetType().GetMethod(instancingMethodName).Invoke(this, null);
			}
		}

		private void Update() {
			if(shldDraw) {
				Graphics.DrawMeshInstancedIndirect(instanceMesh, subMeshIndex, instanceMtl, bounds, argComputeBuffer);
			}
		}

		private void OnDisable() {
			if(posComputeBuffer != null) {
				posComputeBuffer.Dispose();
			}

			if(colorComputeBuffer != null) {
				colorComputeBuffer.Dispose();
			}

			if(argComputeBuffer != null) {
				argComputeBuffer.Release();
			}
		}

		#endregion
	}
}