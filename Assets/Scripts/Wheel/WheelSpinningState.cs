
public class WheelSpinningState : WheelState
{
    public WheelSpinningState(WheelStateMachine machine) : base(machine) { }

    public override void Enter() {
        stateMachine.SetSpinButtonInteractable(false);
        stateMachine.PerformSpin();
    }

    public override void OnSpinRequested() { }
}