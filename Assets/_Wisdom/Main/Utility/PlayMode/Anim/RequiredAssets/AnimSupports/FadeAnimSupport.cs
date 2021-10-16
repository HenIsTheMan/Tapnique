using UnityEngine;

namespace Genesis.Wisdom {
	[System.Serializable]
	internal sealed class FadeAnimSupport {
		[Range(0.0f, 1.0f)]
		[SerializeField]
		internal float startAlpha;

		[Range(0.0f, 1.0f)]
		[SerializeField]
		internal float endAlpha;
	}
}