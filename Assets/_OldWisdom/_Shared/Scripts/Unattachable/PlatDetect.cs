namespace IWP.General {
    internal static class PlatDetect {
		internal static bool IsWinBuild {
			get;
			private set;
		}

		internal static bool IsAndroidBuild {
			get;
			private set;
		}

		internal static bool IsInUnityEditor {
			get;
			private set;
		}

		internal static string PlatInfo {
			get;
			private set;
		}

		static PlatDetect() {
			IsWinBuild = TestWinBuild();

			IsAndroidBuild = TestAndroidBuild();

			IsInUnityEditor = TestInUnityEditor();

			PlatInfo = UnityEngine.SystemInfo.operatingSystem;
		}

		private static bool TestWinBuild() {
			#if UNITY_WSA && !UNITY_EDITOR
				return true;
			#else
				return false;
			#endif
		}

		private static bool TestAndroidBuild() {
			#if UNITY_ANDROID && !UNITY_EDITOR
				return true;
			#else
				return false;
			#endif
		}

		private static bool TestInUnityEditor() {
			#if UNITY_EDITOR
				return true;
			#else
				return false;
			#endif
		}
	}
}