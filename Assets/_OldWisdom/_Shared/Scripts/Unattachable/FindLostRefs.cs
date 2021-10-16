#if UNITY_EDITOR

using System.Reflection;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace IWP.General {
    internal static class FindLostRefs {
		#region Fields

		private const string root = "Tools/FindLostRefs/";

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

        static FindLostRefs() {
		}

		#endregion

		[MenuItem(root + "InCurrScene", false, 50)]
		internal static void FindLostRefsInCurrScene() {
			FindLostRefsInGOs(GetAllSceneGOs());
		}

		[MenuItem(root + "InAllScenes", false, 51)]
		internal static void FindLostRefsInAllScenes() {
			SceneSetup[] val = EditorSceneManager.GetSceneManagerSetup();

			foreach(var myScene in EditorBuildSettings.scenes.Where(scene => scene.enabled)) {
				EditorSceneManager.OpenScene(myScene.path);
				FindLostRefsInCurrScene();
			}

			EditorSceneManager.RestoreSceneManagerSetup(val);
		}

		[MenuItem(root + "InAssets", false, 52)]
		internal static void FindLostRefsInAssets() {
			var allAssets = AssetDatabase.GetAllAssetPaths().Where(path => path.StartsWith("Assets/")).ToArray();
			var objs = allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject).Where(a => a != null).ToArray();

			FindLostRefsInGOs(objs);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
		private static void FindLostRefsInGOs(in GameObject[] GOs) {
			foreach(var GO in GOs) {
				var components = GO.GetComponents<Component>();

				foreach(var component in components) {
					if(!component) {
						Console.LogFormat(
							LogTypes.LogType.Error,
							LogOptions.LogOption.None,
							GO,
							$"Missing component {0}",
							component.GetType().FullName
						);

						continue;
					}

					SerializedObject serializedObj = new SerializedObject(component);
					var serializedProperty = serializedObj.GetIterator();

					var objRefValueMethod = typeof(SerializedProperty).GetProperty(
						"objectReferenceStringValue",
						BindingFlags.Instance
						| BindingFlags.NonPublic
						| BindingFlags.Public
					);

					string dataStr = string.Empty;

					do {
						if(serializedProperty.propertyType == SerializedPropertyType.ObjectReference) {
							string objRefStrVal = string.Empty;

							if(objRefValueMethod != null) {
								objRefStrVal = (string)objRefValueMethod.GetGetMethod(true).Invoke(serializedProperty, new object[]{});
							}

							string displayableName = ObjectNames.NicifyVariableName(serializedProperty.name);

							if(serializedProperty.objectReferenceValue == null
								&& displayableName != "Corresponding Source Object"
								&& displayableName != "Prefab Instance"
								&& displayableName != "Prefab Asset"
								&& displayableName != "Father"
								&& (serializedProperty.objectReferenceInstanceIDValue != 0
								|| objRefStrVal.StartsWith("Missing")
								|| objRefStrVal.StartsWith("None"))
							) {
								dataStr += ObjectNames.NicifyVariableName(serializedProperty.name) + ", ";
							}
						}
					} while(serializedProperty.Next(true));

					if(dataStr != string.Empty) {
						Console.Log(string.Format("[{0}] {1}: {2}", GO.name, component.GetType().Name, dataStr.Substring(0, dataStr.Length - 2)), GO);
					}
				}
			}
		}

		private static GameObject[] GetAllSceneGOs() {
			return Resources.FindObjectsOfTypeAll<GameObject>().Where(
				GO => string.IsNullOrEmpty(AssetDatabase.GetAssetPath(GO))
				&& GO.hideFlags == HideFlags.None
			).ToArray();
		}
	}
}

#endif