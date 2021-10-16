using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TMP_InputField;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.Creation {
	internal sealed class ShowHidePassword: MonoBehaviour {
		public void OnButtonClick() {
			isPasswordHidden = !isPasswordHidden;
			ModifyPasswordStuff();
		}

		internal bool IsPasswordHidden {
			get => isPasswordHidden;
			set {
				isPasswordHidden = value;
				ModifyPasswordStuff();
			}
		}

		[SerializeField]
		private bool isPasswordHidden;

		[SerializeField]
		private Image showHidePasswordImg;

		[SerializeField]
		private Sprite passwordShownSprite;

		[SerializeField]
		private Sprite passwordHiddenSprite;

		[SerializeField]
		private TextMeshProUGUI showHidePasswordTmp;

		[SerializeField]
		private string passwordShownText;

		[SerializeField]
		private string passwordHiddenText;

		[SerializeField]
		private TMP_InputField inputField;

		#if UNITY_EDITOR

		private void OnValidate() {
			if(!EditorApplication.isPlayingOrWillChangePlaymode) {
				ModifyPasswordStuff();
			}
		}

		#endif

		private void ModifyPasswordStuff() {
			if(isPasswordHidden) {
				if(showHidePasswordImg != null) {
					showHidePasswordImg.sprite = passwordHiddenSprite;
				}

				if(showHidePasswordTmp != null) {
					showHidePasswordTmp.SetText(passwordHiddenText);
				}

				if(inputField != null) {
					inputField.contentType = ContentType.Password;
				}
			} else {
				if(showHidePasswordImg != null) {
					showHidePasswordImg.sprite = passwordShownSprite;
				}

				if(showHidePasswordTmp != null) {
					showHidePasswordTmp.SetText(passwordShownText);
				}

				if(inputField != null) {
					inputField.contentType = ContentType.Standard;
				}
			}

			inputField?.textComponent.SetAllDirty();
		}
	}
}