using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Genesis.Creation {
    internal sealed class NoobProgress: MonoBehaviour {
		[SerializeField]
		private float noProgressVal;

		[SerializeField]
		private float halfProgressVal;

		[SerializeField]
		private float fullProgressVal;

		[SerializeField]
		private Image progressBarFgImg;

		[SerializeField]
		private TMP_Text progressTmp;

		[SerializeField]
		private GameObject checkMarkGO0;

		[SerializeField]
		private GameObject checkMarkGO1;

		private int val;

		private void OnEnable() {
			val = PlayerPrefs.GetInt(nameof(val), 0);

			switch(val) {
				case 0:
					((RectTransform)progressBarFgImg.transform).offsetMax
						= new Vector2(
							-noProgressVal,
							((RectTransform)progressBarFgImg.transform).offsetMax.y
						);

					progressTmp.text = "0%";
					break;
				case 1:
					((RectTransform)progressBarFgImg.transform).offsetMax
						= new Vector2(
							-halfProgressVal,
							((RectTransform)progressBarFgImg.transform).offsetMax.y
						);

					progressTmp.text = "50%";

					checkMarkGO0.SetActive(true);

					break;
				case 2:
					((RectTransform)progressBarFgImg.transform).offsetMax
						= new Vector2(
							-fullProgressVal,
							((RectTransform)progressBarFgImg.transform).offsetMax.y
						);

					progressTmp.text = "100%";

					checkMarkGO0.SetActive(true);
					checkMarkGO1.SetActive(true);

					break;
				default:
					UnityEngine.Assertions.Assert.IsTrue(false, "val is invalid!");
					break;
			}
		}

		private void OnDisable() {
			PlayerPrefs.SetInt(nameof(val), val + 1);
		}
	}
}