using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using static IWP.General.ConnectionStatuses;

namespace IWP.General {
	internal sealed class NetworkManager: MonoBehaviourPunCallbacks {
		internal delegate void ConnectDelegate();
		internal delegate void JoinedRoomDelegate();

		#region Fields

		private bool hasConnectedToMasterBefore;

		[SerializeField]
		private bool shldAutomaticallySyncScene;

		[SerializeField]
		private byte maxPlayersPerRoom;

		[SerializeField]
		private string gameVer;

		[SerializeField]
		private string nicknamePrefix;

		[SerializeField]
		private int nicknamePostfixLen;

		[SerializeField]
		private float waitingTime;

		[SerializeField]
		private float connectedTime;

		[SerializeField]
		private float joinedLobbyTime;

		[SerializeField]
		private float joinedRoomTime;

		[SerializeField]
		private float connectingMaxTime;

		[SerializeField]
		private float joiningLobbyMaxTime;

		[SerializeField]
		private float joiningRoomMaxTime;

		private float processingTime;

		private Coroutine joinedLobbyAndJoinRoomCoroutine;

		internal JoinedRoomDelegate joinedRoomDelegate;

		internal static NetworkManager globalObj;

		#endregion

		#region Properties

		internal bool IsInOfflineMode {
			get => PhotonNetwork.OfflineMode;
		}

		internal bool IsConnectedToNetwork {
			get;
			private set;
		}

		internal bool IsConnectedToNetworkAndReady {
			get => IsConnectedToNetwork && PhotonNetwork.IsConnectedAndReady;
		}

		internal bool IsInRoom {
			get => PhotonNetwork.CurrentRoom != null;
		}

		internal bool IsMasterClient {
			get => PhotonNetwork.IsMasterClient;
		}

		internal byte RoomCapacity {
			get => IsInRoom ? PhotonNetwork.CurrentRoom.MaxPlayers : (byte)0;
		}

		internal ConnectionStatus MyConnectionStatus {
			get;
			private set;
		}

		internal Player LocalPlayer {
			get => PhotonNetwork.LocalPlayer;
		}

		internal Player[] Players {
			get => PhotonNetwork.PlayerList;
		}

		internal Player[] OtherPlayers {
			get => PhotonNetwork.PlayerListOthers;
		}

		#endregion

		#region Ctors and Dtor

		internal NetworkManager(): base() {
			hasConnectedToMasterBefore = false;

			shldAutomaticallySyncScene = true;

			maxPlayersPerRoom = 0;

			gameVer = string.Empty;

			nicknamePrefix = string.Empty;
			nicknamePostfixLen = 0;

			waitingTime = 0.0f;
			connectedTime = 0.0f;
			joinedLobbyTime = 0.0f;
			joinedRoomTime = 0.0f;

			connectingMaxTime = 0.0f;
			joiningLobbyMaxTime = 0.0f;
			joiningRoomMaxTime = 0.0f;

			processingTime = 0.0f;

			joinedRoomDelegate = null;

			IsConnectedToNetwork = false;

			MyConnectionStatus = ConnectionStatus.Waiting;
		}

		static NetworkManager() {
			globalObj = null;
		}

		#endregion

		#region Unity User Callback Event Funcs

		private void Awake() {
			globalObj = this;

			PhotonNetwork.AutomaticallySyncScene = shldAutomaticallySyncScene;
			PhotonNetwork.GameVersion = gameVer;
			PhotonNetwork.LocalPlayer.NickName = nicknamePrefix;

			for(int i = 0; i < nicknamePostfixLen; ++i) {
				PhotonNetwork.LocalPlayer.NickName += (char)Random.Range(33, 127);
			}

			PhotonNetwork.NetworkingClient.LoadBalancingPeer.SerializationProtocolType
				= SerializationProtocol.GpBinaryV16;
		}

		private void Update() {
			IsConnectedToNetwork
				= (Application.internetReachability != NetworkReachability.NotReachable)
				&& hasConnectedToMasterBefore;
		}

		#endregion

		#region Photon Callback Funcs

		public override void OnConnectedToMaster() {
			Console.Log("ConnectedToMaster!", gameObject);

			hasConnectedToMasterBefore = true;

			if(PhotonNetwork.OfflineMode) {
				CreateRoomForOfflineMode();
			} else if(!PhotonNetwork.InLobby) {
				StopCoroutine(nameof(TrackProcessingTime));
				_ = StartCoroutine(nameof(ConnectedAndJoinLobbyCoroutine));
			}
		}

