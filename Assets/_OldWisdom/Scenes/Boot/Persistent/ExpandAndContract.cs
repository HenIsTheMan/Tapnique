using IWP.Anim;
using UnityEngine;

namespace IWP.General {
    internal sealed class ExpandAndContract: MonoBehaviour {
        #region Fields

		[SerializeField]
		internal AbstractAnim[] animsForExpansion;

		[SerializeField]
		internal AbstractAnim[] animsForContraction;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal ExpandAndContract(): base() {
			animsForExpansion = System.Array.Empty<AbstractAnim>();
			animsForContraction = System.Array.Empty<AbstractAnim>();
		}

        static ExpandAndContract() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		private void Awake() {
			foreach(AbstractAnim anim in animsForExpansion) {
				anim.InitMe();
			}

			foreach(AbstractAnim anim in animsForContraction) {
				anim.InitMe();
			}
		}

		#endregion

		internal void Expand() {
			foreach(AbstractAnim anim in animsForExpansion) {
				anim.IsUpdating = true;
			}

			foreach(AbstractAnim anim in animsForContraction) {
				anim.IsUpdating = false;
			}
		}

		internal void Contract() {
			foreach(AbstractAnim anim in animsForExpansion) {
				anim.IsUpdating = false;
			}

			foreach(AbstractAnim anim in animsForContraction) {
				anim.IsUpdating = true;
			}
		}
    }
}