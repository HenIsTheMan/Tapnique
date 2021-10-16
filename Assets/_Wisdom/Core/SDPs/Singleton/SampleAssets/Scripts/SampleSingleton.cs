using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed class SampleSingleton: Singleton<SampleSingleton> {
		[field: SerializeField]
		internal int Val {
			get;
			private set;
		}
	}
}