using UnityEngine;
using static IWP.General.InitIDs;

namespace IWP.General {
	[DisallowMultipleComponent]
	internal sealed class InitControl: MonoBehaviour {
		internal delegate void InitDelegate();

		#region Fields

		private readonly uint size;
		private readonly InitDelegate[] initDelegates;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal InitControl(): base() {
			size = (uint)InitID.Amt;

			initDelegates = new InitDelegate[size];

			for(uint i = 0; i < size; ++i) {
				initDelegates[i] = null;
			}
		}

		static InitControl() {
		}

		#endregion

		#region Unity User Callback Event Funcs

		private void Start() {
			for(uint i = 0; i < size; ++i) {
				initDelegates[i]?.Invoke();
			}
		}

		#endregion

		internal void AddMethod(uint i, InitDelegate initDelegate) {
			initDelegates[i] += initDelegate;
		}

		internal void RemoveMethod(uint i, InitDelegate initDelegate) {
			initDelegates[i] -= initDelegate;
		}
	}
}