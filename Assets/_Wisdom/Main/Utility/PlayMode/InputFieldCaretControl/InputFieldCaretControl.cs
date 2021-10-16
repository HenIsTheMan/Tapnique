using TMPro;
using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed class InputFieldCaretControl: MonoBehaviour {
		[SerializeField]
		private TMP_InputField inputField;

		private void Awake() {
			Transform caretTransform = FindCaretTransform(inputField.transform);

			if(caretTransform == null) {
				UnityEngine.Assertions.Assert.IsTrue(false);
				return;
			}

			caretTransform.SetSiblingIndex(caretTransform.parent.childCount - 1);
		}

		private Transform FindCaretTransform(Transform myTransform) {
			foreach(Transform childTransform in myTransform) {
				if(childTransform.name == "Caret") {
					return childTransform;
				}

				return FindCaretTransform(childTransform);
			}

			return null;
		}
	}
}