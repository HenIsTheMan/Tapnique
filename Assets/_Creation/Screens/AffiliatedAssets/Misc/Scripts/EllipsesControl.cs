using TMPro;
using UnityEngine;

namespace Genesis.Creation {
	internal sealed class EllipsesControl: MonoBehaviour {
        #region Fields

        private float BT;
        private float elapsedTime;

        [SerializeField]
        private float delay;

		[SerializeField]
		private bool shldUseLateUpdate;

		[SerializeField]
        private TextMeshProUGUI tmpComponent;

        private int dotCount;

		[SerializeField]
		private string[] textsWithNoEllipsis;

        #endregion

        #region Properties

        internal int DotCount {
            get {
                return dotCount;
            }
        }

        internal TextMeshProUGUI TmpComponent {
            get {
                return tmpComponent;
            }
        }

        #endregion

        #region Ctors and Dtor

        internal EllipsesControl() {
            BT = 0.0f;
            elapsedTime = 0.0f;
            delay = 0.0f;
			shldUseLateUpdate = true;
			tmpComponent = null;
            dotCount = 0;
			textsWithNoEllipsis = System.Array.Empty<string>();
		}

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            UnityEngine.Assertions.Assert.IsNotNull(tmpComponent);
        }

        private void Update() {
			if(!shldUseLateUpdate) {
				MyUpdateFunc();
			}
        }

		private void LateUpdate() {
			if(shldUseLateUpdate) {
				MyLateUpdateFunc();
			}
		}

		private void OnDisable() {
			tmpComponent.text = tmpComponent.text.Substring(0, tmpComponent.text.Length - dotCount);
		}

		#endregion

		private void MyUpdateFunc() {
			elapsedTime += Time.deltaTime;

			if(tmpComponent.text.Length > 0 && BT <= elapsedTime) {
				tmpComponent.text = tmpComponent.text.Substring(0, tmpComponent.text.Length - dotCount);

				foreach(string str in textsWithNoEllipsis) {
					if(tmpComponent.text == str) {
						goto End;
					}
				}

				dotCount = dotCount == 3 ? 0 : dotCount + 1;
				for(int i = 0; i < dotCount; ++i) {
					tmpComponent.text += '.';
				}

			End:
				BT = elapsedTime + delay;
			}
		}

		private void MyLateUpdateFunc() {
			elapsedTime += Time.deltaTime;

			if(BT <= elapsedTime) {
				dotCount = dotCount == 3 ? 0 : dotCount + 1;
				BT = elapsedTime + delay;
			}

			if(tmpComponent.text.Length > 0) {
				while(tmpComponent.text.Substring(tmpComponent.text.Length - 1) == ".") {
					tmpComponent.text = tmpComponent.text.Substring(0, tmpComponent.text.Length - 1);
				}

				foreach(string str in textsWithNoEllipsis) {
					if(tmpComponent.text == str) {
						return;
					}
				}

				for(int i = 0; i < dotCount; ++i) {
					tmpComponent.text += '.';
				}
			}
		}
	}
}