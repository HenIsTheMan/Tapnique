using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Genesis.Wisdom {
	internal sealed class DuplicateDestroyer: MonoBehaviour {
		[SerializeField]
		private Component[] componentArr;

		private void Awake() {
			SceneManager.sceneLoaded += (scene, loadSceneMode) => { //hmmmm
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
			};
		}
	}
}