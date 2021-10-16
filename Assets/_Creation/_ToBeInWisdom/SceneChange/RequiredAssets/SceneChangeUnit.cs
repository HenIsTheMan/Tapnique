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
		internal string SceneName {
			get => sceneName;
		}

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

		[SerializeField]
		private UnloadSceneOptions unloadSceneOptions;

		private bool canLoadScene = true;

		private bool canUnloadScene = true;

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

		public void UnloadSceneAsync() {
			if(!canUnloadScene) {
				return;
			}

			canUnloadScene = false;

			AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName, unloadSceneOptions);

			if(asyncOperation == null) { //Need to check as operation is async
				UnityEngine.Assertions.Assert.IsTrue(false, "asyncOperation == null");
			}

			asyncOperation.completed += (_) => {
				canUnloadScene = true;
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

		internal void LoadSceneAsync(out AsyncOperation asyncOperation) {
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

		internal IEnumerator UnloadSceneCoroutine() {
			if(!canUnloadScene) {
				yield break;
			}

			canUnloadScene = false;

			AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName, unloadSceneOptions);

			if(asyncOperation == null) { //Need to check as operation is async
				UnityEngine.Assertions.Assert.IsTrue(false, "asyncOperation == null");
			}

			yield return asyncOperation;

			canUnloadScene = true;
		}

		internal void UnloadSceneAsync(out AsyncOperation asyncOperation) {
			if(!canUnloadScene) {
				asyncOperation = null;
				return;
			}

			canUnloadScene = false;

			asyncOperation = SceneManager.UnloadSceneAsync(sceneName, unloadSceneOptions);

			if(asyncOperation == null) { //Need to check as operation is async
				UnityEngine.Assertions.Assert.IsTrue(false, "asyncOperation == null");
			}

			asyncOperation.completed += (_) => {
				canUnloadScene = true;
			};
		}
	}
}