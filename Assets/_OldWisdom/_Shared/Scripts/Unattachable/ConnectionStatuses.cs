namespace IWP.General {
	internal static class ConnectionStatuses {
		internal enum ConnectionStatus: byte {
			Waiting,
			Connecting,
			Connected,
			JoiningLobby,
			JoinedLobby,
			JoiningRoom,
			JoinedRoom,
			Amt
		}
	}
}