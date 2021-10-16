using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed class TaskHelperTest: MonoBehaviour {
		[SerializeField]
		private int taskDelayInMilliseconds;

		[SerializeField]
		private int val;

		private void Awake() {
			_ = StartCoroutine(nameof(TaskHelperTestCoroutine));
		}

		private IEnumerator TaskHelperTestCoroutine() {
			Task<int> myTask = MyTask();

			yield return new WaitUntil(() => {
				return TaskHelper.HandleTask(myTask);
			});

			Debug.Log(myTask.Result, gameObject);
		}

		private Task<int> MyTask() {
			return Task.Run(async () => {
				await Task.Delay(taskDelayInMilliseconds);
				return val;
			});
		}
	}
}