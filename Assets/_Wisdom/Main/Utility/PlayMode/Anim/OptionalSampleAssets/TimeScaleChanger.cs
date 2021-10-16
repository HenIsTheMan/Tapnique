using UnityEngine;

namespace Genesis.Wisdom {
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