using System.Collections.Generic;

namespace Genesis.Wisdom {
	internal static class ShuffleElements {
		internal static void Shuffle<T>(this IList<T> container) {
			FisherYatesShuffle(container);
		}

		internal static void FisherYatesShuffle<T>(this IList<T> container) {
			for(int i = container.Count - 1; i > 0; --i) {
				ValHelper.Swap(ref container, UnityEngine.Random.Range(0, i + 1), i);
			}
		}
	}
}