using UnityEngine;

namespace IWP.Anim {
    internal sealed class TimeScaleChanger: MonoBehaviour {
		[SerializeField]
		private float myTimeScale;

		private void OnValidate() {
			if(Application.isPlaying) {
				Time.timeScale = myTimeScale;
			}
		}
    }
}