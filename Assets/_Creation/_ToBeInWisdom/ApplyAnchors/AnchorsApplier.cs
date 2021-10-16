using Genesis.Wisdom;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor.SceneManagement;
using UnityEditor;

#endif

namespace Genesis.Creation {
	[ExecuteInEditMode]
	internal sealed class AnchorsApplier: EditModeTaskPerformer {
		internal enum PseudoAnchorPreset: uint {
			None,
			Top,
			Middle,
			Bottom,
			Left,
			Center,
			Right,
			TopLeft,
			TopCenter,
			TopRight,
			MiddleLeft,
			MiddleCenter,
			MiddleRight,
			BottomLeft,
			BottomCenter,
			BottomRight,
			FullStretch,
			Existing,
			All
		}

		[Flags]
		internal enum AnchorPreset: uint {
			None,
			Top = 1u << 0,
			Middle = 1u << 1,
			Bottom = 1u << 2,
			Left = 1u << 3,
			Center = 1u << 4,
			Right = 1u << 5,
			TopLeft = Top | Left,
			TopCenter = Top | Center,
			TopRight = Top | Right,
			MiddleLeft = Middle | Left,
			MiddleCenter = Middle | Center,
			MiddleRight = Middle | Right,
			BottomLeft = Bottom | Left,
			BottomCenter = Bottom | Center,
			BottomRight = Bottom | Right,
			FullStretch = BottomRight + 1,
			Existing = FullStretch + 1,
			All = uint.MaxValue
		}

		[Serializable]
		internal struct ChildAnchorPresetSet {
			[UnmodifiableInInspector]
			[SerializeField]
			internal RectTransform childRectTransform;

			internal AnchorPreset MyAnchorPreset {
				get {
					return (AnchorPreset)Enum.Parse(
						typeof(AnchorPreset),
						Enum.GetName(typeof(PseudoAnchorPreset), anchorPreset)
					);
				}
			}

			[EnumRange(
				typeof(PseudoAnchorPreset),
				nameof(PseudoAnchorPreset.TopLeft),
				nameof(PseudoAnchorPreset.Existing)
			)]
			[SerializeField]
			internal PseudoAnchorPreset anchorPreset;
		}

		[Serializable]
		internal sealed class ParentChildrenSet {
			[field: SerializeField]
			internal RectTransform ParentRectTransform {
				get;
				private set;
			}

			[field: SerializeField]
			internal List<ChildAnchorPresetSet> ChildAnchorPresetSetList {
				get;
				private set;
			} = new List<ChildAnchorPresetSet>();
		}

		[SerializeField]
		private bool shldWorldSpacePosChange;

		[SerializeField]
		private ParentChildrenSet[] parentChildrenSetArr;

		#if UNITY_EDITOR

		[ContextMenu("Refresh")]
		private void OnValidate() {
			if(EditorApplication.isPlayingOrWillChangePlaymode || parentChildrenSetArr == null) {
				return;
			}

			//* Remove null childRectTransforms
			foreach(ParentChildrenSet myParentChildrenSet in parentChildrenSetArr) {
				if(myParentChildrenSet.ParentRectTransform == null) {
					continue;
				}

				foreach(RectTransform childRectTransform in myParentChildrenSet.ParentRectTransform) {
					List<ChildAnchorPresetSet> childAnchorPresetSetToRemoveList
						= new List<ChildAnchorPresetSet>(myParentChildrenSet.ChildAnchorPresetSetList.Count);

					foreach(ChildAnchorPresetSet childAnchorPresetSet in myParentChildrenSet.ChildAnchorPresetSetList) {
						if(childAnchorPresetSet.childRectTransform == null){
							childAnchorPresetSetToRemoveList.Add(childAnchorPresetSet);
						}
					}

					foreach(ChildAnchorPresetSet childAnchorPresetSet in childAnchorPresetSetToRemoveList) {
						_ = myParentChildrenSet.ChildAnchorPresetSetList.Remove(childAnchorPresetSet);
					}
				}
			}
			//*/

			int parentChildrenSetArrLen = parentChildrenSetArr.Length;
			Dictionary<RectTransform, PseudoAnchorPreset>[] myDictArr
				= new Dictionary<RectTransform, PseudoAnchorPreset>[parentChildrenSetArrLen];
			ParentChildrenSet parentChildrenSet;

			for(int i = 0; i < parentChildrenSetArrLen; ++i) {
				parentChildrenSet = parentChildrenSetArr[i];

				if(parentChildrenSet.ParentRectTransform == null) {
					continue;
				}

				myDictArr[i] = new Dictionary<RectTransform, PseudoAnchorPreset>(parentChildrenSet.ChildAnchorPresetSetList.Count);

				foreach(ChildAnchorPresetSet childAnchorPresetSet in parentChildrenSet.ChildAnchorPresetSetList) {
					myDictArr[i].Add(childAnchorPresetSet.childRectTransform, childAnchorPresetSet.anchorPreset);
				}
			}

			for(int i = 0; i < parentChildrenSetArrLen; ++i) {
				parentChildrenSet = parentChildrenSetArr[i];
				parentChildrenSet.ChildAnchorPresetSetList.Clear();

				if(parentChildrenSet.ParentRectTransform == null) {
					continue;
				}

				foreach(RectTransform childRectTransform in parentChildrenSet.ParentRectTransform) {
					parentChildrenSet.ChildAnchorPresetSetList.Add(new ChildAnchorPresetSet() {
						childRectTransform = childRectTransform,
						anchorPreset = myDictArr[i].ContainsKey(childRectTransform)
							? myDictArr[i][childRectTransform]
							: PseudoAnchorPreset.MiddleCenter
					});
				}
			}
		}

