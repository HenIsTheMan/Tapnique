#if UNITY_EDITOR

using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static IWP.Anim.AnimAccessTypes;

namespace IWP.Anim {
	internal abstract class AbstractAnimEditor: Editor {
		#region Fields

		protected AbstractAnim myScript;

		protected string[] names;
		private int count;

		private List<SerializedProperty> serializedProperties;

		private List<GUIContent> GUIContents;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal AbstractAnimEditor(): base() {
			myScript = null;

			names = System.Array.Empty<string>();
			count = 0;

			serializedProperties = null;

			GUIContents = null;
		}

		static AbstractAnimEditor() {
		}

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected abstract void InitNames();

		public override void OnInspectorGUI() {
			DrawDefaultInspector();

			myScript = target as AbstractAnim;

			InitNames();

			serializedProperties = new List<SerializedProperty>();
			GUIContents = new List<GUIContent>();
			int namesLen = names.Length;
			SerializedProperty serializedProperty;
			string serializedPropertyName;

			for(int i = 0; i < namesLen; ++i) {
				serializedProperty = serializedObject.FindProperty(names[i]);

				System.Type myType = myScript.GetType();
				FieldInfo fieldInfoShldAnimateX = myType.GetField("shldAnimateX");
				var endProperty = serializedProperty.GetEndProperty();

				if(fieldInfoShldAnimateX != null && serializedProperty.propertyType == SerializedPropertyType.Vector3) {
					while(serializedProperty.Next(true)) {
						serializedPropertyName = serializedProperty.name;

						if(
							serializedPropertyName != "x"
							&& serializedPropertyName != "y"
							&& serializedPropertyName != "z"
						) {
							break;
						}

						if(
							(serializedPropertyName == "x" && (bool)fieldInfoShldAnimateX.GetValue(myScript))
							|| (serializedPropertyName == "y" && (bool)myType.GetField("shldAnimateY").GetValue(myScript))
							|| (serializedPropertyName == "z" && (bool)myType.GetField("shldAnimateZ").GetValue(myScript))
						) {
							serializedProperties.Add(serializedProperty.Copy());
							GUIContents.Add(new GUIContent(names[i] + serializedPropertyName.ToUpper()));
						}

						if(SerializedProperty.EqualContents(serializedProperty, endProperty)) {
							break;
						}
					}
				} else {
					if(myScript.countThreshold == 1 && names[i] == "periodicDelay") {
						continue;
					}

					serializedProperties.Add(serializedProperty);
					GUIContents.Add(new GUIContent(names[i]));
				}
			}
			count = serializedProperties.Count;

			if(PrefabUtility.IsPartOfAnyPrefab(myScript)) {
				PrefabUtility.RecordPrefabInstancePropertyModifications(myScript);
			}

			switch(myScript.AccessType) {
				case AnimAccessType.Editor:
					serializedObject.UpdateIfRequiredOrScript();

					for(int i = 0; i < count; ++i) {
						_ = EditorGUILayout.PropertyField(serializedProperties[i], GUIContents[i]);
					}

					_ = serializedObject.ApplyModifiedProperties();

					break;
				case AnimAccessType.Script:
					break;
				case AnimAccessType.Amt:
					if(GUI.changed) {
						General.Console.LogError("myScript.AccessType == AnimAccessType.Amt");
					}

					break;
			}
		}
	}
}

#endif