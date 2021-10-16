using IWP.Anim;
using UnityEngine;
using UnityEngine.UI;
using static IWP.Anim.AbstractAnim;
using static IWP.Anim.AnimAccessTypes;
using static IWP.General.InitIDs;
using static IWP.Math.EasingTypes;

namespace IWP.General {
    internal sealed class ImgCrossfader: MonoBehaviour {
        #region Fields

		[SerializeField]
		private InitControl initControl;

		private int imgGOsLen;

		private ImgFadeAnim[] fadeAnims;

		private GameObject[] containerGOs;

		[SerializeField]
		private GameObject[] imgGOs;

		private int currIndex;

		[SerializeField]
		private float startTimeOffset;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private EasingType fadeEasingType;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal ImgCrossfader(): base() {
			initControl = null;

			imgGOsLen = 0;

			fadeAnims = System.Array.Empty<ImgFadeAnim>();

			containerGOs = System.Array.Empty<GameObject>();
			imgGOs = System.Array.Empty<GameObject>();

			currIndex = 0;

			startTimeOffset = 0.0f;
			animDuration = 0.0f;

			fadeEasingType = EasingType.Amt;
		}

        static ImgCrossfader() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		private void OnEnable() {
			initControl.AddMethod((uint)InitID.ImgCrossfader, Init);
		}

		private void OnDisable() {
			initControl.RemoveMethod((uint)InitID.ImgCrossfader, Init);
		}

		#endregion

		private void Init() {
			GameObject containerGO;
			GameObject imgGO;
			ImgFadeAnim fadeAnim;
			Image img;
			imgGOsLen = imgGOs.Length;

			containerGOs = new GameObject[imgGOsLen];
			fadeAnims = new ImgFadeAnim[imgGOsLen];

			for(int i = 0; i < imgGOsLen; ++i) {
				containerGO = new GameObject("Container" + i.ToString(), typeof(RectTransform));
				containerGO.transform.SetParent(transform, false);
				containerGOs[i] = containerGO;

				RectTransform myRectTransform = (RectTransform)containerGO.transform;
				myRectTransform.anchorMin = Vector2.zero;
				myRectTransform.anchorMax = Vector2.one;
				myRectTransform.sizeDelta = Vector2.zero;
			}

			for(int i = 0; i < imgGOsLen; ++i) {
				imgGO = imgGOs[i];
				imgGO.transform.SetParent(containerGOs[imgGOsLen - 1 - i].transform, false);
				img = imgGO.GetComponent<Image>();

				if(img == null) {
					Console.LogError("Img component not found!", imgGO);
				}

				if(imgGO.GetComponent<ImgFadeAnim>() != null) {
					Console.LogError("ImgFadeAnim component found!", imgGO);
				}
				
				fadeAnim = imgGO.AddComponent<ImgFadeAnim>();

				fadeAnim.AccessType = AnimAccessType.Script;
				//fadeAnim.initControl = null;
				//fadeAnim.isUpdating = false;
				//fadeAnim.shldInitVals = true;
				//fadeAnim.periodicDelay = 0.0f;
				fadeAnim.startTimeOffset = startTimeOffset;
				fadeAnim.animDuration = animDuration;
				fadeAnim.countThreshold = 1;
				fadeAnim.easingType = fadeEasingType;
				fadeAnim.startAlpha = 1.0f;
				fadeAnim.endAlpha = 0.0f;
				fadeAnim.img = img;

				fadeAnim.InitMe();
				fadeAnims[i] = fadeAnim;
			}

			AnimDelegate fadeEndDelegate = null;
			fadeAnim = fadeAnims[currIndex];

			fadeEndDelegate += () => {
				fadeAnim.animEndDelegate -= fadeEndDelegate;

				int nextContainerIndex;
				string str;
				for(int i = 0; i < imgGOsLen; ++i) {
					str = fadeAnims[i].gameObject.transform.parent.name;
					str = str.Substring(str.Length - 1);
					nextContainerIndex = (int.Parse(str) + 1) % imgGOsLen;
					fadeAnims[i].gameObject.transform.SetParent(containerGOs[nextContainerIndex].transform);
				}

				Color color = fadeAnim.img.color;
				color.a = 1.0f;
				fadeAnim.img.color = color;

				currIndex = (currIndex + 1) % imgGOsLen;
				fadeAnim = fadeAnims[currIndex];

				fadeAnim.animEndDelegate += fadeEndDelegate;
				fadeAnim.IsUpdating = true;
			};

			fadeAnim.animEndDelegate += fadeEndDelegate;
			fadeAnim.IsUpdating = true;
		}
	}
}