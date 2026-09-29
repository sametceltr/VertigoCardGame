public class WheelStateMachine
{
    private WheelState currentState;
    private readonly IWheelView wheelView;
    private readonly IWheelSpinStrategy spinStrategy;

    public WheelStateMachine(IWheelView view, IWheelSpinStrategy strategy) {
        wheelView = view;
        spinStrategy = strategy;
        currentState = new WheelIdleState(this);
        currentState.Enter();
    }

    public void TransitionTo(WheelState newState) {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void RequestSpin() {
        currentState.OnSpinRequested();
    }

    public void SetSpinButtonInteractable(bool interactable) {
        wheelView.SetSpinButtonInteractable(interactable);
    }

    public void PerformSpin() {
        var result = spinStrategy.CalculateSpin();
        wheelView.AnimateSpin(result, () => OnSpinComplete(result.Reward));
    }

    private void OnSpinComplete(Reward reward) {
        TransitionTo(new WheelResultState(this, reward));
    }

    public void ProcessReward(Reward reward) {
        wheelView.ProcessReward(reward);
    }
}