#if !USING_PUN2
	#define USING_PUN2
#endif

#if USING_PUN2
	using Photon.Pun;
#endif

#if !USING_MIRROR
	//#define USING_MIRROR
#endif

#if USING_MIRROR
	using Mirror;
#endif

using TMPro;
using UnityEngine;

namespace IWP.General {
    internal sealed class DebugText: MonoBehaviour {
		#region Fields

		[SerializeField]
		private TextMeshProUGUI tmpComponent;

		[SerializeField]
		private bool isRGB;

		[SerializeField]
		private float rgbFactor;

		private float hue;

		[SerializeField]
		private bool showFPS;

		[SerializeField]
		private bool showUnscaledFPS;

		[SerializeField]
		private bool showElapsedTime;

		[SerializeField]
		private bool showUnscaledElapsedTime;

		[SerializeField]
		private bool showPing;

		#if USING_MIRROR
			[SerializeField]
			private bool showTimeSD;
		#endif

		[SerializeField]
		private int dpForFPS;

		[SerializeField]
		private int dpForUnscaledFPS;

		[SerializeField]
		private int dpForElapsedTime;

		[SerializeField]
		private int dpForUnscaledElapsedTime;

		[SerializeField]
		private int dpForPing;

		#if USING_MIRROR
			[SerializeField]
			private int dpForTimeSD;
		#endif

		[SerializeField]
		private string fpsFrontText;

		[SerializeField]
		private string unscaledFpsFrontText;

		[SerializeField]
		private string elapsedTimeFrontText;

		[SerializeField]
		private string unscaledElapsedTimeFrontText;

		[SerializeField]
		private string pingFrontText;

		#if USING_MIRROR
			[SerializeField]
			private string timeSDFrontText;
		#endif

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal DebugText(): base() {
			tmpComponent = null;

			isRGB = true;
			rgbFactor = 1.0f;
			hue = 0.0f;

			showFPS = true;
			showUnscaledFPS = true;
			showElapsedTime = true;
			showUnscaledElapsedTime = true;
			showPing = true;

			#if USING_MIRROR
				showTimeSD = true;
			#endif

			dpForFPS = 2;
			dpForUnscaledFPS = 2;
			dpForElapsedTime = 2;
			dpForUnscaledElapsedTime = 2;
			dpForPing = 2;

			#if USING_MIRROR
				dpForTimeSD = 2;
			#endif

			fpsFrontText = string.Empty;
			unscaledFpsFrontText = string.Empty;
			elapsedTimeFrontText = string.Empty;
			unscaledElapsedTimeFrontText = string.Empty;
			pingFrontText = string.Empty;

			#if USING_MIRROR
				timeSDFrontText = string.Empty;
			#endif
        }

        static DebugText() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		private void Awake() { //I guess
			tmpComponent.text = string.Empty;
		}

		private void Update() {
			tmpComponent.text = string.Empty;

			if(isRGB) {
				hue += Time.unscaledDeltaTime * rgbFactor;

				if(hue > 1.0f) {
					hue = 0.0f;
				}

				tmpComponent.color = Color.HSVToRGB(hue, 1.0f, 1.0f, false);
			}

			if(showFPS) {
				tmpComponent.text += fpsFrontText + ' ' + (1.0f / Time.deltaTime).ToString($"F{dpForFPS}") + '\n';
			}

			if(showUnscaledFPS) {
				tmpComponent.text += unscaledFpsFrontText + ' ' + (1.0f / Time.unscaledDeltaTime).ToString($"F{dpForUnscaledFPS}") + '\n';
			}

			if(showElapsedTime) {
				tmpComponent.text += elapsedTimeFrontText + ' ' + Time.time.ToString($"F{dpForElapsedTime}") + '\n';
			}

			if(showUnscaledElapsedTime) {
				tmpComponent.text += unscaledElapsedTimeFrontText + ' ' + Time.unscaledTime.ToString($"F{dpForUnscaledElapsedTime}") + '\n';
			}

		#if USING_PUN2
			if(showPing && PhotonNetwork.NetworkingClient.AppId != null) {
				tmpComponent.text += pingFrontText + ' ' + PhotonNetwork.GetPing().ToString($"F{dpForPing}") + '\n';
			}
		#elif USING_MIRROR
			if(showPing && Mirror.NetworkManager.singleton.isNetworkActive) {
				tmpComponent.text += pingFrontText + ' ' + NetworkTime.rtt.ToString($"F{dpForPing}") + '\n';
			}

			if(showTimeSD && Mirror.NetworkManager.singleton.isNetworkActive) {
				tmpComponent.text += timeSDFrontText + ' ' + NetworkTime.rtt.ToString($"F{dpForTimeSD}") + '\n';
			}
		#endif
		}

        #endregion
    }
}