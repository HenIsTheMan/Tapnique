using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed class DuplicateDestroyer: MonoBehaviour {
		[SerializeField]
		private Component[] componentArr;

		private void Start() {
			List<Object> objList;

			foreach(Component component in componentArr) {
				objList = FindObjectsOfType(component.GetType()).ToList();

				if(objList.Count > 1) {
					if(!objList.Remove(component)) {
						UnityEngine.Assertions.Assert.IsTrue(false, "!objList.Remove(component)");
					}

					foreach(Object obj in objList) {
						Destroy(obj);
					}
				}
			}
		}
	}
}