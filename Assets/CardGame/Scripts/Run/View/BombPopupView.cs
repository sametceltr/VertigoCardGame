using CardGame.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Run.View
{
    public class BombPopupView : MonoBehaviour
    {
        private const string GiveUpButtonName = "ui_button_give_up";
        private const string ReviveWithGoldButtonName = "ui_button_revive_coin";
        private const string ReviveWithAdButtonName = "ui_button_revive_watch";
        private const string ReviveCostFormat = "Revive for {0}";

        [Header("Buttons")]
        [SerializeField] private Button _giveUpButton;
        [SerializeField] private Button _reviveWithGoldButton;
        [SerializeField] private Button _reviveWithAdButton;

        [Header("Revive")]
        [SerializeField] private TextMeshProUGUI _reviveCostText;

        private GameRun _run;

        public void Initialize(GameRun run) {
            _run = run;
            _run.StateChanged += OnStateChanged;

            _giveUpButton.onClick.AddListener(_run.GiveUp);
            _reviveWithGoldButton.onClick.AddListener(_run.ReviveWithGold);
            _reviveWithAdButton.onClick.AddListener(_run.ReviveWithAd);
        }

        private void OnDestroy() {
            if (_run == null) return;

            _run.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(RunState state) {
            bool isBombHit = state == RunState.BombHit;
            gameObject.SetActive(isBombHit);
            if (isBombHit) ShowReviveOffer();
        }

        private void ShowReviveOffer() {
            _reviveCostText.text = string.Format(ReviveCostFormat, AmountFormatter.Exact(_run.ReviveCost));
            _reviveWithGoldButton.interactable = _run.CanReviveWithGold;
            _reviveWithAdButton.gameObject.SetActive(_run.CanReviveWithAd);
        }

    #if UNITY_EDITOR
        private void OnValidate() {
            _giveUpButton = ChildButtons.FindIfMissing(this, _giveUpButton, GiveUpButtonName);
            _reviveWithGoldButton = ChildButtons.FindIfMissing(this, _reviveWithGoldButton, ReviveWithGoldButtonName);
            _reviveWithAdButton = ChildButtons.FindIfMissing(this, _reviveWithAdButton, ReviveWithAdButtonName);
        }
    #endif
    }
}
