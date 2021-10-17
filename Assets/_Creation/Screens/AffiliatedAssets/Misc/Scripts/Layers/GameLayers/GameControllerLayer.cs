using UnityEngine;
using UnityEngine.EventSystems;

namespace Genesis.Creation {
    internal sealed class GameControllerLayer: MonoBehaviour {
		internal static void ConfigGameButton(GameObject gameButtonGameObj) {
			GameButtonLink gameButtonLink = gameButtonGameObj.GetComponent<GameButtonLink>();

			EventTrigger eventTrigger = gameButtonLink.MyEventTrigger;
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
				gameButtonLink.PtrUpAnim.StopAnim();
				gameButtonLink.PtrDownAnim.StartAnim(true);
			}

			void OnPtrUpHandler() {
				gameButtonLink.PtrDownAnim.StopAnim();
				gameButtonLink.PtrUpAnim.StartAnim(true);
			}
		}
	}
}