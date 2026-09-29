using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BombMenuView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button giveUpButton;
    [SerializeField] private Button coinReviveButton;
    [SerializeField] private Button watchReviveButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI coinReviveText;

    private IBombAction giveUpAction;
    private IBombAction coinReviveAction;
    private IBombAction adReviveAction;

#if UNITY_EDITOR
    private void OnValidate() {
        if (giveUpButton != null && coinReviveButton != null && watchReviveButton != null) return;

        var children = transform.GetComponentsInChildren<Button>();

        foreach (var child in children) {
            if (child.name.Contains("give")) giveUpButton = child;
            else if (child.name.Contains("coin")) coinReviveButton = child;
            else if (child.name.Contains("watch")) watchReviveButton = child;
        }
    }
#endif

    private void Awake() {
        InitializeActions();
    }

    private void InitializeActions() {
        giveUpAction = new GiveUpAction();
        coinReviveAction = new CoinReviveAction();
        adReviveAction = new AdReviveAction();
    }

    private void OnEnable() {
        giveUpButton.onClick.AddListener(OnGiveUpClicked);
        coinReviveButton.onClick.AddListener(OnCoinReviveClicked);
        watchReviveButton.onClick.AddListener(OnAdReviveClicked);

        UpdateReviveCostText();
        UpdateButtonStates();
    }

    private void OnDisable() {
        giveUpButton.onClick.RemoveAllListeners();
        coinReviveButton.onClick.RemoveAllListeners();
        watchReviveButton.onClick.RemoveAllListeners();
    }

    private void OnGiveUpClicked() {
        if (giveUpAction.CanExecute()) {
            giveUpAction.Execute();
            Close();
        }
    }

    private void OnCoinReviveClicked() {
        if (coinReviveAction.CanExecute()) {
            coinReviveAction.Execute();
            Close();
        }
    }

    private void OnAdReviveClicked() {
        if (adReviveAction.CanExecute()) {
            adReviveAction.Execute();
            Close();
        }
    }

    private void UpdateReviveCostText() {
        coinReviveText.text = $"Revive for {ServiceLocator.Economy.ReviveCost}";
    }

    private void UpdateButtonStates() {
        coinReviveButton.interactable = coinReviveAction.CanExecute();
    }

    private void Close() {
        Destroy(gameObject);
    }
}