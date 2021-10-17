using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Genesis.Wisdom;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.Creation {
    internal sealed class GameViewLayer: Singleton<GameViewLayer> {
        [SerializeField]
        private TMP_Text countdownText;

        [SerializeField]
        private TMP_Text gameTimeText;

        internal void ActivateDeactivateCountdownTextGameObj(bool isActivation) {
            countdownText.gameObject.SetActive(isActivation);
        }

        internal void ModifyStrOfCountdownText(string str) {
            countdownText.text = str;
        }

        internal void ModifyStrOfCountdownText(float val) {
            countdownText.text = Mathf.Ceil(val).ToString();
        }

        internal void ModifyStrOfGameTimeText(float val) {
            int valInt = (int)Mathf.Ceil(val);

            int min = valInt / 60;
            int sec = valInt % 60;

            string minStr = min.ToString();
            if(min < 10) {
                minStr = '0' + minStr;
            }

            string secStr = sec.ToString();
            if(sec < 10) {
                secStr = '0' + secStr;
            }

            gameTimeText.text = $"{minStr}:{secStr}";

            #if UNITY_EDITOR

            EditorUtility.SetDirty(gameTimeText);

            #endif
        }

        internal void ColorizeGameButton(GameObject gameButtonGameObj) {
            gameButtonGameObj.GetComponent<Image>().color = Color.HSVToRGB(
                Random.Range(0.0f, 1.0f),
                Random.Range(0.7f, 0.9f),
                Random.Range(0.8f, 0.95f),
                true
            );
        }
    }
}