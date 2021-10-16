#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Genesis.Wisdom {
	[ExecuteInEditMode]
    internal sealed class ScriptReplicator: EditModeTaskPerformer {
		[SerializeField]
		private bool shldReplaceIfExists;

		[SerializeField]
		private string dstFolderPath;

		[SerializeField]
		private string refFolderPath;

		[SerializeField]
		private Object srcScript;

		[SerializeField]
		private int insertAtSrcIndex;

		[SerializeField]
		private int startRefIndex;

		[SerializeField]
		private int endVal;

		[SerializeField]
		private bool shldUseEndValAsOffset;

		protected override void OnEnable() {
			if(Directory.Exists(dstFolderPath)) {
				if(!shldReplaceIfExists) {
					TaskPerformanceOutcome("Script Replication Failure!");
					return;
				}

				Directory.Delete(dstFolderPath, true);
			}
			Directory.CreateDirectory(dstFolderPath);

			StreamReader streamReader;
			string srcFilePath = AssetDatabase.GetAssetPath(srcScript);
			string[] refFilePaths = Directory.GetFiles(refFolderPath);

			foreach(string refFilePath in refFilePaths) {
				if(Path.GetExtension(refFilePath) == ".cs") {
					Object refScript = AssetDatabase.LoadAssetAtPath<Object>(refFilePath);
					string dstFilePath = dstFolderPath + refScript.name + ".cs";

					streamReader = new StreamReader(refFilePath);
					string refFileStr = streamReader.ReadToEnd();
					string[] refLines = refFileStr.Split('\n');
					streamReader.Close();

					streamReader = new StreamReader(srcFilePath);
					string srcFileStr = streamReader.ReadToEnd();
					string[] srcLines = srcFileStr.Split('\n');
					streamReader.Close();

					using var fileStream = new FileStream(dstFilePath, FileMode.Append);
					using var streamWriter = new StreamWriter(fileStream);
					int i;

					for(i = 0; i < insertAtSrcIndex; ++i) {
						streamWriter.Write(srcLines[i] + '\n');
					}

					int limit = shldUseEndValAsOffset ? refLines.Length - endVal : endVal;
					for(i = startRefIndex; i < limit; ++i) {
						streamWriter.Write(refLines[i] + '\n');
					}

					streamWriter.Write(string.Join("\n", srcLines, insertAtSrcIndex, srcLines.Length - insertAtSrcIndex));

					streamWriter.Close();
					AssetDatabase.ImportAsset(dstFilePath);
				}
			}

			TaskPerformanceOutcome("Script Replication Success!");
		}
	}
}

#endif