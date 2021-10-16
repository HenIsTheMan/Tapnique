using System.Threading.Tasks;
using UnityEngine;

namespace Genesis.Creation {
    internal sealed class DelaySceneLoading: MonoBehaviour {
        [SerializeField]
        private int millisecondsTimeout;

        private void Awake() {
            Task myTask = new Task(async () => {
                await Task.Delay(millisecondsTimeout);
            });

            myTask.RunSynchronously();
        }
    }
}