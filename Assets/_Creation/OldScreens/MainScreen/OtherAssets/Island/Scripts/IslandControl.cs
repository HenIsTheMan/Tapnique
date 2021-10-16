using TMPro;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.Creation {
	internal sealed partial class IslandControl: MonoBehaviour {
		internal bool IsLocked {
			get => isLocked;
		}

		internal Renderer SelectedIslandRenderer {
			get => islandRenderers[(int)islandMeshType];
		}

		[SerializeField]
		private bool isLocked;

		[SerializeField]
		private GameObject lockGO;

		[SerializeField]
		private IslandMeshType islandMeshType;

		[SerializeField]
		private Renderer[] islandRenderers;

		[ColorUsage(true, false)]
		[SerializeField]
		private Color lockedColor;

		[ColorUsage(true, false)]
		[SerializeField]
		private Color unlockedColor;

		[SerializeField]
		private float lockedAlpha;

		[SerializeField]
		private float unlockedAlpha;

		[SerializeField]
		private Transform islandTransform;

		[SerializeField]
		private float islandScaleFactor;

		[SerializeField]
		private TMP_Text islandStarCostTmp;

		[SerializeField]
		private uint islandStarCost;

		[SerializeField]
		private TMP_Text islandNameTmp;

		[SerializeField]
		private string islandName;

		[SerializeField]
		private TMP_Text islandDifficultyLvlTmp;

		[SerializeField]
		private IslandDifficultyLvl islandDifficultyLvl;

		private void OnValidate() {
			if(!Application.isPlaying) {
				if(islandTransform != null) {
					islandTransform.localScale = new Vector3(
						islandScaleFactor,
						islandScaleFactor,
						islandScaleFactor
					);
				}

				if(islandStarCostTmp != null) {
					islandStarCostTmp.text = islandStarCost.ToString();
				}

				if(islandNameTmp != null) {
					islandNameTmp.text = islandName;
				}

				if(islandDifficultyLvlTmp != null) {
					islandDifficultyLvlTmp.text = islandDifficultyLvl.MyName;
					islandDifficultyLvlTmp.color = islandDifficultyLvl.MyColor;
				}
			}

			#if UNITY_EDITOR

			if(!EditorApplication.isPlayingOrWillChangePlaymode) {
				EditorApplication.CallbackFunction myCallbackFunc = null;

				myCallbackFunc += () => {
					//* This way prevents branching
					foreach(Renderer islandRenderer in islandRenderers) {
						islandRenderer.gameObject.SetActive(false);
					}
					SelectedIslandRenderer.gameObject.SetActive(true);
					//*/

					LockUnlock();

					EditorApplication.delayCall -= myCallbackFunc;
				};

				EditorApplication.delayCall += myCallbackFunc;
			}

			#endif
		}

		internal void Lock() {
			isLocked = true;
			LockUnlock();
		}

		internal void Unlock() {
			isLocked = false;
			LockUnlock();
		}

		private void LockUnlock() {
			if(islandRenderers == null || islandRenderers.Length != (int)IslandMeshType.Amt) {
				return;
			}

			if(isLocked) {
				if(SelectedIslandRenderer != null) {
					SelectedIslandRenderer.sharedMaterial.color = lockedColor;
				}

				if(islandDifficultyLvlTmp != null) {
					islandDifficultyLvlTmp.alpha = lockedAlpha;
				}

				if(islandNameTmp != null) {
					islandNameTmp.alpha = lockedAlpha;
				}
			} else {
				if(SelectedIslandRenderer != null) {
					SelectedIslandRenderer.sharedMaterial.color = unlockedColor;
				}

				if(islandDifficultyLvlTmp != null) {
					islandDifficultyLvlTmp.alpha = unlockedAlpha;
				}

				if(islandNameTmp != null) {
					islandNameTmp.alpha = unlockedAlpha;
				}
			}

			lockGO?.SetActive(isLocked);
		}
	}
}