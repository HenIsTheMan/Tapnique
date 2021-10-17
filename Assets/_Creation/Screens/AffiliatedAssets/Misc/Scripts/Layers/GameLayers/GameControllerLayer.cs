using Genesis.Wisdom;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Genesis.Creation {
    internal sealed class GameControllerLayer: Singleton<GameControllerLayer> {
		internal void ConfigGameButton(GameButtonLink gameButtonLink) {
			EventTrigger eventTrigger = gameButtonLink.MyEventTrigger;
			eventTrigger.triggers.Clear();

			EventTrigger.Entry ptrDownEntry = new EventTrigger.Entry {
				eventID = EventTriggerType.PointerDown
			};
			ptrDownEntry.callback.AddListener((_) => {
				OnPtrDownHandler(gameButtonLink);
			});

			EventTrigger.Entry ptrUpEntry = new EventTrigger.Entry {
				eventID = EventTriggerType.PointerUp
			};
			ptrUpEntry.callback.AddListener((_) => {
				OnPtrUpHandler(gameButtonLink);
			});

			eventTrigger.triggers.Add(ptrDownEntry);
			eventTrigger.triggers.Add(ptrUpEntry);
		}

		private void OnPtrDownHandler(GameButtonLink gameButtonLink) {
			gameButtonLink.PtrUpAnim.StopAnim();
			gameButtonLink.PtrDownAnim.StartAnim(true);
		}

		private void OnPtrUpHandler(GameButtonLink gameButtonLink) {
			gameButtonLink.PtrDownAnim.StopAnim();
			gameButtonLink.PtrUpAnim.StartAnim(true);

			_ = StartCoroutine(PtrUpOverCoroutine(gameButtonLink));
		}

		private IEnumerator PtrUpOverCoroutine(GameButtonLink gameButtonLink) {
			yield return new WaitForSeconds(gameButtonLink.PtrUpAnim.animDuration);

			GameModelLayer.GlobalObj.ButtonOnClick(gameButtonLink.gameObject);
		}
	}
}