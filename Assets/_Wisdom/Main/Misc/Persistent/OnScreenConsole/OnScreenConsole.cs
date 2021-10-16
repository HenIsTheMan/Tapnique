using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed class OnScreenConsole: Singleton<OnScreenConsole> {
		[SerializeField]
		private bool isVisible;

		[SerializeField]
		private bool shldShowMsg;

		[SerializeField]
		private bool shldShowLogType;

		[SerializeField]
		private bool shldShowStackTrace;

		[SerializeField]
		private KeyCode keyCode;

		[SerializeField]
		private float x;

		[SerializeField]
		private float y;

		[SerializeField]
		private float width;

		[SerializeField]
		private float height;

		[ColorUsage(true, false)]
		[SerializeField]
		private Color contentColor;

		[Min(0)]
		[SerializeField]
		private int fontSize;

		private Rect rect;

		private string myLog;

		private delegate void LogToOnScreenConsoleDelegate(string msg, string stackTrace, LogType logType);

		private LogToOnScreenConsoleDelegate logToOnScreenConsoleDelegate;

		private void Reset() {
			isVisible = false;

			shldShowMsg = true;
			shldShowLogType = false;
			shldShowStackTrace = false;

			keyCode = KeyCode.None;

			x = 0.0f;
			y = 0.0f;
			width = Screen.currentResolution.width;
			height = Screen.currentResolution.height;

			contentColor = Color.green;
			fontSize = 40;
		}

		private void Awake() {
			rect = new Rect(x, y, width, height);

			logToOnScreenConsoleDelegate = null;

			if(shldShowMsg) {
				logToOnScreenConsoleDelegate += LogMsgToOnScreenConsole;
			}

			if(shldShowLogType) {
				logToOnScreenConsoleDelegate += LogLogTypeToOnScreenConsole;
			}

			if(shldShowStackTrace) {
				logToOnScreenConsoleDelegate += LogStackTraceToOnScreenConsole;
			}

			void LogMsgToOnScreenConsole(string msg, string stackTrace, LogType logType) {
				myLog += "Msg: " + msg + '\n';
			}

			void LogLogTypeToOnScreenConsole(string msg, string stackTrace, LogType logType) {
				myLog += "LogType: " + logType.ToString() + '\n';
			}

			void LogStackTraceToOnScreenConsole(string msg, string stackTrace, LogType logType) {
				myLog += "StackTrace: " + stackTrace + '\n';
			}
		}

		private void OnEnable() {
			Application.logMessageReceivedThreaded += LogToOnScreenConsole;
			DebugHelper.myClearConsoleDelegate += ClearOnScreenConsole;
		}

		private void Update() {
			if(Input.GetKeyDown(keyCode)) {
				isVisible = !isVisible;
			}
		}

		private void OnGUI() {
			if(isVisible) {
				GUI.contentColor = contentColor;
				GUI.skin.textArea.fontSize = fontSize;
				GUI.TextArea(rect, myLog);
			}
		}

		private void OnDisable() {
			Application.logMessageReceivedThreaded -= LogToOnScreenConsole;
			DebugHelper.myClearConsoleDelegate -= ClearOnScreenConsole;
		}

		private void LogToOnScreenConsole(string msg, string stackTrace, LogType logType) {
			logToOnScreenConsoleDelegate?.Invoke(msg, stackTrace, logType);
		}

		private void ClearOnScreenConsole() {
			myLog = string.Empty;
		}
	}
}