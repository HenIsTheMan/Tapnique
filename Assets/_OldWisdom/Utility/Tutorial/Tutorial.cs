using UnityEngine;
using static IWP.General.InitIDs;

namespace IWP.General {
    internal sealed class Tutorial: MonoBehaviour {
		#region Fields

		[SerializeField]
		private InitControl initControl;

		private int currIndex;

		[SerializeField]
		private CanvasGroup tutorialCanvasGrp;

		[SerializeField]
		private TutorialData[] myTutorialData;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        internal Tutorial(): base() {
			initControl = null;

			currIndex = 0;

			tutorialCanvasGrp = null;

			myTutorialData = System.Array.Empty<TutorialData>();
        }

        static Tutorial() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		private void OnEnable() {
			initControl.AddMethod((uint)InitID.Tutorial, Init);
		}

		private void Update() {
			if(Input.GetMouseButtonDown(0)) {
				foreach(Rect rect in myTutorialData[currIndex].rects) {
					if(rect.Contains(Input.mousePosition)) {
						_ = myTutorialData[currIndex].rects.Remove(rect);
						break;
					}
				}

				//tutorialCanvasGrp.blocksRaycasts = false;
				//Clicker.Click(Input.mousePosition.x, Input.mousePosition.y);
				//tutorialCanvasGrp.blocksRaycasts = true;

				if(myTutorialData[currIndex].rects.Count == 0) {
					if(currIndex < myTutorialData.Length - 1) {
						++currIndex;
						Console.Log(myTutorialData[currIndex].str, gameObject);
					} else {
						tutorialCanvasGrp.transform.parent.gameObject.SetActive(false);
					}
				}
			}
		}

		private void OnDisable() {
			initControl.RemoveMethod((uint)InitID.Tutorial, Init);
		}

		#endregion

		private void Init() {
			Console.Log(myTutorialData[currIndex].str, gameObject);
		}
	}
}