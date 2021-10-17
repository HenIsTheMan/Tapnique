using UnityEngine;
using Genesis.Wisdom;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

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
		private float totalGameTime;

		private float gameTime;

		[SerializeField]
		private ObjPool buttonPool;

		[SerializeField]
		private Transform buttonParentTransform;

		[SerializeField]
		private ObjPoolData buttonPoolData;

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

			buttonPool.InitMe(
				buttonPoolData.Size,
				buttonPoolData.Prefab,
				buttonParentTransform,
				buttonPoolData.InstanceName
			);

			gameViewLayer.ModifyStrOfGameTimeText(totalGameTime);

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
			gameTime = totalGameTime;
			_ = StartCoroutine(nameof(GameTimeCoroutine));
			_ = StartCoroutine(nameof(GameLogicCoroutine));
		}

		private IEnumerator GameTimeCoroutine() {
			while(gameTime > 0.0f) {
				gameTime -= Time.deltaTime;
				gameTime = Mathf.Max(0.0f, gameTime);

				gameViewLayer.ModifyStrOfGameTimeText(gameTime);
				yield return null;
			}

			EndGame();
		}

		private void EndGame() {
			StopCoroutine(nameof(GameLogicCoroutine));
		}

		private IEnumerator GameLogicCoroutine() {
			float halfScreenWidth = Screen.width * 0.5f;
			float halfScreenHeight = Screen.height * 0.5f;

			while(true) {
				int amtOfButtonsToSpawn = Random.Range(1, 4);
				List<GameObject> activatedButtonGameObjs = new List<GameObject>(amtOfButtonsToSpawn);
				GameObject buttonGameObj;
				RectTransform myRectTransform;
				float xOffset, yOffset;

				for(int i = 0; i < amtOfButtonsToSpawn; ++i) {
					buttonGameObj = buttonPool.ActivateObj();
					myRectTransform = (RectTransform)buttonGameObj.transform;

					xOffset = myRectTransform.sizeDelta.x * myRectTransform.localScale.x * 0.5f;
					yOffset = myRectTransform.sizeDelta.y * myRectTransform.localScale.y * 0.5f;

					myRectTransform.anchoredPosition = new Vector3(
						Random.Range(
							-halfScreenWidth + xOffset,
							halfScreenWidth - xOffset
						),
						Random.Range(
							-halfScreenHeight + yOffset,
							halfScreenHeight - yOffset
						),
						0.0f
					);

					activatedButtonGameObjs.Add(buttonGameObj);
				}

				yield return new WaitWhile(() => {
					return activatedButtonGameObjs.Any((gameObj) => {
						return gameObj.activeInHierarchy;
					});
				});
			}
		}
	}
}