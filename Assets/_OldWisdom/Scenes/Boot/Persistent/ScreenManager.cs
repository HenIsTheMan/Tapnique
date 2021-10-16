using System;
using System.Runtime.InteropServices;
using UnityEngine;
using static IWP.General.ScreenModes;

namespace IWP.General {
	[DisallowMultipleComponent]
	internal sealed class ScreenManager: MonoBehaviour {
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

		private HandleRef windowHandleRef;

		#region Fields

		private const int SW_MAXIMIZE = 3;

		[Min(1), SerializeField]
		private int width;

		[Min(1), SerializeField]
		private int height;

		[SerializeField]
		private ScreenMode mode;

		[SerializeField]
		private int preferredRefreshRate;

		private string oldWindowTitle;

		[SerializeField]
		private string newWindowTitle;

		internal static ScreenManager globalObj;

		#endregion

		#region Properties

		internal int Width {
			set {
				width = value;
				SetScreenRes(width, height, mode, preferredRefreshRate);
			}
		}

		internal int Height {
			set {
				height = value;
				SetScreenRes(width, height, mode, preferredRefreshRate);
			}
		}

		internal ScreenMode Mode {
			get => mode;
			set {
				mode = value;
				SetScreenRes(width, height, mode, preferredRefreshRate);
			}
		}

		internal int PsreferredRefreshRate {
			get => preferredRefreshRate;
			set {
				preferredRefreshRate = value;
				SetScreenRes(width, height, mode, preferredRefreshRate);
			}
		}

		#endregion

		#region Ctors and Dtor

		internal ScreenManager(): base() {
			width = 1;
			height = 1;
			mode = ScreenMode.Windowed;
			preferredRefreshRate = 0;

			oldWindowTitle = string.Empty;
			newWindowTitle = string.Empty;
		}

		static ScreenManager() {
			globalObj = null;
		}

		#endregion

		#region Unity User Callback Event Funcs

		private void OnValidate() {
			UnityEngine.Assertions.Assert.AreNotEqual(
				mode, ScreenMode.Amt,
				"mode, ScreenMode.Amt"
			);
		}

		private void Awake() {
			globalObj = this;

			oldWindowTitle = Application.productName;

			if(Screen.fullScreenMode != (FullScreenMode)mode
				|| Screen.width != width
				|| Screen.height != height
			) {
				SetScreenRes(width, height, mode, preferredRefreshRate);
			}

			if(PlatDetect.IsWinBuild
				&& (mode == ScreenMode.Windowed || mode == ScreenMode.MaximizedWindow)
				&& newWindowTitle != oldWindowTitle
			) {
				SetWindowTitle(FindWindow(null, oldWindowTitle), newWindowTitle);
			}
		}

		#endregion

		internal void SetScreenRes(int width, int height, ScreenMode mode, int preferredRefreshRate = 0) {
			if(mode == ScreenMode.MaximizedWindow) {
				_ = StartCoroutine(SetScreenResAndMaximizeWindow(width, height, preferredRefreshRate));
			} else {
				Screen.SetResolution(width, height, (FullScreenMode)mode, preferredRefreshRate);
			}

			this.width = width;
			this.height = height;
			this.mode = mode;
			this.preferredRefreshRate = preferredRefreshRate;
		}

		private System.Collections.IEnumerator SetScreenResAndMaximizeWindow(int width, int height, int preferredRefreshRate = 0) {
			Screen.SetResolution(width, height, FullScreenMode.Windowed, preferredRefreshRate);

			yield return new WaitForSeconds(0.2f);

			EnumWindows(EnumWindowsCallBack, IntPtr.Zero);
			ShowWindow(windowHandleRef.Handle, SW_MAXIMIZE);
		}

		private bool EnumWindowsCallBack(IntPtr hWnd, IntPtr lParam) {
			GetWindowThreadProcessId(new HandleRef(this, hWnd), out int procid); //Inline var declaration

			int currPID = System.Diagnostics.Process.GetCurrentProcess().Id;

			new HandleRef(this, System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);

			if(procid == currPID) {
				windowHandleRef = new HandleRef(this, hWnd);
				return false;
			}

			return true;
		}

		internal void SetWindowTitle(IntPtr windowHandle, string windowTitle) {
			_ = SetWindowText(windowHandle, windowTitle);
		}
	}
}