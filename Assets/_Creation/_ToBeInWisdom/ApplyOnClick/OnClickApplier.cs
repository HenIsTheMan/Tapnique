#if UNITY_EDITOR

using Genesis.Wisdom;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

namespace Genesis.Creation {
	[ExecuteInEditMode]
	internal sealed class OnClickApplier: EditModeTaskPerformer {
		[System.Serializable]
		internal struct OnClickSet {
			[field: SerializeField]
			internal Button MyButton {
				get;
				private set;
			}

			[field: SerializeField]
			internal UnityEvent MyUnityEvent {
				get;
				private set;
			}
		}

		[SerializeField]
		private OnClickSet[] onClickSetArr;

		protected override void OnEnable() {
			if(onClickSetArr == null) {
				TaskPerformanceOutcome("ApplyOnClick Failure!");
				return;
			}

			int i = 0;
			foreach(OnClickSet onClickSet in onClickSetArr) {
				if(onClickSet.MyButton == null) {
					continue;
				}

				onClickSet.MyButton.onClick = new ButtonClickedEvent();

				UnityEventTools.AddIntPersistentListener(
					onClickSet.MyButton.onClick,
					Run,
					i++
				);
			}

			TaskPerformanceOutcome("ApplyOnClick Success!");

			EditorSceneManager.SaveScene(gameObject.scene);
		}

		public void Run(int index) {
			onClickSetArr[index].MyUnityEvent?.Invoke();
		}
	}
}

#endif