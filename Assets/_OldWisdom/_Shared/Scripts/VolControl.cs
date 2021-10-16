using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static IWP.General.InitIDs;

namespace IWP.General {
	internal sealed class VolControl: MonoBehaviour {
		#region Fields

		[SerializeField]
		private InitControl initControl;

		[SerializeField]
		private bool isMusic;

		[SerializeField]
		private Slider slider;

		[SerializeField]
		private TextMeshProUGUI tmpComponent;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal VolControl(): base() {
			initControl = null;

			isMusic = true;

			slider = null;

			tmpComponent = null;
		}

		static VolControl() {
		}

		#endregion

		#region Unity User Callback Event Funcs

		private void OnEnable() {
			initControl.AddMethod((uint)InitID.VolControl, Init);
		}

		private void OnDisable() {
			initControl.RemoveMethod((uint)InitID.VolControl, Init);
		}

		#endregion

		private void Init() {
			slider.value = isMusic ? AudioManager.globalObj.MusicVol : AudioManager.globalObj.SoundVol;
			tmpComponent.text = (int)(slider.value * 100.0f) + "%";

			slider.onValueChanged.AddListener(delegate {
				OnSliderValChange();
			});
		}

		private void OnSliderValChange() {
			tmpComponent.text = (int)(slider.value * 100.0f) + "%";

			if(isMusic) {
				AudioManager.globalObj.AdjustVolOfAllMusic(slider.value);
			} else {
				AudioManager.globalObj.AdjustVolOfAllSounds(slider.value);
			}
		}
	}
}