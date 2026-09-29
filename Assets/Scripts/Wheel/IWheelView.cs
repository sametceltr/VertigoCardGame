public interface IWheelView
{
    void SetSpinButtonInteractable(bool interactable);
    void AnimateSpin(SpinResult result, System.Action onComplete);
    void ProcessReward(Reward reward);
}