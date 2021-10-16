#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Genesis.Wisdom {
	[CustomPropertyDrawer(typeof(UnmodifiableInInspectorAttribute))]
	internal sealed class UnmodifiableInInspectorPropertyDrawer: PropertyDrawer {
		public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
			UnmodifiableInInspectorAttribute propertyAttrib = attribute as UnmodifiableInInspectorAttribute;

			if((propertyAttrib.attribWorkingPeriod == AttribWorkingPeriod.EditMode && Application.isPlaying)
				|| (propertyAttrib.attribWorkingPeriod == AttribWorkingPeriod.PlayMode && !Application.isPlaying)
			) {
				EditorGUI.PropertyField(pos, property, label, true);
				return;
			}

			bool prevVal = GUI.enabled;

			GUI.enabled = false;

			EditorGUI.PropertyField(pos, property, label, true);

			GUI.enabled = prevVal;
		}
    }
}

#endif