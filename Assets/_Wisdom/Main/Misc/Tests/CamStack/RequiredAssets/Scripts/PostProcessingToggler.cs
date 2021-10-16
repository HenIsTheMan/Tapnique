using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Genesis.Wisdom {
    internal sealed class PostProcessingToggler: MonoBehaviour {
		[SerializeField]
		private UniversalAdditionalCameraData[] allUniversalAdditionalCamData;

		[SerializeField]
		private int startIndex;

		[SerializeField]
		private float delay;

		private void OnEnable() {
			_ = StartCoroutine(nameof(MyCoroutine));
		}

		private void OnDisable() {
			StopCoroutine(nameof(MyCoroutine));
			TogglePostProcessing(startIndex);
		}

		private System.Collections.IEnumerator MyCoroutine() {
			WaitHelper.AddWaitForSeconds(delay);

			int index = startIndex;
			int allUniversalAdditionalCamDataLen = allUniversalAdditionalCamData.Length;

			while(true) {
				DebugHelper.ClearConsole();
				Debug.Log(index, gameObject);

				TogglePostProcessing(index);

				if(++index > allUniversalAdditionalCamDataLen - 1) {
					index = -2;
				}

				yield return WaitHelper.GetWaitForSeconds(delay);
			}
		}

		private void TogglePostProcessing(int index) {
			if(index < 0) {
				foreach(UniversalAdditionalCameraData universalAdditionalCamData in allUniversalAdditionalCamData) {
					universalAdditionalCamData.renderPostProcessing = index == -1;
				}
			} else {
				foreach(UniversalAdditionalCameraData universalAdditionalCamData in allUniversalAdditionalCamData) {
					universalAdditionalCamData.renderPostProcessing = false;
				}
				allUniversalAdditionalCamData[index].renderPostProcessing = true;
			}
		}
	}
}