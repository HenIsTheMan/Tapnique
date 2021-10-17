using UnityEngine;

namespace Genesis.Wisdom {
    internal abstract class Singleton<T>: MonoBehaviour where T:
		MonoBehaviour
	{
		internal static T GlobalObj {
			get {
				lock(myLock) {
					if(globalObj != null) {
						return globalObj;
					}

					T[] objs = FindObjectsOfType<T>();

					if(objs.Length > 1) {
						UnityEngine.Assertions.Assert.IsTrue(false, "objs.Length > 1");
					}

					return objs.Length == 1 ? objs[0] : null;
				}
			}
		}

		private protected virtual void OnValidate() {
			if(Application.isPlaying) {
				lock(myLock) {
					if(FindObjectsOfType<T>().Length > 1) {
						Debug.LogError(typeof(T).ToString() + ": FindObjectsOfType<T>().Length > 1", null);
					}
				}
			}
		}

		private static readonly object myLock = new object();

		private static T globalObj = null;
	}
}