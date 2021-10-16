using UnityEngine;
using UnityEngine.UI;
using static IWP.General.InitIDs;
using static IWP.General.ScreenModes;

namespace IWP.General {
    internal sealed class FullscreenToggle: MonoBehaviour {
        #region Fields

		[SerializeField]
		private InitControl initControl;

		[SerializeField]
		private Toggle toggle;

		[SerializeField]
		private ScreenMode fullscreenScreenMode;

		[SerializeField]
		private ScreenMode notFullscreenScreenMode;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal FullscreenToggle(): base() {
			initControl = null;

			toggle = null;

			fullscreenScreenMode = ScreenMode.Amt;
			notFullscreenScreenMode = ScreenMode.Amt;
		}

        static FullscreenToggle() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		private void OnValidate() {
			UnityEngine.Assertions.Assert.AreNotEqual(
				fullscreenScreenMode, ScreenMode.Amt,
				"fullscreenScreenMode, ScreenMode.Amt"
			);
			UnityEngine.Assertions.Assert.AreNotEqual(
				notFullscreenScreenMode, ScreenMode.Amt,
				"notFullscreenScreenMode, ScreenMode.Amt"
			);
		}

		private void OnEnable() {
			initControl.AddMethod((uint)InitID.FullscreenToggle, Init);
		}

		private void OnDisable() {
			initControl.RemoveMethod((uint)InitID.FullscreenToggle, Init);
		}

		#endregion

		private void Init() {
			if(ScreenManager.globalObj == null) { //Lame
				return;
			}

			toggle.isOn
				= ScreenManager.globalObj.Mode == ScreenMode.ExclusiveFullscreen
				|| ScreenManager.globalObj.Mode == ScreenMode.FullscreenWindow;

			toggle.onValueChanged.AddListener((isOn) => {
				ScreenManager.globalObj.Mode = isOn ? fullscreenScreenMode : notFullscreenScreenMode;
			});
		}
	}
}