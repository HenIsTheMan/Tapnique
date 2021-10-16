using UnityEngine;
using UnityEngine.SceneManagement;
using Genesis.Wisdom;
using System.Collections;
using System;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.Creation {
	[CreateAssetMenu(
		fileName = nameof(SceneChangeUnit),
		menuName = "ScriptableObjs/" + nameof(SceneChangeUnit),
		order = 1
	)]
	internal sealed class SceneChangeUnit: ScriptableObject {
		#if UNITY_EDITOR

		[SerializeField]
		private SceneAsset sceneAsset;

		#endif

		[UnmodifiableInInspector]
		[SerializeField]
		private string sceneName;

		[SerializeField]
		private LoadSceneMode loadSceneMode;

		[SerializeField]
		private LocalPhysicsMode localPhysicsMode;

		private bool canChangeScene = true;

		#if UNITY_EDITOR

		private void OnValidate() {
			if(EditorApplication.isPlayingOrWillChangePlaymode || sceneAsset == null) {
				return;
			}

			sceneName = sceneAsset.name;

			EditorUtility.SetDirty(this); //So can do File --> Save Project
		}

		#endif

		public void ChangeScene() {
			if(!canChangeScene) {
				return;
			}

			canChangeScene = false;

			_ = SceneManager.LoadScene(sceneName, new LoadSceneParameters {
				loadSceneMode = loadSceneMode,
				localPhysicsMode = localPhysicsMode
			});

			canChangeScene = true;
		}

		public void ChangeSceneAsync() {
			if(!canChangeScene) {
				return;
			}

			canChangeScene = false;

			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, new LoadSceneParameters {
				loadSceneMode = loadSceneMode,
				localPhysicsMode = localPhysicsMode
			});

			if(asyncOperation == null) { //Need to check as operation is async
				UnityEngine.Assertions.Assert.IsTrue(false, "asyncOperation == null");
			}

			asyncOperation.completed += (_) => {
				canChangeScene = true;
			};
		}

		internal IEnumerator ChangeSceneCoroutine() {
			if(!canChangeScene) {
				yield break;
			}

			canChangeScene = false;

			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, new LoadSceneParameters {
				loadSceneMode = loadSceneMode,
				localPhysicsMode = localPhysicsMode
			});

			if(asyncOperation == null) { //Need to check as operation is async
				UnityEngine.Assertions.Assert.IsTrue(false, "asyncOperation == null");
			}

			yield return asyncOperation;

			canChangeScene = true;
		}

		internal void ChangeScene(out Scene scene) {
			if(!canChangeScene) {
				scene = default(Scene);
				return;
			}

			canChangeScene = false;

			scene = SceneManager.LoadScene(sceneName, new LoadSceneParameters {
				loadSceneMode = loadSceneMode,
				localPhysicsMode = localPhysicsMode
			});

			canChangeScene = true;
		}

		public void ChangeSceneAsync(out AsyncOperation asyncOperation) {
			if(!canChangeScene) {
				asyncOperation = null;
				return;
			}

			canChangeScene = false;

			asyncOperation = SceneManager.LoadSceneAsync(sceneName, new LoadSceneParameters {
				loadSceneMode = loadSceneMode,
				localPhysicsMode = localPhysicsMode
			});

			if(asyncOperation == null) { //Need to check as operation is async
				UnityEngine.Assertions.Assert.IsTrue(false, "asyncOperation == null");
			}

			asyncOperation.completed += (_) => {
				canChangeScene = true;
			};
		}
	}
}