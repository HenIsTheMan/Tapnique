using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Genesis.Creation {
	internal sealed class AdminTabButtonSelectionSupport: MonoBehaviour {
		[System.Serializable]
		internal struct GameObjActivitySet {
			[field: SerializeField]
			internal GameObject GameObj {
				get;
				private set;
			}

			[field: SerializeField]
			internal bool IsActiveWhenSelected {
				get;
				private set;
			}
		}

		[System.Serializable]
		internal struct ImgSet {
			[field: SerializeField]
			internal Image Img {
				get;
				private set;
			}

			[field: SerializeField]
			internal Material Mtl {
				get;
				private set;
			}

			[field: SerializeField]
			internal Material SelectedMtl {
				get;
				private set;
			}
		}

		[System.Serializable]
		internal struct TextSet {
			[field: SerializeField]
			internal TMP_Text Text {
				get;
				private set;
			}

			[field: SerializeField]
			internal Material Mtl {
				get;
				private set;
			}

			[field: SerializeField]
			internal Material SelectedMtl {
				get;
				private set;
			}
		}

		[SerializeField]
		private GameObjActivitySet[] gameObjActivitySetArr;

		[SerializeField]
		private ImgSet[] imgSetArr;

		[SerializeField]
		private TextSet[] textSetArr;

		public void OnSelected() {
			foreach(GameObjActivitySet gameObjActivitySet in gameObjActivitySetArr) {
				gameObjActivitySet.GameObj.SetActive(gameObjActivitySet.IsActiveWhenSelected);
			}

			foreach(ImgSet imgSet in imgSetArr) {
				imgSet.Img.material = imgSet.SelectedMtl;
			}

			foreach(TextSet textSet in textSetArr) {
				textSet.Text.fontSharedMaterial = textSet.SelectedMtl;
			}
		}

		public void OnDeselected() {
			foreach(GameObjActivitySet gameObjActivitySet in gameObjActivitySetArr) {
				gameObjActivitySet.GameObj.SetActive(!gameObjActivitySet.IsActiveWhenSelected);
			}

			foreach(ImgSet imgSet in imgSetArr) {
				imgSet.Img.material = imgSet.Mtl;
			}

			foreach(TextSet textSet in textSetArr) {
				textSet.Text.fontSharedMaterial = textSet.Mtl;
			}
		}
    }
}