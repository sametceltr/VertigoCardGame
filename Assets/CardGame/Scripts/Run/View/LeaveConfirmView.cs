using CardGame.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Run.View
{
    public class LeaveConfirmView : MonoBehaviour
    {
        private const string ConfirmButtonName = "ui_button_confirm";
        private const string GoBackButtonName = "ui_button_go_back";

        [Header("Buttons")]
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _goBackButton;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private TextMeshProUGUI _confirmLabel;

        [Header("Collect (safe and super zones)")]
        [SerializeField, TextArea] private string _collectMessage = "Want to exit and collect your rewards?";
        [SerializeField] private string _collectLabel = "Collect Rewards";
        [SerializeField] private Color _collectButtonColor = Color.green;

        [Header("Quit (other zones)")]
        [SerializeField, TextArea] private string _loseRewardsMessage = "Leaving now loses your rewards. Still want to quit?";
        [SerializeField, TextArea] private string _noRewardsMessage = "No rewards are available. Still want to quit?";
        [SerializeField] private string _quitLabel = "Exit";
        [SerializeField] private Color _quitButtonColor = Color.white;

        private GameRun _run;

        public void Initialize(GameRun run) {
            _run = run;
            _run.StateChanged += OnStateChanged;
            _confirmButton.onClick.AddListener(OnConfirmClicked);
            _goBackButton.onClick.AddListener(Close);
        }

        public void Open() {
            if (_run.CanCollectRewards) ShowOffer(_collectMessage, _collectLabel, _collectButtonColor);
            else ShowOffer(_run.CollectedRewards.Count > 0 ? _loseRewardsMessage : _noRewardsMessage, _quitLabel, _quitButtonColor);
            gameObject.SetActive(true);
        }

        private void OnDestroy() {
            if (_run == null) return;

            _run.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(RunState state) {
            if (state != RunState.Ready) Close();
        }

        private void OnConfirmClicked() {
            Close();
            _run.Leave();
        }

        private void ShowOffer(string message, string confirmLabel, Color confirmColor) {
            _messageText.text = message;
            _confirmLabel.text = confirmLabel;
            _confirmButton.image.color = confirmColor;
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
