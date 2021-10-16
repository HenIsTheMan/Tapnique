#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Genesis.Wisdom {
	[ExecuteInEditMode]
	internal sealed class MtlChanger: EditModeTaskPerformer {
		[SerializeField]
		private bool shldReplaceIfExists;

		[SerializeField]
		private GameObject prefabRoot;

		[SerializeField]
		private Material srcMtl;

		[SerializeField]
		private bool shldCreateNewInstancesOfMtl;

		[SerializeField]
		private string dstMtlNamePrefix;

		[SerializeField]
		private string dstMtlNamePostfix;

		[SerializeField]
		private string dstMtlFolderPath;

		protected override void OnEnable() {
			string prefabFilePath = AssetDatabase.GetAssetPath(prefabRoot);
			GameObject prefab = PrefabUtility.LoadPrefabContents(prefabFilePath);

			Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
			Material myMtl;
			Texture myTex;

			if(shldCreateNewInstancesOfMtl) {
				string srcMtlFilePath = AssetDatabase.GetAssetPath(srcMtl);

				foreach(Renderer myRenderer in renderers) {
					string oldDstFilePath
						= dstMtlFolderPath
						+ myRenderer.sharedMaterial.name
						+ ".mat";

					myTex = myRenderer.sharedMaterial.mainTexture;

					if(File.Exists(oldDstFilePath)) {
						if(!shldReplaceIfExists) {
							TaskPerformanceOutcome("ChangeMtls Failure!");
							return;
						}

						_ = AssetDatabase.CopyAsset(srcMtlFilePath, oldDstFilePath);
						myMtl = AssetDatabase.LoadAssetAtPath<Material>(oldDstFilePath);
					} else {
						myMtl = new Material(srcMtl) {
							mainTexture = myTex
						};

						AssetDatabase.CreateAsset(myMtl,
							dstMtlFolderPath
							+ dstMtlNamePrefix
							+ myRenderer.sharedMaterial.name
							+ dstMtlNamePostfix
							+ ".mat"
						);
					}

					myRenderer.sharedMaterial = myMtl;
				}
			} else {
				foreach(Renderer myRenderer in renderers) {
					myTex = myRenderer.sharedMaterial.mainTexture;
					myRenderer.sharedMaterial = srcMtl;
					myRenderer.sharedMaterial.mainTexture = myTex;
				}
			}

			PrefabUtility.SaveAsPrefabAsset(prefab, prefabFilePath);
			PrefabUtility.UnloadPrefabContents(prefab);

			TaskPerformanceOutcome("ChangeMtls Success!");
		}
	}
}

#endif