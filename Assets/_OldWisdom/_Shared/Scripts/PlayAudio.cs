using UnityEngine;

namespace IWP.General {
    internal sealed class PlayAudio: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        internal PlayAudio(): base() {
        }

        static PlayAudio() {
        }

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		public void PlayMusic(string name) {
			AudioManager.globalObj.PlayMusic(name);
		}

		public void PlaySound(string name) {
			AudioManager.globalObj.PlaySound(name);
		}
    }
}