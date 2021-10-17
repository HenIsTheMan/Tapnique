using TMPro;
using UnityEngine;

namespace Genesis.Creation {
    internal sealed class GameViewLayer: MonoBehaviour {
        [SerializeField]
        private TMP_Text countdownText;

        internal void ActivateDeactivateCountdownTextGameObj(bool isActivation) {
            countdownText.gameObject.SetActive(isActivation);
        }

        internal void ModifyStrOfCountdownText(string str) {
            countdownText.text = str;
        }

        internal void ModifyStrOfCountdownText(float val) {
            countdownText.text = Mathf.Ceil(val).ToString();
        }
    }
}