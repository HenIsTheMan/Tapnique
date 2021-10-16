#if UNITY_EDITOR

using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Genesis.Wisdom {
	[CustomPropertyDrawer(typeof(EnumIndicesAttribute))]
	internal class EnumIndicesPropertyDrawer: PropertyDrawer {
		public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
			if(!typeof(IEnumerable).IsAssignableFrom(fieldInfo.FieldType)) { //If not a container...
				Debug.LogError("!typeof(IEnumerable).IsAssignableFrom(fieldInfo.FieldType)", null);
				return;
			}

			ProcessFunc<EnumIndicesAttribute>(property, label);

			EditorGUI.PropertyField(pos, property, label, true);
		}

		protected void ProcessFunc<T>(SerializedProperty property, GUIContent label) where T:
			EnumIndicesAttribute
		{
			T propertyAttrib = attribute as T;

			int index = System.Convert.ToInt32(
				property.propertyPath.Substring(property.propertyPath.IndexOf("[")).Replace("[", "").Replace("]", "")
			);

			label.text = string.Empty;
			foreach(string[] nameSubcontainer in propertyAttrib.nameContainer) {
				if(index > nameSubcontainer.Length - 1) { //Prevents index from going out of range
					continue;
				}

				label.text += nameSubcontainer[index] + propertyAttrib.delimiter;
			}
			if(label.text != string.Empty) {
				label.text = label.text.Substring(0, label.text.Length - 2);
			}
		}
    }
}

#endif