using UnityEngine;
using UnityEngine.EventSystems;

namespace Genesis.Creation {
    internal sealed class CamFreeScroll: MonoBehaviour {
		private Vector3 targetPos;

		[SerializeField]
		private float camFollowSpd;

		[SerializeField]
		private Transform camTransform;

		[SerializeField]
		private float xSens;

		[SerializeField]
		private float ySens;

		private void Awake() {
			EventTrigger.Entry dragEntry = new EventTrigger.Entry {
				eventID = EventTriggerType.Drag
			};
			dragEntry.callback.AddListener((eventData) => {
				OnDragHandler((PointerEventData)eventData);
			});

			(GetComponent<EventTrigger>()
				?? gameObject.AddComponent<EventTrigger>()).triggers.Add(dragEntry);

			targetPos = camTransform.position;
		}

		private void OnDragHandler(PointerEventData ptrEventData) {
			targetPos += new Vector3(
				-ptrEventData.delta.x * xSens,
				-ptrEventData.delta.y * ySens,
				0.0f
			) * Time.deltaTime;
		}

		private void FixedUpdate() {
			camTransform.position = Vector3.Lerp(camTransform.position, targetPos, Time.fixedDeltaTime * camFollowSpd);
		}
	}
}