using System.Collections.Generic;
using UnityEngine;

namespace Genesis.Wisdom {
	internal static class WaitHelper {
		internal static WaitForEndOfFrame MyWaitForEndOfFrame {
			get;
		} = new WaitForEndOfFrame();

		internal static WaitForFixedUpdate MyWaitForFixedUpdate {
			get;
		} = new WaitForFixedUpdate();

		internal static void AddWaitForSeconds(float seconds) {
			string key = seconds.ToString(numericFormatStr);

			if(!waitForSecondsDict.ContainsKey(key)) {
				waitForSecondsDict.Add(key, new WaitForSeconds(seconds));
			}
		}

		internal static void AddWaitForSecondsRealtime(float seconds) {
			string key = seconds.ToString(numericFormatStr);

			if(!waitForSecondsRealtimeDict.ContainsKey(key)) {
				waitForSecondsRealtimeDict.Add(key, new WaitForSecondsRealtime(seconds));
			}
		}

		internal static WaitForSeconds GetWaitForSeconds(float seconds) {
			return waitForSecondsDict[seconds.ToString(numericFormatStr)];
		}

		internal static WaitForSecondsRealtime GetWaitForSecondsRealtime(float seconds) {
			return waitForSecondsRealtimeDict[seconds.ToString(numericFormatStr)];
		}

		private static readonly Dictionary<string, WaitForSeconds> waitForSecondsDict
			= new Dictionary<string, WaitForSeconds>();

		private static readonly Dictionary<string, WaitForSecondsRealtime> waitForSecondsRealtimeDict
			= new Dictionary<string, WaitForSecondsRealtime>();

		private const int precision = 4;

		private static readonly string numericFormatStr = 'F' + precision.ToString();
	}
}