#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Genesis.Wisdom {
	[ExecuteInEditMode]
	internal sealed partial class LineEndingChanger: EditModeTaskPerformer {
		[SerializeField]
		private string folderPath;

		[EnumRange(typeof(LineEndingType), 0, (int)LineEndingType.Amt)]
		[SerializeField]
		private LineEndingType finalLineEndingType;

		[SerializeField]
		private SearchOption searchOption;

		protected override void OnEnable() {
			if(finalLineEndingType == LineEndingType.Mixed) {
				ChangeLineEndingMixed();
			} else {
				ChangeLineEndingNormal();
			}
		}

		private void ChangeLineEndingMixed() {
			StreamReader streamReader;
			string[] filePaths = Directory.GetFiles(folderPath, "*.*", searchOption);

			string[] lineEndings = new string[] {
				"\r\n",
				"\r",
				"\n"
			};
			int lineEndingsLen = lineEndings.Length;

			foreach(string filePath in filePaths) {
				if(Path.GetExtension(filePath) == ".cs") {
					Object script = AssetDatabase.LoadAssetAtPath<Object>(filePath);

					streamReader = new StreamReader(filePath);
					string fileStr = streamReader.ReadToEnd();

					string[] lines = fileStr.Split(
						lineEndings,
						System.StringSplitOptions.None
					);

					streamReader.Close();

					using var fileStream = new FileStream(filePath, FileMode.Truncate);
					using var streamWriter = new StreamWriter(fileStream);

					int index = -1;
					int linesLen = lines.Length;
					for(int i = 0; i < linesLen; ++i) {
						if(i == linesLen - 1) {
							streamWriter.Write(lines[i]);
						} else {
							index = Random.Range(0, (index == 1 && lines[i].Length == 0) ? lineEndingsLen - 1 : lineEndingsLen);
							streamWriter.Write(lines[i] + lineEndings[index]);
						}
					}

					streamWriter.Close();
					AssetDatabase.ImportAsset(filePath);
				}
			}

			TaskPerformanceOutcome("ChangeLineEnding Success!");
		}

		private void ChangeLineEndingNormal() {
			StreamReader streamReader;
			string[] filePaths = Directory.GetFiles(folderPath, "*.*", searchOption);

			ConfigLineEnding(finalLineEndingType, out string finalLineEnding);

			foreach(string filePath in filePaths) {
				if(Path.GetExtension(filePath) == ".cs") {
					Object script = AssetDatabase.LoadAssetAtPath<Object>(filePath);

					streamReader = new StreamReader(filePath);
					string fileStr = streamReader.ReadToEnd();
					streamReader.Close();

					using var fileStream = new FileStream(filePath, FileMode.Truncate);
					using var streamWriter = new StreamWriter(fileStream);

					streamWriter.Write(string.Join(finalLineEnding, fileStr.Split(
						new string[] {
							"\r\n",
							"\r",
							"\n",
						},
						System.StringSplitOptions.None
					)));

					streamWriter.Close();
					AssetDatabase.ImportAsset(filePath);
				}
			}

			TaskPerformanceOutcome("ChangeLineEnding Success!");
		}

		private void ConfigLineEnding(LineEndingType lineEndingType, out string lineEnding) {
			switch(lineEndingType) {
				case LineEndingType.CRLF:
					lineEnding = "\r\n";
					break;
				case LineEndingType.CR:
					lineEnding = "\r";
					break;
				case LineEndingType.LF:
					lineEnding = "\n";
					break;
				default:
					UnityEngine.Assertions.Assert.IsTrue(false, "lineEndingType is invalid!");
					lineEnding = string.Empty;
					break;
			}
		}
	}
}

#endif