using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Genesis.Wisdom;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.Creation {
    internal sealed class SplashToGame: MonoBehaviour {
		public void Callback() {
			if(!canClick) {
				return;
			}

			canClick = false;

			canvasGrpFadeAnim.StartAnim(true);
		}

		[SerializeField]
		private SceneChangeUnit sceneChangeUnit;

		[SerializeField]
		private TMP_Text userFeedbackText;

		[SerializeField]
		private UserFeedbackTextModifier userFeedbackTextModifier;

		[SerializeField]
		private Image[] progressImgArr;

		[SerializeField]
		private TMP_Text[] progressTextArr;

		[SerializeField]
		private CanvasGrpFadeAnim canvasGrpFadeAnim;

		[SerializeField]
		private float initialDelay;

		[SerializeField]
		private float finalDelay;

		private bool canClick = false;

		#if UNITY_EDITOR

		private void OnValidate() {
			if(EditorApplication.isPlayingOrWillChangePlaymode
				|| userFeedbackTextModifier == null
				|| userFeedbackText == null
			) {
				return;
			}

			userFeedbackTextModifier.InOnValidate(userFeedbackText);
		}

		#endif

		private void Awake() {
			if(userFeedbackTextModifier == null || userFeedbackText == null) {
				UnityEngine.Assertions.Assert.IsTrue(
					false,
					"userFeedbackTextModifier == null || userFeedbackText == null"
				);
				return;
			}

			progressImgArr ??= new Image[0];
			progressTextArr ??= new TMP_Text[0];

			userFeedbackTextModifier.InAwake(userFeedbackText);

			foreach(Image progressImg in progressImgArr) {
				progressImg.fillAmount = 0.0f;
			}

			foreach(TMP_Text progressText in progressTextArr) {
				progressText.text = "0%";
			}

			_ = StartCoroutine(nameof(MyCoroutine));
		}

		private IEnumerator MyCoroutine() {
			userFeedbackTextModifier.Processing(userFeedbackText);

			yield return new WaitForSeconds(initialDelay);

			sceneChangeUnit.LoadSceneAsync(out AsyncOperation asyncOperation);

			asyncOperation.allowSceneActivation = false;

			yield return StartCoroutine(MyOtherCoroutine(asyncOperation));

			userFeedbackTextModifier.Success(userFeedbackText);

			canClick = true;
		}

		private IEnumerator MyOtherCoroutine(AsyncOperation asyncOperation) {
			for(;;) {
				foreach(Image progressImg in progressImgArr) {
					progressImg.fillAmount = asyncOperation.progress;
				}

				foreach(TMP_Text progressText in progressTextArr) {
					progressText.text = ((int)Mathf.Round(
						asyncOperation.progress * 100.0f
					)).ToString() + '%';
				}

				if(asyncOperation.progress >= 0.9f) {
					if(!asyncOperation.allowSceneActivation) {
						yield return new WaitForSeconds(finalDelay);

						asyncOperation.allowSceneActivation = true;

						yield return asyncOperation;

						continue;
					}

					break;
				} else {
					yield return null;
				}
			}
		}
    }
}