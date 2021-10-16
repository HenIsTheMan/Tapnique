using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class StrHelperTest: MonoBehaviour {
		[SerializeField]
		private string usernameStr;

		[SerializeField]
		private string emailStr;

		private void Awake() {
			Debug.Log(usernameStr.IsEmail(), gameObject);
			Debug.Log(StrHelper.IsEmail(emailStr), gameObject);
		}
	}
}