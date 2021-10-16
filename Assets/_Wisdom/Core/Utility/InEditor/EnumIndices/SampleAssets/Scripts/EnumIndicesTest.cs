#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Genesis.Wisdom {
    internal sealed partial class EnumIndicesTest: MonoBehaviour {
		[EnumIndices(typeof(MyEnum), typeof(MyOtherEnum))]
		[SerializeField]
		private List<string> strContainer;

		[field: EnumIndices(" . ", typeof(MyEnum), typeof(MyOtherEnum))]
		[field: SerializeField]
		private string[] OtherStrContainer {
			get;
			set;
		}

		[EnumIndicesForColor(true, true, true, typeof(MyEnum), typeof(MyOtherEnum))]
		[SerializeField]
		private Color[] colorContainer0;

		[EnumIndicesForColor(true, true, true, typeof(MyEnum), typeof(MyOtherEnum))]
		[SerializeField]
		private List<Color> colorContainer1;

		private void OnValidate() {
			if(EditorApplication.isPlayingOrWillChangePlaymode
				|| strContainer == null
				|| OtherStrContainer == null
			) {
				return;
			}

			int count = strContainer.Count;
			for(int i = 0; i < count; ++i) {
				strContainer[i] = i.ToString();
			}

			int len = OtherStrContainer.Length;
			for(int i = 0; i < len; ++i) {
				OtherStrContainer[i] = i.ToString();
			}
		}
    }
}

#endif