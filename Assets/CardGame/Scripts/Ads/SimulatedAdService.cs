using System;
using CardGame.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Ads
{
    public class SimulatedAdService : MonoBehaviour, IAdService
    {
        private const string SkipButtonName = "ui_button_skip";

        [Header("Buttons")]
        [SerializeField] private Button _skipButton;

        [Header("Countdown")]
        [SerializeField] private TextMeshProUGUI _countdownText;
        [SerializeField] private float _durationSeconds = 3f;

        private Action<bool> _onFinished;
        private Tween _countdown;

        public void ShowRewardedAd(Action<bool> onFinished) {
            _onFinished = onFinished;
            gameObject.SetActive(true);
            _countdown = DOVirtual.Float(_durationSeconds, 0f, _durationSeconds, ShowSecondsLeft)
                .SetEase(Ease.Linear)
                .OnComplete(() => Finish(true));
        }

        private void Awake() {
            _skipButton.onClick.AddListener(Skip);
        }

        private void OnDestroy() {
            _countdown?.Kill();
        }

        private void Skip() {
            _countdown.Kill();
            Finish(false);
        }

        private void Finish(bool isCompleted) {
            gameObject.SetActive(false);
            var onFinished = _onFinished;
            _onFinished = null;
            onFinished(isCompleted);
        }

        private void ShowSecondsLeft(float secondsLeft) {
            _countdownText.text = Mathf.CeilToInt(secondsLeft).ToString();
        }

    #if UNITY_EDITOR
        private void OnValidate() {
            _skipButton = ChildButtons.FindIfMissing(this, _skipButton, SkipButtonName);
        }
    #endif
    }
}
