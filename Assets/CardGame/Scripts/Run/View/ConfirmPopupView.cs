using System;
using CardGame.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Run.View
{
    public class ConfirmPopupView : MonoBehaviour
    {
        private const string ConfirmButtonName = "ui_button_confirm";
        private const string GoBackButtonName = "ui_button_go_back";

        [Header("Buttons")]
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _goBackButton;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private TextMeshProUGUI _confirmLabel;

        [Header("Confirmations")]
        [SerializeField] private Confirmation _collectRewards;
        [SerializeField] private Confirmation _loseRewards;
        [SerializeField] private Confirmation _noRewards;

        private GameRun _run;
        private Action _onConfirmed;

        public void Initialize(GameRun run) {
            _run = run;
            _run.StateChanged += OnStateChanged;
            _confirmButton.onClick.AddListener(OnConfirmClicked);
            _goBackButton.onClick.AddListener(Close);
        }

        public void Ask(ExitOutcome outcome, Action onConfirmed) {
            _onConfirmed = onConfirmed;
            Show(ConfirmationFor(outcome));
            gameObject.SetActive(true);
        }

        private void OnDestroy() {
            if (_run == null) return;

            _run.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(RunState state) {
            Close();
        }

        private void OnConfirmClicked() {
            Close();
            _onConfirmed();
        }

        private Confirmation ConfirmationFor(ExitOutcome outcome) {
            switch (outcome) {
                case ExitOutcome.CollectRewards: return _collectRewards;
                case ExitOutcome.LoseRewards: return _loseRewards;
                case ExitOutcome.NoRewards: return _noRewards;
                default: throw new ArgumentOutOfRangeException(nameof(outcome), outcome, null);
            }
        }

        private void Show(Confirmation confirmation) {
            _titleText.text = confirmation.Title;
            _titleText.gameObject.SetActive(!string.IsNullOrEmpty(confirmation.Title));
            _messageText.text = confirmation.Message;
            _confirmLabel.text = confirmation.ConfirmLabel;
            _confirmButton.image.color = confirmation.ConfirmColor;
        }

        private void Close() {
            gameObject.SetActive(false);
        }

    #if UNITY_EDITOR
        private void OnValidate() {
            _confirmButton = ChildButtons.FindIfMissing(this, _confirmButton, ConfirmButtonName);
            _goBackButton = ChildButtons.FindIfMissing(this, _goBackButton, GoBackButtonName);
        }
    #endif
    }
}
