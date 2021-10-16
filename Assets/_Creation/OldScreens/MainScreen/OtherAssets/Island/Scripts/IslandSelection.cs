using UnityEngine;

namespace Genesis.Creation.MainScreen {
    internal sealed class IslandSelection: MonoBehaviour {
		[SerializeField]
		private Camera cam;

		[SerializeField]
		private LayerMask raycastLayerMask;

		[SerializeField]
		private LayerMask nonRaycastLayerMask;

		private void Update() {
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray, out RaycastHit hitInfo, cam.farClipPlane, raycastLayerMask & ~nonRaycastLayerMask)
				&& Input.GetMouseButtonUp(0)
			) {
				IslandControl islandControl = hitInfo.transform.parent.parent.GetComponent<IslandControl>();
				if(!islandControl.IsLocked) {
					CargoShip.MyIslandControl = islandControl;
				}
			}
		}
	}
}