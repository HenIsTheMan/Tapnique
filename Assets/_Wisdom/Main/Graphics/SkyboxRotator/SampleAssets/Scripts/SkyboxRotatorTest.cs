using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class SkyboxRotatorTest: MonoBehaviour {
		[Min(0.0f)]
		[SerializeField]
		private float timeScale;

		private void OnValidate() {
			if(Application.isPlaying) {
				Time.timeScale = timeScale;
			}
		}
    }
}