using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Genesis.Wisdom.RateOfChange;

#if UNITY_EDITOR

using UnityEditor.SceneManagement;
using UnityEditor;

#endif

namespace Genesis.Wisdom {
	internal abstract partial class AbstractAnim: MonoBehaviour {
		[SerializeField]
		protected internal bool isLooping;

		[Min(0.0f)]
		[SerializeField]
		protected internal float startTimeOffset;

		[Min(0.0f)]
		[SerializeField]
		internal float animDuration;

		protected internal float AnimTime {
			get;
			private set;
		}

		protected internal float LerpFactor {
			get;
			private set;
		}

		protected internal int Count {
			get;
			private set;
		}

		protected internal bool IsPlaying {
			get;
			private set;
		}

		protected internal bool IsUpdating {
			get => isUpdating;
		}

		protected internal List<float> PeriodicDelayList {
			set {
				if(value == null) {
					UnityEngine.Assertions.Assert.IsTrue(false, "value == null");
				}

				periodicDelayList = value;

				periodicDelayList.ForEach((periodicDelay) => {
					WaitHelper.AddWaitForSeconds(periodicDelay);
					WaitHelper.AddWaitForSecondsRealtime(periodicDelay);
				});
			}
		}

		protected internal RateOfChangeType MyRateOfChangeType {
			get => myRateOfChangeType;
			set {
				myRateOfChangeType = value;
				ModifyLerpDelegate();
			}
		}

		protected internal bool ShldUseUnscaledDt {
			get => shldUseUnscaledDt;
			set {
				shldUseUnscaledDt = value;
				ModifyDtDelegate();
			}
		}

		protected internal void StartAnim(bool isUpdating) {
			ResetAnim();
			this.isUpdating = isUpdating;
			_ = StartCoroutine(nameof(MyUpdateFunc));
		}

		protected internal void PauseAnim() {
			isUpdating = false;
		}

		protected internal void ResumeAnim() {
			isUpdating = true;
		}

		protected internal void StopAnim() {
			isUpdating = false;
			StopCoroutine(nameof(MyUpdateFunc));
		}

		protected internal void ResetAnim() {
			AnimTime = 0.0f;
			Count = 0;
			UpdateAnim(0.0f);
		}

		protected internal void RestartAnim(bool isUpdating) {
			StopAnim();
			StartAnim(isUpdating);
		}

		#if UNITY_EDITOR

		protected virtual void OnValidate() {
			if(shldPresetValsNow) {
				if(!EditorApplication.isPlayingOrWillChangePlaymode) {
					EditorApplication.CallbackFunction myCallbackFunc = null;

					myCallbackFunc += () => {
						UpdateAnim(presetLerpFactor);
						EditorSceneManager.SaveScene(gameObject.scene);

						EditorApplication.delayCall -= myCallbackFunc;
					};

					EditorApplication.delayCall += myCallbackFunc;
				}

				shldPresetValsNow = false;
			}
		}

		#endif

		protected virtual void Awake() {
			WaitHelper.AddWaitForSeconds(startTimeOffset);
			WaitHelper.AddWaitForSecondsRealtime(startTimeOffset);

			periodicDelayList ??= new List<float>();
			periodicDelayList.ForEach((periodicDelay) => {
				WaitHelper.AddWaitForSeconds(periodicDelay);
				WaitHelper.AddWaitForSecondsRealtime(periodicDelay);
			});

			ModifyLerpDelegate();
			ModifyDtDelegate();

			if(shldPlayOnAwake) {
				StartAnim(isUpdating);
			}
		}

		protected virtual IEnumerator CheckAnim() {
			if(AnimTime >= animDuration) {
				if(Count >= periodicDelayList.Count) {
					if(isLooping) {
						Count = 0;
					} else {
						++Count;
						StopAnim();
						yield break;
					}
				}

				AnimTime = 0.0f;

				if(periodicDelayList.Count == 0) {
					yield return null;
				} else {
					if(shldUseWaitForSecondsRealtime) {
						yield return WaitHelper.GetWaitForSecondsRealtime(periodicDelayList[Count++]);
					} else {
						yield return WaitHelper.GetWaitForSeconds(periodicDelayList[Count++]);
					}
				}
			} else {
				yield return null;
			}
		}

		protected abstract void UpdateAnim(float myLerpFactor);

		[SerializeField]
		private bool shldPresetValsNow;

		[SerializeField]
		private float presetLerpFactor;

		[Min(0.0f)]
		[SerializeField]
		private List<float> periodicDelayList;

		[EnumRange(typeof(RateOfChangeType), 0, (int)RateOfChangeType.Amt)]
		[SerializeField]
		private RateOfChangeType myRateOfChangeType;

		[SerializeField]
		private bool shldUseUnscaledDt;

		[SerializeField]
		private bool shldUseWaitForSecondsRealtime;

		[SerializeField]
		private bool shldPlayOnAwake;

		[SerializeField]
		private bool isUpdating;

		private delegate float LerpFactorDelegate(float x);

		private LerpFactorDelegate lerpFactorDelegate;

		private delegate float DtDelegate();

		private DtDelegate dtDelegate;

		private void ModifyDtDelegate() {
			if(ShldUseUnscaledDt) {
				dtDelegate = () => {
					return Time.unscaledDeltaTime;
				};
			} else {
				dtDelegate = () => {
					return Time.deltaTime;
				};
			}
		}

		private void ModifyLerpDelegate() {
			RateOfChangeDelegate rateOfChangeDelegate = RateOfChangeFuncs[(int)MyRateOfChangeType];

			if(rateOfChangeDelegate == null) {
				lerpFactorDelegate = (float x) => {
					return x;
				};
			} else {
				lerpFactorDelegate = (float x) => {
					return rateOfChangeDelegate(x);
				};
			}
		}

		private IEnumerator MyUpdateFunc() {
			while(!isUpdating) {
				yield return null;
			}

			if(shldUseWaitForSecondsRealtime) {
				yield return WaitHelper.GetWaitForSecondsRealtime(startTimeOffset);
			} else {
				yield return WaitHelper.GetWaitForSeconds(startTimeOffset);
			}

			while(true) {
				while(!isUpdating) {
					yield return null;
				}

				AnimTime += dtDelegate.Invoke();
				LerpFactor = lerpFactorDelegate(Mathf.Min(1.0f, AnimTime / animDuration));
				UpdateAnim(LerpFactor);

				yield return CheckAnim();
			}
		}
	}
}