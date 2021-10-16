using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Genesis.Wisdom;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.Creation {
    internal sealed class SplashToGame: MonoBehaviour {
		public async void Callback() {
			if(!canClick) {
				return;
			}

			canClick = false;

			canvasGrpFadeAnim.StartAnim(true);

			mySceneChangeUnit.UnloadSceneAsync(out AsyncOperation asyncOperation);

			asyncOperation.allowSceneActivation = false;

			await Task.FromResult(canvasGrpFadeAnim.IsUpdating == false);

			asyncOperation.allowSceneActivation = true;
		}

		[SerializeField]
		private SceneChangeUnit mySceneChangeUnit;

		[SerializeField]
		private SceneChangeUnit otherSceneChangeUnit;

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

		[SerializeField]
		private Camera myCam;

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
			if(userFeedbackTextModifier == null || userFeedbackText == null || myCam == null) {
				UnityEngine.Assertions.Assert.IsTrue(
					false,
					"userFeedbackTextModifier == null || userFeedbackText == null || myCam == null"
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

			otherSceneChangeUnit.LoadSceneAsync(out AsyncOperation asyncOperation);

			asyncOperation.allowSceneActivation = false;

			yield return StartCoroutine(MyOtherCoroutine(asyncOperation));

			List<Camera> otherCams = FindObjectsOfType<Camera>().ToList();
			if(!otherCams.Remove(myCam)) {
				UnityEngine.Assertions.Assert.IsTrue(false, "!otherCams.Remove(myCam)");
			}

			int count = otherCams.Count;
			List<float> camPriorities = new List<float>(count);

			otherCams.ForEach((otherCam) => {
				camPriorities.Add(otherCam.depth);
			});

			myCam.depth = camPriorities.ToArray().Max() + 0.1f;

			myCam.clearFlags = CameraClearFlags.Nothing;

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

						continue; //Force a continue
					}

					if(asyncOperation.isDone) {
						break;
					} else {
						yield return null;
					}
				} else {
					yield return null;
				}
			}
		}
    }
}