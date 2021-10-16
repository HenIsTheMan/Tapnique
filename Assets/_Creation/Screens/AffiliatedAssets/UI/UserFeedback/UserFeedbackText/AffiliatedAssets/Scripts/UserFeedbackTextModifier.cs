using Genesis.Wisdom;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.Creation {
	[CreateAssetMenu(
		fileName = nameof(UserFeedbackTextModifier),
		menuName = "ScriptableObjs/" + nameof(UserFeedbackTextModifier),
		order = 2
	)]
	internal sealed class UserFeedbackTextModifier: ScriptableObject {
		[field: SerializeField]
		internal string InitialStr {
			get;
			private set;
		}

		[field: SerializeField]
		internal string ProcessingStr {
			get;
			private set;
		}

		[field: SerializeField]
		internal string SuccessStr {
			get;
			private set;
		}

		[field: SerializeField]
		internal string FailureStr {
			get;
			private set;
		}

		[field: SerializeField]
		internal Material InitialMtl {
			get;
			private set;
		}

		[field: SerializeField]
		internal Material ProcessingMtl {
			get;
			private set;
		}

		[field: SerializeField]
		internal Material SuccessMtl {
			get;
			private set;
		}

		[field: SerializeField]
		internal Material FailureMtl {
			get;
			private set;
		}

		[field: SerializeField]
		internal float SuccessLifetime {
			get;
			private set;
		}

		[field: SerializeField]
		internal float FailureLifetime {
			get;
			private set;
		}

		internal void InOnValidate(TMP_Text userFeedbackText) {
			userFeedbackText.fontSharedMaterial = InitialMtl;
			userFeedbackText.text = InitialStr;

			#if UNITY_EDITOR

			EditorUtility.SetDirty(this); //So can do File --> Save Project

			#endif
		}

		internal void InAwake(TMP_Text userFeedbackText) {
			WaitHelper.AddWaitForSeconds(SuccessLifetime);
			WaitHelper.AddWaitForSeconds(FailureLifetime);
		}

		internal void Processing(TMP_Text userFeedbackText) {
			userFeedbackText.fontSharedMaterial = ProcessingMtl;
			userFeedbackText.text = ProcessingStr;
		}

		internal void Success(TMP_Text userFeedbackText) {
			userFeedbackText.fontSharedMaterial = SuccessMtl;
			userFeedbackText.text = SuccessStr;
		}

		internal void Failure(TMP_Text userFeedbackText) {
			userFeedbackText.fontSharedMaterial = FailureMtl;
			userFeedbackText.text = FailureStr;
		}

		internal IEnumerator OnSuccessCoroutine(TMP_Text userFeedbackText) {
			yield return SuccessCoroutine(userFeedbackText);
		}

		internal IEnumerator OnFailureCoroutine(TMP_Text userFeedbackText) {
			yield return FailureCoroutine(userFeedbackText);
		}

		internal async void OnSuccessAsync(TMP_Text userFeedbackText) {
			userFeedbackText.fontSharedMaterial = SuccessMtl;
			userFeedbackText.text = SuccessStr;

			await Task.Delay((int)(SuccessLifetime * 1000.0f));

			userFeedbackText.fontSharedMaterial = null;
			userFeedbackText.text = string.Empty;
		}

		internal async void OnFailureAsync(TMP_Text userFeedbackText) {
			userFeedbackText.fontSharedMaterial = FailureMtl;
			userFeedbackText.text = FailureStr;

			await Task.Delay((int)(FailureLifetime * 1000.0f));

			userFeedbackText.fontSharedMaterial = null;
			userFeedbackText.text = string.Empty;
		}

		private IEnumerator SuccessCoroutine(TMP_Text userFeedbackText) {
			userFeedbackText.fontSharedMaterial = SuccessMtl;
			userFeedbackText.text = SuccessStr;

			yield return WaitHelper.GetWaitForSeconds(SuccessLifetime);

			userFeedbackText.text = string.Empty;
		}

		private IEnumerator FailureCoroutine(TMP_Text userFeedbackText) {
			userFeedbackText.fontSharedMaterial = FailureMtl;
			userFeedbackText.text = FailureStr;

			yield return WaitHelper.GetWaitForSeconds(FailureLifetime);

			userFeedbackText.text = string.Empty;
		}
	}
}