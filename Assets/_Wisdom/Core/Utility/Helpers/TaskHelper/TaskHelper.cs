using System.Threading.Tasks;
using UnityEngine;

namespace Genesis.Wisdom {
	internal static class TaskHelper {
		internal static bool HandleTask(Task task) {
			return task.Status == TaskStatus.RanToCompletion;
		}

		internal static bool HandleTask(Task task, bool shldPrintIfCanceled, bool shldPrintIfFaulted) {
			if(task.Status == TaskStatus.Canceled) { //task.IsCanceled
				if(shldPrintIfCanceled) {
					Debug.Log(nameof(task.IsCanceled), null);
				}
				return false;
			}

			if(task.Status == TaskStatus.Faulted) { //task.IsFaulted
				if(shldPrintIfFaulted) {
					Debug.Log(task.Exception.GetBaseException().Message, null);
				}
				return false;
			}

			return task.Status == TaskStatus.RanToCompletion;
		}
	}
}