		#endif

		protected override void OnEnable() {
			if(parentChildrenSetArr == null) {
				TaskPerformanceOutcome("ApplyAnchors Failure!");
				return;
			}

			float x, y;
			AnchorPreset childAnchorPreset;

			foreach(ParentChildrenSet parentChildrenSet in parentChildrenSetArr) {
				foreach(ChildAnchorPresetSet childAnchorPresetSet in parentChildrenSet.ChildAnchorPresetSetList) {
					childAnchorPreset = childAnchorPresetSet.MyAnchorPreset;

					if(childAnchorPreset == AnchorPreset.Existing) {
						continue;
					}

					if(childAnchorPreset == AnchorPreset.FullStretch) {
						childAnchorPresetSet.childRectTransform.anchorMin = Vector2.zero;
						childAnchorPresetSet.childRectTransform.anchorMax = Vector2.one;
						childAnchorPresetSet.childRectTransform.sizeDelta = Vector2.zero;
						continue;
					}

					if(childAnchorPreset.HasFlag(AnchorPreset.Left)) {
						x = 0.0f;
					} else if(childAnchorPreset.HasFlag(AnchorPreset.Right)) {
						x = 1.0f;
					} else {
						x = 0.5f;
					}

					if(childAnchorPreset.HasFlag(AnchorPreset.Bottom)) {
						y = 0.0f;
					} else if(childAnchorPreset.HasFlag(AnchorPreset.Top)) {
						y = 1.0f;
					} else {
						y = 0.5f;
					}

					if(shldWorldSpacePosChange) {
						childAnchorPresetSet.childRectTransform.anchorMin
							= childAnchorPresetSet.childRectTransform.anchorMax
							= new Vector2(x, y);
					} else {
						double tempWorldSpacePosX = childAnchorPresetSet.childRectTransform.position.x;
						double tempWorldSpacePosY = childAnchorPresetSet.childRectTransform.position.y;
						double tempWorldSpacePosZ = childAnchorPresetSet.childRectTransform.position.z;

						childAnchorPresetSet.childRectTransform.anchorMin
							= childAnchorPresetSet.childRectTransform.anchorMax
							= new Vector2(x, y);

						childAnchorPresetSet.childRectTransform.position = new Vector3(
							(float)tempWorldSpacePosX,
							(float)tempWorldSpacePosY,
							(float)tempWorldSpacePosZ
						);
					}
				}
			}

			TaskPerformanceOutcome("ApplyAnchors Success!");

			_ = StartCoroutine(nameof(SaveSceneSoon));
		}

		private IEnumerator SaveSceneSoon() { //For anchoredPos to adjust 1st
			yield return WaitHelper.MyWaitForEndOfFrame;

			#if UNITY_EDITOR

			EditorSceneManager.SaveScene(gameObject.scene);

			#endif
		}
	}
}