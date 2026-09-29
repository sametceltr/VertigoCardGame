using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelView : MonoBehaviour, IWheelView
{
    [Header("Visual References")]
    [SerializeField] private WheelConfigSO wheelConfig;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image baseImage;
    [SerializeField] private Image indicatorImage;
    [SerializeField] private RewardSlice[] rewardSlices;

    [Header("Dependencies")]
    [SerializeField] private Button spinButton;

    private WheelStateMachine stateMachine;
    private WheelType currentWheelType;
    private Reward[] currentRewards;

    private void Awake() {
        var spinStrategy = new RandomSpinStrategy(this);
        stateMachine = new WheelStateMachine(this, spinStrategy);
    }

    private void OnEnable() {
        spinButton.onClick.AddListener(OnSpinButtonClicked);
        GameEvents.OnZoneChanged += OnZoneChanged;
        GameEvents.OnReviveRequested += GameEvents.ZoneLevelUp;
    }

    private void OnDisable() {
        spinButton.onClick.RemoveAllListeners();
        GameEvents.OnZoneChanged -= OnZoneChanged;
        GameEvents.OnReviveRequested -= GameEvents.ZoneLevelUp;
    }

    private void OnSpinButtonClicked() {
        stateMachine.RequestSpin();
    }

    private void OnZoneChanged(int zoneIndex) { 
    
    }

    public void Configure(WheelType wheelType, Reward[] rewards) {
        currentWheelType = wheelType;
        currentRewards = rewards;

        UpdateVisuals();
        UpdateSlices();
    }

    private void UpdateVisuals() {
        var config = wheelConfig.GetConfig(currentWheelType);
        if (config != null) {
            baseImage.sprite = config.BaseSprite;
            indicatorImage.sprite = config.IndicatorSprite;
            titleText.text = $"{currentWheelType} SPIN";
            titleText.color = config.TitleColor;
        }
    }

    private void UpdateSlices() {
        if (currentRewards == null || rewardSlices == null) return;

        int minLength = Mathf.Min(currentRewards.Length, rewardSlices.Length);
        for (int i = 0; i < minLength; i++) {
            if (rewardSlices[i] != null) {
                rewardSlices[i].UpdateSlice(currentRewards[i]);
            }
        }
    }

    public void SetSpinButtonInteractable(bool interactable) {
        spinButton.interactable = interactable;
    }

    public void AnimateSpin(SpinResult result, System.Action onComplete) {
        GameEvents.SpinStarted();

        var targetRotation = new Vector3(0, 0, result.TargetRotation);

        baseImage.transform
            .DORotate(targetRotation, result.Duration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuart)
            .OnComplete(() => {
                baseImage.transform.eulerAngles = Vector3.zero;
                GameEvents.SpinCompleted(result.Reward);
                onComplete?.Invoke();
            });
    }

    public void ProcessReward(Reward reward) {
        GameEvents.RewardAwarded(reward);

        if (reward.RewardType == RewardType.BOMB) {
            GameEvents.BombHit();
        } else {
            GameEvents.ZoneLevelUp();
        }
    }

    public Reward[] GetCurrentRewards() => currentRewards;
    public WheelType GetCurrentWheelType() => currentWheelType;
}