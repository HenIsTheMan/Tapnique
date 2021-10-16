using UnityEngine;
using UnityEngine.SceneManagement;

namespace IWP.General {
	[DisallowMultipleComponent]
	internal sealed class SceneManager: MonoBehaviour {
		internal delegate void DoneDelegate();

		#region Fields

		internal static SceneManager globalObj;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal SceneManager(): base() {
		}

		static SceneManager() {
			globalObj = null;
		}

		#endregion

		#region Unity User Callback Event Funcs

		private void Awake() {
			globalObj = this;
		}

		#endregion

		internal void LoadScene(string sceneName, LoadSceneTypes.LoadSceneType type, DoneDelegate doneDelegate) {
			var operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, (LoadSceneMode)type);

			if(operation != null) { //Need to check as operation is async
				operation.completed += (_) => {
					doneDelegate?.Invoke();
				};
			}
		}

		internal void UnloadScene(string sceneName, UnloadSceneTypes.UnloadSceneType type, DoneDelegate doneDelegate) {
			var operation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName, (UnloadSceneOptions)type);

			if(operation != null) { //Need to check as operation is async
				operation.completed += (_) => {
					doneDelegate?.Invoke();
				};
			}
		}
	}
}