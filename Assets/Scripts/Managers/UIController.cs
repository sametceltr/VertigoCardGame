using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI coinBalanceText;
    [SerializeField] private Button spinButton;

#if UNITY_EDITOR
    private void OnValidate() {
        if (spinButton != null) return;

        var children = GameObject.FindGameObjectWithTag("Canvas").transform.GetComponentsInChildren<Button>();

        foreach (var child in children) {
            if (child.name.Contains("spin")) spinButton = child;
        }
    }
#endif

    private void Awake() {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void OnEnable() {
        GameEvents.OnCoinBalanceChanged += UpdateCoinDisplay;
        GameEvents.OnSpinStarted += DisableSpinButton;
        GameEvents.OnSpinCompleted += EnableSpinButton;
    }

    private void OnDisable() {
        GameEvents.OnCoinBalanceChanged -= UpdateCoinDisplay;
        GameEvents.OnSpinStarted -= DisableSpinButton;
        GameEvents.OnSpinCompleted -= EnableSpinButton;
    }

    private void Start() {  
        UpdateCoinDisplay(ServiceLocator.Economy.CurrentBalance);
    }

    private void UpdateCoinDisplay(int newBalance) {
        coinBalanceText.text = newBalance.ToString();
    }

    private void DisableSpinButton() {
        spinButton.interactable = false;
    }

    private void EnableSpinButton(Reward reward) {
        spinButton.interactable = true;
    }
}
