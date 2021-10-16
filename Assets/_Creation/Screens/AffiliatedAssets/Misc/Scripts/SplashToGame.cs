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

			_ = StartCoroutine(nameof(MyCoroutine));
		}

		private IEnumerator MyCoroutine() {
			userFeedbackTextModifier.Processing(userFeedbackText);

			sceneChangeUnit.LoadSceneAsync(out AsyncOperation asyncOperation);

			while(!asyncOperation.isDone) {
				foreach(Image progressImg in progressImgArr) {
					progressImg.fillAmount = asyncOperation.progress / 0.9f;
				}

				foreach(TMP_Text progressText in progressTextArr) {
					progressText.text = ((int)Mathf.Round(asyncOperation.progress / 0.9f * 100.0f)).ToString();
				}

				yield return null;
			}

			userFeedbackTextModifier.Success(userFeedbackText);

			canClick = true;
		}
    }
}