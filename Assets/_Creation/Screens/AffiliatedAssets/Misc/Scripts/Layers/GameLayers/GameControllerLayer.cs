using UnityEngine;
using UnityEngine.EventSystems;

namespace Genesis.Creation {
    internal sealed class GameControllerLayer: MonoBehaviour {
		internal static void ConfigGameButton(GameObject gameButtonGameObj) {
			EventTrigger eventTrigger = gameButtonGameObj.GetComponent<EventTrigger>();
			eventTrigger.triggers.Clear();

			EventTrigger.Entry ptrDownEntry = new EventTrigger.Entry {
				eventID = EventTriggerType.PointerDown
			};
			ptrDownEntry.callback.AddListener((_) => {
				OnPtrDownHandler();
			});

			EventTrigger.Entry ptrUpEntry = new EventTrigger.Entry {
				eventID = EventTriggerType.PointerUp
			};
			ptrUpEntry.callback.AddListener((_) => {
				OnPtrUpHandler();
			});

			eventTrigger.triggers.Add(ptrDownEntry);
			eventTrigger.triggers.Add(ptrUpEntry);

			void OnPtrDownHandler() {
				print("hereDown");
			}

			void OnPtrUpHandler() {
				print("hereUp");
			}
		}
	}
}