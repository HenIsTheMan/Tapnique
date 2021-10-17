using UnityEngine;
using Genesis.Wisdom;
using System.Collections;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.Creation {
    internal sealed class GameModelLayer: MonoBehaviour {
		[SerializeField]
		private GameViewLayer gameViewLayer;

        #if UNITY_EDITOR

		[SerializeField]
		private SceneAsset sceneAsset;

		#endif

		[UnmodifiableInInspector]
		[SerializeField]
		private string sceneName;

		[SerializeField]
		private float startGameCountdownTime;

		[SerializeField]
		private string countdownEndedStr;

		[SerializeField]
		private float startGameDelay;

		private float startGameTime;

		[SerializeField]
		private ObjPool objPool;

		[SerializeField]
		private Transform objParentTransform;

		[SerializeField]
		private ObjPoolData objPoolData;

		#if UNITY_EDITOR

		private void OnValidate() {
			if(EditorApplication.isPlayingOrWillChangePlaymode || sceneAsset == null) {
				return;
			}

			sceneName = sceneAsset.name;

			EditorUtility.SetDirty(this); //So can do File --> Save Project
		}

		#endif

		private void Awake() {
			WaitHelper.AddWaitForSeconds(startGameDelay);

			objPool.InitMe(
				objPoolData.Size,
				objPoolData.Prefab,
				objParentTransform,
				objPoolData.InstanceName
			);

			_ = StartCoroutine(nameof(StartGameCoroutine));
		}

		private IEnumerator StartGameCoroutine() {
			yield return new WaitUntil(() => {
				return !SceneManager.GetSceneByName(sceneName).isLoaded;
			});

			gameViewLayer.ActivateDeactivateCountdownTextGameObj(true);
			startGameTime = startGameCountdownTime;

			while(startGameTime > 0.0f) {
				startGameTime -= Time.deltaTime;
				startGameTime = Mathf.Max(0.0f, startGameTime);

				if(Mathf.Approximately(startGameTime, 0.0f)) {
					gameViewLayer.ModifyStrOfCountdownText(countdownEndedStr);
					break;
				} else {
					gameViewLayer.ModifyStrOfCountdownText(startGameTime);
					yield return null;
				}
			}

			yield return WaitHelper.GetWaitForSeconds(startGameDelay);

			gameViewLayer.ActivateDeactivateCountdownTextGameObj(false);
			StartGame();
		}

		private void StartGame() {
			print("here");
		}
    }
}