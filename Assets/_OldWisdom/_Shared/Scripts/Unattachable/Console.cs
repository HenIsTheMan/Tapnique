using System.Reflection;
using UnityEngine;

namespace IWP.General {
	internal static class Console {
		internal delegate void ClearConsoleDelegate();

		#region Fields

		private static readonly MethodInfo clearMethodInfo;

		internal static ClearConsoleDelegate clearConsoleDelegate;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		static Console() {
			if(PlatDetect.IsInUnityEditor) {
				clearMethodInfo = System.Type.GetType("UnityEditor.LogEntries, UnityEditor", false).GetMethod("Clear");
			}

			clearConsoleDelegate = null;
		}

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		internal static void Log(object msg, Object context = null) {
			Debug.Log(msg, context);
		}

		internal static void LogWarning(object msg, Object context = null) {
			Debug.LogWarning(msg, context);
		}

		internal static void LogError(object msg, Object context = null) {
			Debug.LogError(msg, context);
		}

		internal static void LogFormat(string format, params object[] args) {
			Debug.LogFormat(format, args);
		}

		internal static void LogFormat(Object context, string format, params object[] args) {
			Debug.LogFormat(context, format, args);
		}

		internal static void LogFormat(LogTypes.LogType logType, LogOptions.LogOption logOption, Object context, string format, params object[] args) {
			Debug.LogFormat((LogType)logType, (LogOption)logOption, context, format, args);
		}

		internal static void Clear() {
			_ = clearMethodInfo?.Invoke(null, null);
			clearConsoleDelegate?.Invoke();
		}
	}
}