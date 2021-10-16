using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed class RaycastTest: MonoBehaviour {
		private void Update() {
			if(Input.GetMouseButtonDown(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if(Physics.Raycast(ray, out RaycastHit hitInfo)) {
					Debug.Log(hitInfo.transform.name);
				}
			}
		}
	}
}