using UnityEngine;
using Genesis.Wisdom;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

#if UNITY_EDITOR

using UnityEditor.SceneManagement;
using UnityEditor;

#endif

namespace Genesis.Creation {
    internal sealed class GameModelLayer: Singleton<GameModelLayer> {
		internal void ButtonOnClick(GameObject gameButtonGameObj) {
			gameButtonPool.DeactivateObj(gameButtonGameObj);
		}

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
		private float startGameDelay;

		private float startGameTime;

		[SerializeField]
		private float totalGameTime;

		private float gameTime;

		[SerializeField]
		private int initialPts;

		private int pts;

		[SerializeField]
		private float totalRoundTime;

		private float roundTime;

		[SerializeField]
		private ObjPool gameButtonPool;

		[SerializeField]
		private Transform gameButtonParentTransform;

		[SerializeField]
		private ObjPoolData gameButtonPoolData;

		[SerializeField]
		private float initialSpd;

		[SerializeField]
		private float spdMultiplier;

		#if UNITY_EDITOR

		[ContextMenu("Refresh")]
		private protected override void OnValidate() {
			base.OnValidate();

			if(EditorApplication.isPlayingOrWillChangePlaymode) {
				return;
			}

			if(sceneAsset != null) {
				sceneName = sceneAsset.name;
			}

			Init();

			Task myTask = new Task(async () => {
				await Task.Delay(14); //Just in case

				if(gameObject != null) {
					EditorSceneManager.SaveScene(gameObject.scene);
				}
			});

			myTask.RunSynchronously();
		}

		#endif

		private void Awake() {
			Init();

			WaitHelper.AddWaitForSeconds(startGameDelay);

			gameButtonPool.InitMe(
				gameButtonPoolData.Size,
				gameButtonPoolData.Prefab,
				gameButtonParentTransform,
				gameButtonPoolData.InstanceName
			);

			_ = StartCoroutine(nameof(StartGameCoroutine));
		}

		private void Init() {
			gameViewLayer = GameViewLayer.GlobalObj;
			gameViewLayer.ModifyStrOfGameTimeText(totalGameTime);
			gameViewLayer.ModifyStrOfPtsText(initialPts);
			gameViewLayer.ModifyStrOfRoundTimeText(totalRoundTime);
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
					gameViewLayer.ShowCountdownEndedStr();
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
			pts = initialPts;
			roundTime = totalRoundTime;

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

			int highScore = PlayerPrefs.GetInt(nameof(pts), 0);

			if(pts > highScore) {
				highScore = pts;
				PlayerPrefs.SetInt(nameof(pts), highScore);

				gameViewLayer.ShowGameEndView(true, pts, highScore);
			} else {
				gameViewLayer.ShowGameEndView(false, pts, highScore);
			}
		}

		private IEnumerator GameLogicCoroutine() {
			float halfScreenWidth = Screen.width * 0.5f;
			float halfScreenHeight = Screen.height * 0.5f;

			while(true) {
				int amtOfButtonsToSpawn = Random.Range(1, 4);
				GameObject gameButtonGameObj;

				GameButtonLink gameButtonLink;
				List<GameButtonLink> gameButtonLinkList = new List<GameButtonLink>(amtOfButtonsToSpawn);

				RectTransform myRectTransform;
				float xOffset, yOffset;

				for(int i = 0; i < amtOfButtonsToSpawn; ++i) {
					gameButtonGameObj = gameButtonPool.ActivateObj();
					gameButtonLink = gameButtonGameObj.GetComponent<GameButtonLink>();

					gameViewLayer.ColorizeGameButton(gameButtonLink);

					gameButtonLink.dir = Random.insideUnitCircle.normalized;
					gameButtonLink.MyRigidbody.velocity
						= gameButtonLink.dir
						* initialSpd;

					myRectTransform = (RectTransform)gameButtonGameObj.transform;

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

					GameControllerLayer.GlobalObj.ConfigGameButton(gameButtonLink);

					gameButtonLinkList.Add(gameButtonLink);
				}

				Vector3 anchoredPos;
				Vector3 vel;

				while(gameButtonPool.ActiveObjs.Any((gameObj) => {
					return gameObj.activeInHierarchy;
				}) && roundTime > 0.0f) {
					gameButtonLinkList.ForEach((gameButtonLink) => {
						myRectTransform = (RectTransform)gameButtonLink.transform;

						xOffset = myRectTransform.sizeDelta.x * myRectTransform.localScale.x * 0.5f;
						yOffset = myRectTransform.sizeDelta.y * myRectTransform.localScale.y * 0.5f;

						anchoredPos = myRectTransform.anchoredPosition;
						vel = gameButtonLink.MyRigidbody.velocity;

						if(anchoredPos.x < -halfScreenWidth + xOffset) {
							anchoredPos.x = -halfScreenWidth + xOffset;
							vel.x = -vel.x;
							gameButtonLink.dir.x = -gameButtonLink.dir.x;
						}

						if(anchoredPos.x > halfScreenWidth - xOffset) {
							anchoredPos.x = halfScreenWidth - xOffset;
							vel.x = -vel.x;
							gameButtonLink.dir.x = -gameButtonLink.dir.x;
						}

						if(anchoredPos.y < -halfScreenHeight + yOffset) {
							anchoredPos.y = -halfScreenHeight + yOffset;
							vel.y = -vel.y;
							gameButtonLink.dir.y = -gameButtonLink.dir.y;
						}

						if(anchoredPos.y > halfScreenHeight - yOffset) {
							anchoredPos.y = halfScreenHeight - yOffset;
							vel.y = -vel.y;
							gameButtonLink.dir.y = -gameButtonLink.dir.y;
						}

						myRectTransform.anchoredPosition = anchoredPos;
						gameButtonLink.MyRigidbody.velocity = vel;

						gameButtonLink.MyRigidbody.velocity
							= gameButtonLink.dir
							* (initialSpd + RateOfChange.EaseInSine(1.0f - gameTime / totalGameTime) * spdMultiplier);
					});

					roundTime -= Time.deltaTime;
					roundTime = Mathf.Max(0.0f, roundTime);

					gameViewLayer.ModifyStrOfRoundTimeText(roundTime);
					yield return null;
				}

				pts += (int)Mathf.Floor(roundTime * 100.0f);
				gameViewLayer.ModifyStrOfPtsText(pts);

				gameButtonPool.DeactivateAllObjs();
				roundTime = totalRoundTime;
			}
		}
	}
}