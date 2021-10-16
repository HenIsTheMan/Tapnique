#if UNITY_EDITOR

using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Genesis.Wisdom {
	[CustomPropertyDrawer(typeof(EnumIndicesForColorAttribute))]
	internal sealed class EnumIndicesForColorPropertyDrawer: EnumIndicesPropertyDrawer {
		public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
			if(!typeof(IEnumerable).IsAssignableFrom(fieldInfo.FieldType)) { //If not a container...
				Debug.LogError("!typeof(IEnumerable).IsAssignableFrom(fieldInfo.FieldType)", null);
				return;
			}

			ProcessFunc<EnumIndicesForColorAttribute>(property, label);

			EnumIndicesForColorAttribute propertyAttrib = attribute as EnumIndicesForColorAttribute;

			property.colorValue = EditorGUI.ColorField(
				pos,
				label,
				property.colorValue,
				propertyAttrib.shldShowEyedropper,
				propertyAttrib.shldShowAlpha,
				propertyAttrib.isHDR
			);
		}
    }
}

#endif