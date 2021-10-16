using TMPro;
using UnityEngine;
using static IWP.General.ConnectionStatuses;
using static IWP.General.InitIDs;

namespace IWP.General {
    internal sealed class NetworkText: MonoBehaviour {
		#region Fields

		[SerializeField]
		private InitControl initControl;

		[SerializeField]
		private TextMeshProUGUI tmpComponent;

		[SerializeField]
		private string waitingText;

		[SerializeField]
		private string connectingText;

		[SerializeField]
		private string connectedText;

		[SerializeField]
		private string joiningLobbyText;

		[SerializeField]
		private string joinedLobbyText;

		[SerializeField]
		private string joiningRoomText;

		[SerializeField]
		private string joinedRoomText;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal NetworkText(): base() {
			initControl = null;

			tmpComponent = null;

			waitingText = string.Empty;

			connectingText = string.Empty;
			connectedText = string.Empty;

			joiningLobbyText = string.Empty;
			joinedLobbyText = string.Empty;

			joiningRoomText = string.Empty;
			joinedRoomText = string.Empty;
		}

        static NetworkText() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		private void OnEnable() {
			initControl.AddMethod((uint)InitID.NetworkText, Init);
		}

		private void OnDisable() {
			initControl.RemoveMethod((uint)InitID.NetworkText, Init);
		}

		#endregion

		private void Init() {
			_ = StartCoroutine(nameof(ManageText));
		}

		private System.Collections.IEnumerator ManageText() {
			while(NetworkManager.globalObj == null) {
				yield return null;
			}

			while(true) {
				switch(NetworkManager.globalObj.MyConnectionStatus) {
					case ConnectionStatus.Waiting:
						tmpComponent.text = waitingText;
						break;
					case ConnectionStatus.Connecting:
						tmpComponent.text = connectingText;
						break;
					case ConnectionStatus.Connected:
						tmpComponent.text = connectedText;
						break;
					case ConnectionStatus.JoiningLobby:
						tmpComponent.text = joiningLobbyText;
						break;
					case ConnectionStatus.JoinedLobby:
						tmpComponent.text = joinedLobbyText;
						break;
					case ConnectionStatus.JoiningRoom:
						tmpComponent.text = joiningRoomText;
						break;
					case ConnectionStatus.JoinedRoom:
						tmpComponent.text = joinedRoomText;
						break;
				}

				yield return null;
			}
		}
	}
}