		private System.Collections.IEnumerator ConnectedAndJoinLobbyCoroutine() {
			yield return new WaitForSeconds(Mathf.Max(0.0f, connectingMaxTime - processingTime));
			processingTime = 0.0f;

			MyConnectionStatus = ConnectionStatus.Connected;

			yield return new WaitForSeconds(connectedTime);

			if(!IsConnectedToNetworkAndReady) {
				yield break;
			}

			_ = PhotonNetwork.JoinLobby();

			MyConnectionStatus = ConnectionStatus.JoiningLobby;

			_ = StartCoroutine(nameof(TrackProcessingTime));
		}

		public override void OnJoinedLobby() {
			Console.Log("Lobby joined!", gameObject);
		}

		public override void OnLeftLobby() {
			Console.Log("Lobby left!", gameObject);
		}

		public override void OnRoomListUpdate(List<RoomInfo> roomList) {
			if(PhotonNetwork.InLobby) {
				_ = PhotonNetwork.LeaveLobby();

				foreach(RoomInfo info in roomList) {
					if(info.PlayerCount == info.MaxPlayers) {
						continue;
					}

					StopCoroutine(nameof(TrackProcessingTime));
					joinedLobbyAndJoinRoomCoroutine = StartCoroutine(JoinedLobbyAndJoinRoomCoroutine(info.Name));

					return;
				}

				StopCoroutine(nameof(TrackProcessingTime));
				_ = StartCoroutine(nameof(JoinedLobbyAndCreateThenJoinRoomCoroutine));
			}
		}

		public override void OnCreatedRoom() {
			Console.Log("Room created!", gameObject);
		}

		public override void OnJoinedRoom() {
			Console.Log("Room joined!", gameObject);

			StopCoroutine(nameof(TrackProcessingTime));
			_ = StartCoroutine(nameof(JoinedRoomCoroutine));
		}

		public override void OnPlayerEnteredRoom(Player player) {
			Console.Log("A player entered the room: " + player.NickName, gameObject);

			if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers) {
				PhotonNetwork.CurrentRoom.IsOpen = false;
				PhotonNetwork.CurrentRoom.IsVisible = false;
			}
		}

		public override void OnPlayerLeftRoom(Player player) {
			Console.Log("A player left the room: " + player.NickName, gameObject);
		}

		public override void OnCreateRoomFailed(short returnCode, string msg) {
			Console.LogError("OnCreateRoomFailed " + '(' + returnCode + "): " + msg, gameObject);
		}

		public override void OnJoinRoomFailed(short returnCode, string msg) {
			Console.LogError("OnJoinRoomFailed " + '(' + returnCode + "): " + msg, gameObject);
		}

		public override void OnDisconnected(DisconnectCause cause) {
			Console.Log("Disconnected " + '(' + cause.ToString() + ")!", gameObject);
		}

		#endregion

		#region Other Photon Funcs

		private System.Collections.IEnumerator JoinedLobbyAndJoinRoomCoroutine(string roomName, string[] expectedUsers = null) {
			yield return new WaitForSeconds(Mathf.Max(0.0f, joiningLobbyMaxTime - processingTime));
			processingTime = 0.0f;

			MyConnectionStatus = ConnectionStatus.JoinedLobby;

			yield return new WaitForSeconds(joinedLobbyTime);

			if(!IsConnectedToNetworkAndReady) {
				yield break;
			}

			_ = PhotonNetwork.JoinRoom(roomName, expectedUsers);

			MyConnectionStatus = ConnectionStatus.JoiningRoom;

			_ = StartCoroutine(nameof(TrackProcessingTime));
		}

		private System.Collections.IEnumerator JoinedLobbyAndCreateThenJoinRoomCoroutine() {
			yield return new WaitForSeconds(Mathf.Max(0.0f, joiningLobbyMaxTime - processingTime));
			processingTime = 0.0f;

			MyConnectionStatus = ConnectionStatus.JoiningLobby;

			yield return new WaitForSeconds(joinedLobbyTime);

			if(!IsConnectedToNetworkAndReady) {
				yield break;
			}

			_ = PhotonNetwork.CreateRoom(
				null,
				new RoomOptions {
					MaxPlayers = maxPlayersPerRoom,
					PlayerTtl = 0,
					EmptyRoomTtl = 0,
					PublishUserId = true
				},
				null,
				null
			);

			MyConnectionStatus = ConnectionStatus.JoiningRoom;

			_ = StartCoroutine(nameof(TrackProcessingTime));
		}

		private void CreateRoomForOfflineMode() {
			_ = PhotonNetwork.CreateRoom(
				null,
				new RoomOptions {
					MaxPlayers = 1,
				},
				null,
				null
			);
		}

		private System.Collections.IEnumerator JoinedRoomCoroutine() {
			yield return new WaitForSeconds(Mathf.Max(0.0f, joiningRoomMaxTime - processingTime));
			processingTime = 0.0f;

			MyConnectionStatus = ConnectionStatus.JoinedRoom;

			yield return new WaitForSeconds(joinedRoomTime);

			if(!IsConnectedToNetworkAndReady) {
				yield break;
			}

			joinedRoomDelegate?.Invoke();
		}

