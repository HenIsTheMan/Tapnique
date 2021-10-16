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

		private bool canLoadScene = true;

		#if UNITY_EDITOR

		private void OnValidate() {
			if(EditorApplication.isPlayingOrWillChangePlaymode || sceneAsset == null) {
				return;
			}

			sceneName = sceneAsset.name;

			EditorUtility.SetDirty(this); //So can do File --> Save Project
		}

		#endif

		public void LoadScene() {
			if(!canLoadScene) {
				return;
			}

			canLoadScene = false;

			_ = SceneManager.LoadScene(sceneName, new LoadSceneParameters {
				loadSceneMode = loadSceneMode,
				localPhysicsMode = localPhysicsMode
			});

			canLoadScene = true;
		}

		public void LoadSceneAsync() {
			if(!canLoadScene) {
				return;
			}

			canLoadScene = false;

			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, new LoadSceneParameters {
				loadSceneMode = loadSceneMode,
				localPhysicsMode = localPhysicsMode
			});

			if(asyncOperation == null) { //Need to check as operation is async
				UnityEngine.Assertions.Assert.IsTrue(false, "asyncOperation == null");
			}

			asyncOperation.completed += (_) => {
				canLoadScene = true;
			};
		}

		internal IEnumerator LoadSceneCoroutine() {
			if(!canLoadScene) {
				yield break;
			}

			canLoadScene = false;

			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, new LoadSceneParameters {
				loadSceneMode = loadSceneMode,
				localPhysicsMode = localPhysicsMode
			});

			if(asyncOperation == null) { //Need to check as operation is async
				UnityEngine.Assertions.Assert.IsTrue(false, "asyncOperation == null");
			}

			yield return asyncOperation;

			canLoadScene = true;
		}

		internal void LoadScene(out Scene scene) {
			if(!canLoadScene) {
				scene = default(Scene);
				return;
			}

			canLoadScene = false;

			scene = SceneManager.LoadScene(sceneName, new LoadSceneParameters {
				loadSceneMode = loadSceneMode,
				localPhysicsMode = localPhysicsMode
			});

			canLoadScene = true;
		}

		public void LoadSceneAsync(out AsyncOperation asyncOperation) {
			if(!canLoadScene) {
				asyncOperation = null;
				return;
			}

			canLoadScene = false;

			asyncOperation = SceneManager.LoadSceneAsync(sceneName, new LoadSceneParameters {
				loadSceneMode = loadSceneMode,
				localPhysicsMode = localPhysicsMode
			});

			if(asyncOperation == null) { //Need to check as operation is async
				UnityEngine.Assertions.Assert.IsTrue(false, "asyncOperation == null");
			}

			asyncOperation.completed += (_) => {
				canLoadScene = true;
			};
		}
	}
}