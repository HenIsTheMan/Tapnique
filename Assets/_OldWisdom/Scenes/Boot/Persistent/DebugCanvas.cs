using UnityEngine;

namespace IWP.General {
    internal sealed class DebugCanvas: MonoBehaviour {
        #region Fields

		[SerializeField]
		private bool isVisible;

		[SerializeField]
		private KeyCode keyCode;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal DebugCanvas(): base() {
			isVisible = true;

			keyCode = KeyCode.Space;
		}

        static DebugCanvas() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		private void Awake() {
			Visibility();
		}

		private void Update() {
			if(Input.GetKeyDown(keyCode)) {
				isVisible = !isVisible;
				Visibility();
			}
		}

		#endregion

		private void Visibility() {
			foreach(Transform child in transform) {
				child.gameObject.SetActive(isVisible);
			}
		}
	}
}