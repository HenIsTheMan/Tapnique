#if UNITY_EDITOR

using System.IO;
using UnityEngine;

namespace Genesis.Wisdom {
	[ExecuteInEditMode]
	internal sealed class ScreenToImgConverter: EditModeTaskPerformer {
		[SerializeField]
		private bool shldReplaceIfExists;

		[SerializeField]
		private Camera cam;

		[SerializeField]
		private RenderTexture renderTex;

		[SerializeField]
		private string imgFileName;

		[SerializeField]
		private string imgFileExtension;

		[SerializeField]
		private string imgFolderPath;

		protected override void OnEnable() {
			string imgFilePath = imgFolderPath + imgFileName + '.' + imgFileExtension;

			if(!shldReplaceIfExists && File.Exists(imgFilePath)) {
				TaskPerformanceOutcome("ScreenToImg Failure!");
				return;
			}

			if(!Directory.Exists(imgFolderPath)) {
				Directory.CreateDirectory(imgFolderPath);
			}

			cam.targetTexture = renderTex;
			RenderTexture currRenderTex = RenderTexture.active;
			cam.targetTexture.Release();
			RenderTexture.active = cam.targetTexture;
			cam.Render();

			Texture2D imgTex = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.ARGB32, false);
			imgTex.ReadPixels(new Rect(0.0f, 0.0f, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
			imgTex.Apply();
			cam.targetTexture = null;

			RenderTexture.active = currRenderTex;
			byte[] bytesPNG = imgTex.EncodeToPNG();
			File.WriteAllBytes(imgFilePath, bytesPNG);

			TaskPerformanceOutcome("ScreenToImg Success!");
		}
	}
}

#endif