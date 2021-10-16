using UnityEngine;

namespace Genesis.Wisdom {
    internal static class VibrationMaster {
		#if UNITY_ANDROID && !UNITY_EDITOR

        private static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        private static AndroidJavaObject currActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        private static AndroidJavaObject vibrator = currActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

		#else

		private static AndroidJavaClass unityPlayer;
		private static AndroidJavaObject currActivity;
		private static AndroidJavaObject vibrator;

		#endif

		internal static void StartVibration() {
			#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)

			Handheld.Vibrate();

			#endif
		}

		internal static void StartVibration(long milliseconds) {
			#if UNITY_ANDROID && !UNITY_EDITOR

			vibrator.Call("vibrate", milliseconds);

			#endif
        }

		internal static void StartVibration(long[] pattern, int repeat) {
			#if UNITY_ANDROID && !UNITY_EDITOR

			vibrator.Call("vibrate", pattern, repeat);

			#endif
        }

		internal static void StopVibration() {
			#if UNITY_ANDROID && !UNITY_EDITOR

			vibrator.Call("cancel");

			#endif
		}
	}
}