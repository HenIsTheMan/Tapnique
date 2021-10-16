using TMPro;
using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class InputFieldPlaceholderControl: MonoBehaviour {
		[SerializeField]
		private TMP_InputField inputField;

		public void OnInputFieldSelect() {
			inputField.placeholder.gameObject.SetActive(false);
		}

		public void OnInputFieldDeselect() {
			inputField.placeholder.gameObject.SetActive(true);
		}
	}
}