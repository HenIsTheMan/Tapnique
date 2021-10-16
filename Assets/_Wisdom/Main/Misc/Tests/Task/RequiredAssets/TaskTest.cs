using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class TaskTest: MonoBehaviour {
		private void Awake() {
			_ = StartCoroutine(nameof(MyCoroutine));
			MyFunc();
		}

		private IEnumerator MyCoroutine() {
			//Task antecedent = Task.Run(() => {
			//	Debug.Log("Antecedent from " + nameof(MyCoroutine), null);
			//});
			//Task antecedent = Task.Factory.StartNew(() => {
			//	Debug.Log("Antecedent from " + nameof(MyCoroutine), null);
			//});
			//Task antecedent = new Task(() => {
			//	Debug.Log("Antecedent from " + nameof(MyCoroutine), null);
			//});
			Task<int> antecedent = Task.FromResult(TestFunc(4, nameof(MyFunc)));

			Task continuationTask = antecedent.ContinueWith((_) => {
				Debug.Log("Continuation from " + nameof(MyCoroutine), null);
			});

			//antecedent.Start();

			Debug.Log("Antecedent from " + nameof(MyCoroutine) + " (before): " + antecedent.Status.ToString(), gameObject);
			Debug.Log("Continuation from " + nameof(MyCoroutine) + " (before): " + continuationTask.Status.ToString(), gameObject);

			/* Inconsistent
			yield return new WaitUntil(() => {
				return antecedent.IsCompleted;
			});
			//*/

			//* Consistent
			yield return new WaitUntil(() => {
				return continuationTask.IsCompleted;
			});
			//*/

			Debug.Log("Antecedent from " + nameof(MyCoroutine) + " (after): " + antecedent.Status.ToString(), gameObject);
			Debug.Log("Continuation from " + nameof(MyCoroutine) + " (after): " + continuationTask.Status.ToString(), gameObject);

			Debug.Log(nameof(MyCoroutine), gameObject);
		}

		private async void MyFunc() {
			//Task antecedent = Task.Run(() => {
			//	Debug.Log("Antecedent from " + nameof(MyFunc), null);
			//});
			//Task antecedent = Task.Factory.StartNew(() => {
			//	Debug.Log("Antecedent from " + nameof(MyFunc), null);
			//});
			//Task antecedent = new Task(() => {
			//	Debug.Log("Antecedent from " + nameof(MyFunc), null);
			//});
			Task<int> antecedent = Task.FromResult(TestFunc(4, nameof(MyFunc)));

			Task continuationTask = antecedent.ContinueWith((_) => {
				Debug.Log("Continuation from " + nameof(MyFunc), null);
			});

			//antecedent.Start();

			Debug.Log("Antecedent from " + nameof(MyFunc) + " (before): " + antecedent.Status.ToString(), gameObject);
			Debug.Log("Continuation from " + nameof(MyFunc) + " (before): " + continuationTask.Status.ToString(), gameObject);

			//await antecedent; //Inconsistent
			await continuationTask; //Consistent

			Debug.Log("Antecedent from " + nameof(MyFunc) + " (after): " + antecedent.Status.ToString(), gameObject);
			Debug.Log("Continuation from " + nameof(MyFunc) + " (after): " + continuationTask.Status.ToString(), gameObject);

			Debug.Log(nameof(MyFunc), gameObject);
		}

		private int TestFunc(int val, string name) {
			Debug.Log("Antecedent from " + name, null);
			return val;
		}
	}
}