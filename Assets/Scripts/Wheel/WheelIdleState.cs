public class WheelIdleState : WheelState
{
    public WheelIdleState(WheelStateMachine machine) : base(machine) { }

    public override void Enter() {
        stateMachine.SetSpinButtonInteractable(true);
    }

    public override void OnSpinRequested() {
        stateMachine.TransitionTo(new WheelSpinningState(stateMachine));
    }
}