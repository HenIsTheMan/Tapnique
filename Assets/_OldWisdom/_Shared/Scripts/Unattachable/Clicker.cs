using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace IWP.General {
    internal static class Clicker {
		[DllImport("User32.dll", SetLastError = true)]
		public static extern int SendInput(int nInputs, ref INPUT pInputs, int cbSize);

		public struct MOUSEINPUT {
			public int dx;
			public int dy;
			public int mouseData;
			public int dwFlags;
			public int time;
			public IntPtr dwExtraInfo;
		}

		public struct INPUT {
			public uint type;
			public MOUSEINPUT mi;
		};

		#region Fields

		private const int leftDownMouseEventF = 2;
		private const int leftUpMouseEventF = 4;
		private const int inputMouse = 0;
		private static INPUT i;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		static Clicker() {
			i = new INPUT();
		}

		#endregion

		internal static void Click(float x, float y) {
			i.type = inputMouse;
			i.mi.dx = (int)(x * (0xFFFF / Screen.width));
			i.mi.dy = (int)(y * (0xFFFF / Screen.height));
			i.mi.dwExtraInfo = IntPtr.Zero;
			i.mi.mouseData = 0;
			i.mi.time = 0;

			i.mi.dwFlags = leftDownMouseEventF;
			SendInput(1, ref i, Marshal.SizeOf(i));

			i.mi.dwFlags = leftUpMouseEventF;
			SendInput(1, ref i, Marshal.SizeOf(i));
		}
    }
}