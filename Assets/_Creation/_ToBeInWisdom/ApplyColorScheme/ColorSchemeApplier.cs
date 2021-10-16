#if UNITY_EDITOR

using Genesis.Wisdom;
using TMPro;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.Creation {
	[ExecuteInEditMode]
	internal sealed class ColorSchemeApplier: EditModeTaskPerformer {
		[SerializeField]
		private ColorScheme colorScheme;

		[System.Serializable]
		internal struct MtlSet {
			internal enum MtlType: byte {
				Default,
				Font,
				Amt
			}

			internal static readonly int[] shaderPropertyIDs = new int[(int)MtlType.Amt]{
				Shader.PropertyToID("_Color"),
				ShaderUtilities.ID_FaceColor
			};

			[field: SerializeField]
			internal Material Mtl {
				get;
				private set;
			}

			[field: EnumRange(typeof(MtlType), 0, (int)MtlType.Amt)]
			[field: SerializeField]
			internal MtlType MyMtlType {
				get;
				private set;
			}
		}

		[System.Serializable]
		internal sealed class MtlSetArrWrapper {
			[field: SerializeField]
			internal MtlSet[] MtlSetArr {
				get;
				private set;
			}
		}

		[SerializeField]
		private MtlSetArrWrapper[] mtlSetArrWrapperArr;

		[ContextMenu("Refresh")]
		private void OnValidate() {
			if(EditorApplication.isPlayingOrWillChangePlaymode || colorScheme == null) {
				mtlSetArrWrapperArr = null;
				return;
			}

			int oldLen = mtlSetArrWrapperArr.Length;
			MtlSetArrWrapper[] oldMtlSetArrWrapperArr = new MtlSetArrWrapper[oldLen];
			System.Array.Copy(mtlSetArrWrapperArr, oldMtlSetArrWrapperArr, oldLen);
			mtlSetArrWrapperArr = new MtlSetArrWrapper[colorScheme.ColorArr.Length];

			int smallerLen = Mathf.Min(mtlSetArrWrapperArr.Length, oldLen);
			for(int i = 0; i < smallerLen; ++i) {
				mtlSetArrWrapperArr[i] = oldMtlSetArrWrapperArr[i];
			}
		}

		protected override void OnEnable() {
			if(colorScheme == null || mtlSetArrWrapperArr == null) {
				TaskPerformanceOutcome("ApplyColorScheme Failure!");
				return;
			}

			int colorArrLen = colorScheme.ColorArr.Length;
			Color color;
			MtlSet[] mtlSetArr;

			for(int i = 0; i < colorArrLen; ++i) {
				color = colorScheme.ColorArr[i];
				mtlSetArr = mtlSetArrWrapperArr[i].MtlSetArr;

				if(mtlSetArr == null) {
					continue;
				}

				foreach(MtlSet mtlSet in mtlSetArr) {
					mtlSet.Mtl.SetColor(MtlSet.shaderPropertyIDs[(int)mtlSet.MyMtlType], color);
				}
			}

			TaskPerformanceOutcome("ApplyColorScheme Success!");
		}
	}
}

#endif