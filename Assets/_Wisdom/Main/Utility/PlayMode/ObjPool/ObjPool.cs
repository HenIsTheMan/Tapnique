using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class ObjPool: MonoBehaviour {
		internal List<GameObject> ActiveObjs {
			get => activeObjs;
		}

		internal List<GameObject> InactiveObjs {
			get => inactiveObjs;
		}

		internal void InitMe(int size, [JetBrains.Annotations.NotNull] GameObject prefab, Transform parentTransform, string instanceName) {
			activeObjs = new List<GameObject>(size);
			inactiveObjs = new List<GameObject>(size);

			for(int i = 0; i < size; ++i) {
				GameObject GO = Instantiate(prefab, parentTransform);

				GO.SetActive(false);
				GO.name = instanceName;

				inactiveObjs.Add(GO);
			}
		}

		internal GameObject ActivateObj() {
			GameObject GO = inactiveObjs[0];

			GO.SetActive(true);
			inactiveObjs.RemoveAt(0);
			activeObjs.Add(GO);

			return GO;
		}

		internal void DeactivateObj(GameObject obj) {
			GameObject GO = activeObjs.Where(x => x == obj).SingleOrDefault();

			if(GO != null) {
				GO.SetActive(false);
				inactiveObjs.Add(GO);
				_ = activeObjs.Remove(GO);
			}
		}

		internal List<GameObject> ActivateAllObjs() {
			List<GameObject> activatedGameObjs = new List<GameObject>(inactiveObjs.Count);

			foreach(GameObject inactiveObj in inactiveObjs) {
				inactiveObj.SetActive(true);
				activatedGameObjs.Add(inactiveObj);
			}

			inactiveObjs.Clear();
			activeObjs.AddRange(activatedGameObjs);

			return activatedGameObjs;
		}

		internal void DeactivateAllObjs() {
			foreach(GameObject activeObj in activeObjs) {
				activeObj.SetActive(false);
				inactiveObjs.Add(activeObj);
			}

			activeObjs.Clear();
		}

		private List<GameObject> activeObjs;
		private List<GameObject> inactiveObjs;
	}
}