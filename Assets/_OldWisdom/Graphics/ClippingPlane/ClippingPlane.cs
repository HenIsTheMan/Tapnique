using UnityEngine;

namespace IWP.General {
	[ExecuteAlways]
	internal sealed class ClippingPlane: MonoBehaviour {
        #region Fields

		[SerializeField]
		private Material[] mtls;

		private Plane plane;

        #endregion

        #region Properties

		internal Material[] Mtls {
			get => mtls;
			set => mtls = value;
		}

        #endregion

        #region Ctors and Dtor

        internal ClippingPlane(): base() {
			mtls = System.Array.Empty<Material>();

			//plane;
        }

        static ClippingPlane() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		private void Update() {
			plane = new Plane(transform.up, transform.position);

			foreach(Material mtl in mtls) {
				mtl.SetVector("_PlaneData", new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance));
			}
		}

        #endregion
    }
}