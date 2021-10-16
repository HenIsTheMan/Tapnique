#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed class FileExplorerTest: MonoBehaviour {
		[SerializeField]
		private string fileName;

		[SerializeField]
		private string title;

		[SerializeField]
		private string directory;

		[SerializeField]
		private string extension;

		[SerializeField]
		private string defaultName;

		private void Awake() {
			System.Diagnostics.Process p = new System.Diagnostics.Process();
			p.StartInfo = new System.Diagnostics.ProcessStartInfo(fileName);
			p.Start();

			string selectedAbsFilePath;

			selectedAbsFilePath = EditorUtility.OpenFilePanel(title, directory, extension);
			Debug.Log(selectedAbsFilePath, gameObject);

			selectedAbsFilePath = EditorUtility.SaveFilePanel(title, directory, defaultName, extension);
			Debug.Log(selectedAbsFilePath, gameObject);
		}
    }
}

#endif