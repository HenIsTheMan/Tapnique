using UnityEngine;
using Genesis.Wisdom;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.Creation {
    internal sealed class ModelLayer: MonoBehaviour {
        #if UNITY_EDITOR

		[SerializeField]
		private SceneAsset sceneAsset;

		#endif

		[UnmodifiableInInspector]
		[SerializeField]
		private string sceneName;

		[SerializeField]
		private TMP_Text countdownText;

		[SerializeField]
		private float startGameCountdown;

		[SerializeField]
		private string countdownEndedStr;

		[SerializeField]
		private float startGameDelay;

		private float startGameTime;

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

			_ = StartCoroutine(nameof(StartGameCoroutine));
		}

		private IEnumerator StartGameCoroutine() {
			yield return new WaitUntil(() => {
				return !SceneManager.GetSceneByName(sceneName).isLoaded;
			});

			countdownText.gameObject.SetActive(true);
			startGameTime = startGameCountdown;

			while(startGameTime > 0.0f) {
				startGameTime -= Time.deltaTime;
				startGameTime = Mathf.Max(0.0f, startGameTime);
				countdownText.text = ((int)Mathf.Round(startGameTime)).ToString();
			}

			countdownText.text = countdownEndedStr;

			yield return WaitHelper.GetWaitForSeconds(startGameDelay);

			StartGame();
		}

		private void StartGame() {
			print("here");
		}
    }
}