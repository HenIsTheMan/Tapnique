#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Genesis.Wisdom {
	[CustomPropertyDrawer(typeof(EnumIncludeExcludeAttribute))]
	internal sealed class EnumIncludeExcludePropertyDrawer: PropertyDrawer {
		public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
			if(property.propertyType != SerializedPropertyType.Enum) {
				Debug.LogError("property.propertyType != SerializedPropertyType.Enum", null);
				return;
			}

			List<int> enumValIndexList = ((EnumIncludeExcludeAttribute)attribute).enumValIndexList;
			string[] nameArr = ((EnumIncludeExcludeAttribute)attribute).nameList?.ToArray();

			if((enumValIndexList == null)
				|| (nameArr == null)
				|| (enumValIndexList.Count != nameArr.Length)
				|| (enumValIndexList.Count == 0)
				|| (nameArr.Length == 0)
			) {
				UnityEngine.Assertions.Assert.IsTrue(false);
				return;
			}

			int popupIndex = enumValIndexList.IndexOf(property.enumValueIndex);
			if(popupIndex == -1) {
				popupIndex = enumValIndexList.IndexOf(
					(Mathf.Abs(property.enumValueIndex - enumValIndexList[0])
					< Mathf.Abs(property.enumValueIndex - enumValIndexList[enumValIndexList.Count - 1]))
					? enumValIndexList[0]
					: enumValIndexList[enumValIndexList.Count - 1]
				); //Snap to closer val

				if(popupIndex == -1) {
					UnityEngine.Assertions.Assert.IsTrue(false, "popupIndex == -1");
				}
			}

			property.enumValueIndex = enumValIndexList[EditorGUI.Popup(
				pos,
				label.text,
				popupIndex,
				nameArr
			)];
		}
    }
}

#endif