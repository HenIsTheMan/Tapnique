using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Genesis.Wisdom;
using System.Threading.Tasks;

#if UNITY_EDITOR

using UnityEditor.SceneManagement;
using UnityEditor;

#endif

namespace Genesis.Creation {
    internal sealed class GameViewLayer: Singleton<GameViewLayer> {
        [SerializeField]
        private TMP_Text countdownText;

        [SerializeField]
        private string countdownEndedStr;

        [SerializeField]
        private TMP_Text gameTimeText;

        [SerializeField]
        private TMP_Text ptsText;

        [SerializeField]
        private TMP_Text roundTimeText;

        [SerializeField]
        private int roundTimePrecision;

        [UnmodifiableInInspector]
        [SerializeField]
        private string roundTimeNumericFormatStr;

        [SerializeField]
        private Canvas gameCamCanvas;

        [SerializeField]
        private Canvas gameEndCamCanvas;

        [SerializeField]
        private TMP_Text gameOverNewHighScoreText;

        [SerializeField]
        private TMP_Text scoreText;

        [SerializeField]
        private TMP_Text highScoreText;

        [SerializeField]
        private string gameOverStr;

        [SerializeField]
        private string newHighScoreStr;

        internal void ActivateDeactivateCountdownTextGameObj(bool isActivation) {
            countdownText.gameObject.SetActive(isActivation);
        }

        internal void ShowCountdownEndedStr() {
            countdownText.text = countdownEndedStr;
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

        internal void ModifyStrOfPtsText(int val) {
            ptsText.text = val + "\nPoints";

            #if UNITY_EDITOR

            EditorUtility.SetDirty(ptsText);

            #endif
        }

        internal void ModifyStrOfRoundTimeText(float val) {
            roundTimeText.text = val.ToString(roundTimeNumericFormatStr) + " s";

            #if UNITY_EDITOR

            EditorUtility.SetDirty(roundTimeText);

            #endif
        }

        internal void ColorizeGameButton(GameButtonLink gameButtonLink) {
            gameButtonLink.Img.color = Color.HSVToRGB(
                Random.Range(0.0f, 1.0f),
                Random.Range(0.7f, 0.9f),
                Random.Range(0.8f, 0.95f),
                true
            );
        }

        internal void ShowGameEndView(bool hadGottenHighScore, int score, int highScore) {
            gameCamCanvas.gameObject.SetActive(false);
            gameEndCamCanvas.gameObject.SetActive(true);

            if(hadGottenHighScore) {
                gameOverNewHighScoreText.text = newHighScoreStr;
            }

            scoreText.text = "Score: " + score.ToString();
            highScoreText.text = "HighScore: " + highScore.ToString();
        }

        #if UNITY_EDITOR

        private protected override void OnValidate() {
            base.OnValidate();

            if(EditorApplication.isPlayingOrWillChangePlaymode) {
                return;
            }

            if(gameOverNewHighScoreText != null) {
                gameOverNewHighScoreText.text = gameOverStr;
                EditorUtility.SetDirty(gameOverNewHighScoreText);
            }

            Awake();

            Task myTask = new Task(async () => {
                await Task.Delay(14); //Just in case

                if(this != null) {
                    EditorSceneManager.SaveScene(gameObject.scene);
                }
            });

            myTask.RunSynchronously();
        }

        #endif

        private void Awake() {
            roundTimeNumericFormatStr = 'F' + roundTimePrecision.ToString();
        }
    }
}