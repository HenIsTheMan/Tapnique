#if UNITY_EDITOR

using System.IO;
using UnityEngine;

namespace Genesis.Wisdom {
	[ExecuteInEditMode]
	internal sealed class FolderCreator: EditModeTaskPerformer {
		[SerializeField]
		private bool shldReplaceIfExists;

		[SerializeField]
		private string folderPath;

		protected override void OnEnable() {
			if(Directory.Exists(folderPath)) {
				if(!shldReplaceIfExists) {
					TaskPerformanceOutcome("Folder Creation Failure!");
					return;
				}

				Directory.Delete(folderPath, true);
			}

			Directory.CreateDirectory(folderPath);
			TaskPerformanceOutcome("Folder Creation Success!");
		}
    }
}

#endif