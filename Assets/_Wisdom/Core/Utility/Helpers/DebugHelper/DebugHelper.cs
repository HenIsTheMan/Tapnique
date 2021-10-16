using System.Reflection;

namespace Genesis.Wisdom {
    internal static class DebugHelper {
		internal delegate void ClearConsoleDelegate();

		internal static ClearConsoleDelegate myClearConsoleDelegate = null;

		internal static void ClearConsole() {
			_ = clearMethodInfo?.Invoke(null, null);
			myClearConsoleDelegate?.Invoke();
		}

		private static readonly MethodInfo clearMethodInfo = System.Type.GetType("UnityEditor.LogEntries, UnityEditor", false).GetMethod("Clear");
    }
}