#if UNITY_STANDALONE_WIN

using System;
using System.Runtime.InteropServices;
using UnityEngine;

#endif

namespace Genesis.Wisdom {
	internal sealed class ScreenManager: Singleton<ScreenManager> {
		#if UNITY_STANDALONE_WIN

		internal int ScreenResWidth {
			get => screenResWidth;
			set {
				if(value >= 1) {
					screenResWidth = value;
					SetScreenRes();
				}
			}
		}

		internal int ScreenResHeight {
			get => screenResHeight;
			set {
				if(value >= 1) {
					screenResHeight = value;
					SetScreenRes();
				}
			}
		}

		internal FullScreenMode FullscreenMode {
			get => fullscreenMode;
			set {
				fullscreenMode = value;

				if(fullscreenMode != FullScreenMode.ExclusiveFullScreen) {
					preferredRefreshRate = 0;
				}

				SetScreenRes();
			}
		}

		internal int PreferredRefreshRate {
			get => preferredRefreshRate;
			set {
				if(value < 0) {
					return;
				}

				preferredRefreshRate
					= fullscreenMode == FullScreenMode.ExclusiveFullScreen
					? value
					: 0;

				SetScreenRes();
			}
		}

		internal string NextWindowTitle {
			get => nextWindowTitle;
			set {
				if(value == null) {
					return;
				}

				nextWindowTitle = value;
				SetWindowTitle();
			}
		}

		private const int SW_HIDE = 0;

		private const int SW_MAXIMIZE = 3;

		private HandleRef windowHandleRef;

		[SerializeField]
		private bool shldChangeScreenResInAwake;

		[SerializeField]
		private bool shldChangeWindowTitleInAwake;

		[Min(1, order = 0)]
		[SerializeField]
		[ShowHideInInspector(true, nameof(shldChangeScreenResInAwake), true, order = 1)]
		private int screenResWidth;

		[Min(1, order = 0)]
		[SerializeField]
		[ShowHideInInspector(true, nameof(shldChangeScreenResInAwake), true, order = 1)]
		private int screenResHeight;

		[SerializeField]
		[ShowHideInInspector(true, nameof(shldChangeScreenResInAwake), true)]
		private FullScreenMode fullscreenMode;

		[Min(0, order = 0)]
		[SerializeField]
		[ShowHideInInspector(true, nameof(shldChangeScreenResInAwake), true, order = 1)]
		private int preferredRefreshRate;

		[SerializeField]
		[ShowHideInInspector(true, nameof(shldChangeWindowTitleInAwake), true)]
		private string nextWindowTitle;

		private string currWindowTitle;

		#endif

		private void Reset() {
			screenResWidth = Screen.currentResolution.width;
			screenResHeight = Screen.currentResolution.height;

			fullscreenMode = FullScreenMode.ExclusiveFullScreen;

			preferredRefreshRate = 0;

			nextWindowTitle = Application.productName + ' ' + Application.version;
		}

		private protected override void OnValidate() {
			base.OnValidate();

			if(fullscreenMode != FullScreenMode.ExclusiveFullScreen) {
				preferredRefreshRate = 0;
			}
		}

		private void Awake() {
			#if UNITY_STANDALONE_WIN

			currWindowTitle = Application.productName;

			if(shldChangeScreenResInAwake) {
				SetScreenRes();
			}

			if(shldChangeWindowTitleInAwake) {
				SetWindowTitle();
			}

			#endif
		}

		private void SetScreenRes() {
			if(fullscreenMode == FullScreenMode.MaximizedWindow) {
				_ = StartCoroutine(nameof(SetScreenResAndMaximizeWindow));
			} else {
				Screen.SetResolution(screenResWidth, screenResHeight, fullscreenMode, preferredRefreshRate);
				_ = SetWindowPos(FindWindow(null, currWindowTitle), 0, -8, 0, 0, 0, 5);
			}
		}

		private void SetWindowTitle() {
			_ = SetWindowText(FindWindow(null, currWindowTitle), nextWindowTitle);
			currWindowTitle = nextWindowTitle;
			nextWindowTitle = null;
		}

		private System.Collections.IEnumerator SetScreenResAndMaximizeWindow() {
			Screen.SetResolution(screenResWidth, screenResHeight, FullScreenMode.Windowed, preferredRefreshRate);
			_ = SetWindowPos(FindWindow(null, currWindowTitle), 0, -8, 0, 0, 0, 5);

			yield return new WaitForSeconds(0.1f);

			EnumWindows(EnumWindowsCallBack, IntPtr.Zero);

			ShowWindow(windowHandleRef.Handle, SW_HIDE);

			ShowWindow(windowHandleRef.Handle, SW_MAXIMIZE);
		}

		private bool EnumWindowsCallBack(IntPtr hWnd, IntPtr lParam) {
			_ = GetWindowThreadProcessId(new HandleRef(this, hWnd), out int procId);

			int currPID = System.Diagnostics.Process.GetCurrentProcess().Id;

			_ = new HandleRef(this, System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);

			if(procId == currPID) {
				windowHandleRef = new HandleRef(this, hWnd);
				return false;
			}

			return true;
		}

		#if UNITY_STANDALONE_WIN

		[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
		private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

		[DllImport("user32.dll", EntryPoint = "FindWindow")]
		private static extern IntPtr FindWindow(string className, string windowName);

		[DllImport("user32.dll", EntryPoint = "SetWindowText")]
		private static extern bool SetWindowText(IntPtr hwnd, string lpString);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

		private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool EnumWindows(EnumWindowsProc callback, IntPtr extraData);

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

		[DllImport("user32.dll")]
		private static extern IntPtr GetActiveWindow();

		#endif
	}
}