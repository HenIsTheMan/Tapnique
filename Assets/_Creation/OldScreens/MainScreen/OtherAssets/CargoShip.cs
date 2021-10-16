using IWP.Anim;
using UnityEngine;
using static IWP.Anim.AbstractAnim;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.Creation.MainScreen {
    internal sealed class CargoShip: MonoBehaviour {
		internal bool IsFacingRight {
			get => isFacingRight;
			private set {
				isFacingRight = value;
				ModifyShipFacing();
			}
		}

		internal static CargoShip GlobalObj {
			get;
			private set;
		}

		internal static IslandControl MyIslandControl {
			get => myIslandControl;
			set {
				if(myIslandControl == null) {
					myIslandControl = value;

					Renderer selectedIslandRenderer = myIslandControl.SelectedIslandRenderer;
					GlobalObj.anim.startPos = GlobalObj.anim.myTransform.position;
					GlobalObj.anim.endPos = selectedIslandRenderer.transform.position;

					GlobalObj.anim.endPos -= (GlobalObj.anim.endPos - GlobalObj.anim.startPos).normalized
						* Mathf.Sqrt(Mathf.Pow(selectedIslandRenderer.bounds.size.x * 0.5f, 2.0f)
						* Mathf.Pow(selectedIslandRenderer.bounds.size.y * 0.5f, 2.0f))
						* (selectedIslandRenderer.transform.localScale.x + selectedIslandRenderer.transform.localScale.y) * 0.5f;

					GlobalObj.IsFacingRight = (GlobalObj.anim.endPos - GlobalObj.anim.startPos).x >= 0.0f;

					AnimDelegate myEndDelegate = null;
					myEndDelegate += () => {
						myIslandControl = null;

						//GlobalObj.simpleSceneTransition.TransitionScene();

						GlobalObj.anim.animEndDelegate -= myEndDelegate;
					};
					GlobalObj.anim.animEndDelegate += myEndDelegate;

					GlobalObj.anim.InitMe();
					GlobalObj.anim.IsUpdating = true;
				}
			}
		}

		private static IslandControl myIslandControl = null;

		[SerializeField]
		private bool isFacingRight;

		//[SerializeField]
		//private SimpleSceneTransition simpleSceneTransition;

		[SerializeField]
		private TransformTranslateAnim anim;

		[SerializeField]
		private Camera cam;

		#if UNITY_EDITOR

		private void OnValidate() {
			if(!EditorApplication.isPlayingOrWillChangePlaymode) {
				ModifyShipFacing();
			}
		}

		#endif

		private void Awake() {
			GlobalObj = this;
		}

		private void ModifyShipFacing() {
			if(IsFacingRight) {
				if(transform.localScale.x < 0.0f) {
					Vector3 myLocalScale = transform.localScale;
					myLocalScale.x = -myLocalScale.x;
					transform.localScale = myLocalScale;
				}
			} else {
				if(transform.localScale.x > 0.0f) {
					Vector3 myLocalScale = transform.localScale;
					myLocalScale.x = -myLocalScale.x;
					transform.localScale = myLocalScale;
				}
			}
		}
    }
}