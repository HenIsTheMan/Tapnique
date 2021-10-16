using UnityEngine;

namespace IWP.General {
	[DisallowMultipleComponent]
	internal sealed class Persistence: MonoBehaviour {
		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal Persistence(): base() {
		}

        static Persistence() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		private void Awake() {
			if(transform.parent == null) {
				DontDestroyOnLoad(gameObject);
			}
		}

		#endregion
	}
}