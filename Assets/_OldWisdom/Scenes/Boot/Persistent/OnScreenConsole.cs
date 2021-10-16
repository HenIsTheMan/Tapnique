using UnityEngine;

namespace IWP.General {
	[DisallowMultipleComponent]
	internal sealed class OnScreenConsole: MonoBehaviour {
		#region Fields

		private Rect rect;
		private string myLog;

		[SerializeField]
		private bool isVisible;

		[SerializeField]
		private KeyCode keyCode;

		[SerializeField]
		private bool showMsg;

		[SerializeField]
		private bool showLogType;

		[SerializeField]
		private bool showStackTrace;

		[SerializeField]
		private float xOffset;

		[SerializeField]
		private float yOffset;

		[SerializeField]
		private float widthOffset;

		[SerializeField]
		private float heightOffset;

		[SerializeField]
		private Color bgColor;

		[SerializeField]
		private Color contentColor;

		[Min(0), SerializeField]
		private int fontSize;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal OnScreenConsole(): base() {
			rect = Rect.zero;
			myLog = string.Empty;
			isVisible = false;
			keyCode = KeyCode.Space;

			showMsg = true;
			showLogType = false;
			showStackTrace = false;

			xOffset = 0.0f;
			yOffset = 0.0f;
			widthOffset = 0.0f;
			heightOffset = 0.0f;
			bgColor = Color.white;
			contentColor = Color.white;
			fontSize = 0;
		}

		static OnScreenConsole() {
		}

		#endregion

		#region Unity User Callback Event Funcs

		private void Awake() {
			rect = new Rect(xOffset, yOffset, Screen.width + widthOffset, Screen.height + heightOffset);
		}

		private void OnEnable() {
			Application.logMessageReceivedThreaded += LogToOnScreenConsole;
			Console.clearConsoleDelegate += ClearOnScreenConsole;
		}

		private void Update() {
			if(Input.GetKeyDown(keyCode)) {
				isVisible = !isVisible;
			}
		}

		private void OnGUI() {
			if(isVisible) {
				GUI.skin.textArea.fontSize = fontSize;
				GUI.backgroundColor = bgColor;
				GUI.contentColor = contentColor;
				GUI.TextArea(rect, myLog);
			}
		}

		private void OnDisable() {
			Application.logMessageReceivedThreaded -= LogToOnScreenConsole;
			Console.clearConsoleDelegate -= ClearOnScreenConsole;
		}

		#endregion

		private void LogToOnScreenConsole(string msg, string stackTrace, LogType logType) {
			if(showMsg) {
				myLog += "Msg: " + msg + '\n';
			}

			if(showLogType) {
				myLog += "LogType: " + logType.ToString() + '\n';
			}

			if(showStackTrace) {
				myLog += "StackTrace: " + stackTrace + '\n';
			}
		}

		private void ClearOnScreenConsole() {
			myLog = string.Empty;
		}
	}
}