		internal void ModifyCustomPropertiesOfPlayer(Player player, object key, object val) {
			Hashtable playerCustomProperties = new Hashtable();

			playerCustomProperties.Add(key, val);

			_ = player.SetCustomProperties(playerCustomProperties);
		}

		internal object RetrieveCustomPropertyOfPlayer(Player player, object key) {
			if(player.CustomProperties.TryGetValue(key, out object val)) { //Inline var declaration
				return val;
			}
			return null;
		}

		internal void ClearCustomPropertiesOfPlayer(Player player) {
			List<object> keys = new List<object>(player.CustomProperties.Keys.Count);

			foreach(var myKey in player.CustomProperties.Keys) {
				keys.Add(myKey);
			}

			foreach(object key in keys) {
				ModifyCustomPropertiesOfPlayer(player, key, null);
			}
		}

		internal T Instantiate<T>(bool usePhoton, T prefab, byte grp = 0, object[] data = null) where T: Object {
			if(usePhoton) {
				if(PhotonNetwork.PrefabPool is DefaultPool pool && !pool.ResourceCache.ContainsKey(prefab.name)) {
					pool.ResourceCache.Add(prefab.name, prefab as GameObject); //cannot use (GameObject)
				}

				return PhotonNetwork.Instantiate(prefab.name, Vector3.zero, Quaternion.identity, grp, data) as T; //cannot use (T)
			} else {
				return Object.Instantiate(prefab);
			}
		}

		internal void Destroy(bool usePhoton, Object obj, float t = 0.0f) {
			if(usePhoton) {
				PhotonNetwork.Destroy(((GameObject)obj).GetComponent<PhotonView>()); //as GameObject also can
			} else {
				Object.Destroy(obj, t);
			}
		}

		internal void ConnectForSinglePlayer(ConnectDelegate connectDelegate) {
			_ = StartCoroutine(WaitForDisconnection(connectDelegate));
		}

		private System.Collections.IEnumerator WaitForDisconnection(ConnectDelegate connectDelegate) {
			Disconnect(); //Just in case

			while(PhotonNetwork.IsConnected) {
				yield return null;
			}

			PhotonNetwork.OfflineMode = true;

			connectDelegate?.Invoke();
		}

		internal void ConnectForMultiplayer(ConnectDelegate connectDelegate) {
			_ = StartCoroutine(ConnectForMultiplayerCoroutine(connectDelegate));
		}

		private System.Collections.IEnumerator ConnectForMultiplayerCoroutine(ConnectDelegate connectDelegate) {
			MyConnectionStatus = ConnectionStatus.Waiting;

			yield return new WaitForSeconds(waitingTime);

			Disconnect(); //Just in case

			while(PhotonNetwork.IsConnected) {
				yield return null;
			}

			PhotonNetwork.OfflineMode = false;

			_ = PhotonNetwork.ConnectUsingSettings();

			MyConnectionStatus = ConnectionStatus.Connecting;

			_ = StartCoroutine(nameof(TrackProcessingTime));

			connectDelegate?.Invoke();
		}

		private System.Collections.IEnumerator TrackProcessingTime() {
			while(true) {
				processingTime += Time.deltaTime;
				yield return null;
			}
		}

		internal void Disconnect() {
			if(PhotonNetwork.IsConnected) {
				PhotonNetwork.NetworkingClient.AppId = null;

				ClearCustomPropertiesOfPlayer(LocalPlayer);

				//* Just in case
				StopCoroutine(nameof(ConnectedAndJoinLobbyCoroutine));

				if(joinedLobbyAndJoinRoomCoroutine != null) {
					StopCoroutine(joinedLobbyAndJoinRoomCoroutine);
				}

				StopCoroutine(nameof(JoinedLobbyAndCreateThenJoinRoomCoroutine));
				StopCoroutine(nameof(JoinedRoomCoroutine));
				//*/

				PhotonNetwork.Disconnect();
			}
		}

		internal bool RaiseEvent(byte eventCode, object eventContent = null, RaiseEventOptions raiseEventOptions = null, SendOptions? sendOptions = null) {
			if(raiseEventOptions == null) {
				raiseEventOptions = new RaiseEventOptions {
					Receivers = ReceiverGroup.All
				};
			}

			if(sendOptions == null) {
				sendOptions = SendOptions.SendReliable;
			}

			return PhotonNetwork.RaiseEvent(
				eventCode,
				eventContent,
				raiseEventOptions,
				(SendOptions)sendOptions
			);
		}

		#endregion

		#region Others
		#endregion
	}
}