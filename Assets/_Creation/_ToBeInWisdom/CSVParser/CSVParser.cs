using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Genesis.Creation {
	internal sealed class CSVParser: MonoBehaviour {
		internal List<List<string[]>> RowResults {
			get;
			private set;
		}

		internal void Parse() {
			int filePathsLen = filePaths.Length;
			StreamReader streamReader;
			RowResults = new List<List<string[]>>(filePathsLen);

			string[] lineEndings = new string[] {
				"\r\n",
				"\r",
				"\n"
			};

			int i = 0;
			foreach(string filePath in filePaths) {
				if(Path.GetExtension(filePath) != ".csv") {
					UnityEngine.Assertions.Assert.IsTrue(false);
					return;
				}

				//Object script = AssetDatabase.LoadAssetAtPath<Object>(filePath);

				streamReader = new StreamReader(filePath);
				string fileStr = streamReader.ReadToEnd();

				string[] lines = fileStr.Split(
					lineEndings,
					System.StringSplitOptions.None
				);

				RowResults.Add(new List<string[]>(lines.Length));

				foreach(string line in lines) {
					RowResults[i].Add(line.Split(
						",".ToCharArray(),
						System.StringSplitOptions.None
					));
				}

				streamReader.Close();
				++i;
			}
		}

		[SerializeField]
		private string[] filePaths;
	}
}