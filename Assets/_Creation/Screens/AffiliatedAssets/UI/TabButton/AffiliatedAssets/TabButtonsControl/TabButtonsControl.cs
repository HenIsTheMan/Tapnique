using Genesis.Wisdom;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_EDITOR

using UnityEditor.SceneManagement;
using UnityEditor;

#endif

namespace Genesis.Creation {
	internal sealed class TabButtonsControl: MonoBehaviour {
		public void SelectFunc(int index) {
			if(tabButtonsControlUnitArr == null
				|| index < 0
				|| index > tabButtonsControlUnitArr.Length - 1
			) {
				UnityEngine.Assertions.Assert.IsTrue(false);
				return;
			}

			Select(index);
		}

		public void DeselectFunc(int index) {
			if(tabButtonsControlUnitArr == null
				|| index < 0
				|| index > tabButtonsControlUnitArr.Length - 1
			) {
				UnityEngine.Assertions.Assert.IsTrue(false);
				return;
			}

			Deselect(index, true);
		}

		[Serializable]
		internal sealed class TabButtonsControlUnit {
			[UnmodifiableInInspector(AttribWorkingPeriod.PlayMode)]
			[SerializeField]
			internal bool isSelected;

			[field: UnmodifiableInInspector(AttribWorkingPeriod.PlayMode)]
			[field: SerializeField]
			internal bool CanDeselect {
				get;
				private set;
			}

			[field: UnmodifiableInInspector(AttribWorkingPeriod.PlayMode)]
			[field: SerializeField]
			[SerializeField]
			internal Button TabButton {
				get;
				private set;
			}

			[field: SerializeField]
			internal UnityEvent SelectedUnityEvent {
				get;
				private set;
			}

			[field: SerializeField]
			internal UnityEvent DeselectedUnityEvent {
				get;
				private set;
			}
		}

		internal List<TabButtonsControlUnit> SelectedTabButtonsControlUnit {
			get {
				List<TabButtonsControlUnit> tabButtonsControlUnitList = new List<TabButtonsControlUnit>(selectedIndices.Count);

				selectedIndices.ForEach((selectedIndex) => {
					tabButtonsControlUnitList.Add(tabButtonsControlUnitArr[selectedIndex]);
				});

				return tabButtonsControlUnitList;
			}
		}

		internal List<TabButtonsControlUnit> UnselectedTabButtonsControlUnit {
			get {
				List<TabButtonsControlUnit> tabButtonsControlUnitList = new List<TabButtonsControlUnit>(unselectedIndices.Count);

				unselectedIndices.ForEach((selectedIndex) => {
					tabButtonsControlUnitList.Add(tabButtonsControlUnitArr[selectedIndex]);
				});

				return tabButtonsControlUnitList;
			}
		}

		[SerializeField]
		private bool canSelectDeselectMultiple;

		[SerializeField]
		private bool shldCallUnityEventsInEditMode;

		[SerializeField]
		private TabButtonsControlUnit[] tabButtonsControlUnitArr;

		[UnmodifiableInInspector]
		[SerializeField]
		private List<int> selectedIndices;

		[UnmodifiableInInspector]
		[SerializeField]
		private List<int> unselectedIndices;

		#if UNITY_EDITOR

		private async void OnValidate() {
			if(EditorApplication.isPlayingOrWillChangePlaymode || tabButtonsControlUnitArr == null) {
				return;
			}

			int len = tabButtonsControlUnitArr.Length;
			selectedIndices = new List<int>(len);
			unselectedIndices = new List<int>(len);

			int selectedCount = 0;
			for(int i = 0; i < len; ++i) {
				if(!canSelectDeselectMultiple && selectedCount > 1) {
					break;
				}

				if(tabButtonsControlUnitArr[i].isSelected) {
					selectedIndices.Add(i);
					_ = ++selectedCount;
				} else {
					unselectedIndices.Add(i);
				}
			}

			if(!canSelectDeselectMultiple && selectedCount > 1) {
				selectedIndices.Clear();
				unselectedIndices.Clear();

				for(int i = 0; i < len; ++i) {
					tabButtonsControlUnitArr[i].isSelected = false;
					unselectedIndices.Add(i);
				}
			}

			UnityEvent unityEvent;

			UnityEventCallState unityEventCallState
				= shldCallUnityEventsInEditMode
				? UnityEventCallState.EditorAndRuntime
				: UnityEventCallState.RuntimeOnly;

			for(int i = 0; i < len; ++i) {
				unityEvent = tabButtonsControlUnitArr[i].SelectedUnityEvent;
				ConfigUnityEvent(unityEvent);

				if(tabButtonsControlUnitArr[i].isSelected) {
					unityEvent?.Invoke();
				}

				unityEvent = tabButtonsControlUnitArr[i].DeselectedUnityEvent;
				ConfigUnityEvent(unityEvent);

				if(!tabButtonsControlUnitArr[i].isSelected) {
					unityEvent?.Invoke();
				}
			}

			void ConfigUnityEvent(UnityEvent myUnityEvent) {
				if(myUnityEvent != null) {
					int count = myUnityEvent.GetPersistentEventCount();

					for(int j = 0; j < count; ++j) {
						myUnityEvent.SetPersistentListenerState(j, unityEventCallState);
					}
				}
			}

			await System.Threading.Tasks.Task.Delay(4);

			EditorSceneManager.SaveScene(gameObject.scene);
		}

		#endif

		private void Awake() {
			int len = tabButtonsControlUnitArr.Length;
			for(int i = 0; i < len; ++i) {
				int index = i;
				tabButtonsControlUnitArr[i].TabButton.onClick.AddListener(() => {
					SelectDeselect(index);
				});

				(tabButtonsControlUnitArr[i].isSelected
					? tabButtonsControlUnitArr[i].SelectedUnityEvent
					: tabButtonsControlUnitArr[i].DeselectedUnityEvent)
					?.Invoke();
			}
		}

		private void SelectDeselect(int index) {
			if(tabButtonsControlUnitArr[index].isSelected) {
				Deselect(index);
			} else {
				Select(index);
			}
		}

		private void Select(int index) {
			TabButtonsControlUnit tabButtonsControlUnit = tabButtonsControlUnitArr[index];

			tabButtonsControlUnit.isSelected = true;
			tabButtonsControlUnit.SelectedUnityEvent?.Invoke();

			if(unselectedIndices.Remove(index)) {
				selectedIndices.Add(index);
			}

			if(!canSelectDeselectMultiple && selectedIndices.Count == 2) {
				Deselect(selectedIndices[0], true);
			}
		}

		private void Deselect(int index, bool shldBypassCanDeselect = false) {
			TabButtonsControlUnit tabButtonsControlUnit = tabButtonsControlUnitArr[index];
			if(!shldBypassCanDeselect && !tabButtonsControlUnit.CanDeselect) {
				return;
			}

			tabButtonsControlUnit.isSelected = false;
			tabButtonsControlUnit.DeselectedUnityEvent?.Invoke();

			if(selectedIndices.Remove(index)) {
				unselectedIndices.Add(index);
			}
		}
	}
}