using System;
using CardGame.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Ads.View
{
    public class SimulatedAdService : MonoBehaviour, IAdService
    {
        private const string CloseButtonName = "ui_button_close";

        [Header("Buttons")]
        [SerializeField] private Button _closeButton;

        [Header("Countdown")]
        [SerializeField] private TextMeshProUGUI _countdownText;
        [SerializeField] private float _durationSeconds = 5f;

        private Action<bool> _onFinished;
        private Tween _countdown;

        public void ShowRewardedAd(Action<bool> onFinished) {
            _onFinished = onFinished;
            _closeButton.gameObject.SetActive(false);
            gameObject.SetActive(true);
            _countdown = DOVirtual.Float(_durationSeconds, 0f, _durationSeconds, ShowSecondsLeft)
                .SetEase(Ease.Linear)
                .OnComplete(ShowCloseButton);
        }

        private void Awake() {
            _closeButton.onClick.AddListener(Close);
        }

        private void OnDestroy() {
            _countdown?.Kill();
        }

        private void ShowCloseButton() {
            _closeButton.gameObject.SetActive(true);
        }

        private void Close() {
            gameObject.SetActive(false);
            var onFinished = _onFinished;
            _onFinished = null;
            onFinished(true);
        }

        private void ShowSecondsLeft(float secondsLeft) {
            _countdownText.text = Mathf.CeilToInt(secondsLeft).ToString();
        }

    #if UNITY_EDITOR
        private void OnValidate() {
            _closeButton = ChildButtons.FindIfMissing(this, _closeButton, CloseButtonName);
        }
    #endif
    }
}
