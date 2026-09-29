public abstract class WheelState
{
    protected WheelStateMachine stateMachine;

    public WheelState(WheelStateMachine machine) {
        stateMachine = machine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void OnSpinRequested() { }
}