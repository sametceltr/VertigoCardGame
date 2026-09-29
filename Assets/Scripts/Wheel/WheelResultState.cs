public class WheelResultState : WheelState
{
    private Reward reward;

    public WheelResultState(WheelStateMachine machine, Reward reward) : base(machine) {
        this.reward = reward;
    }

    public override void Enter() {
        stateMachine.ProcessReward(reward);
        stateMachine.TransitionTo(new WheelIdleState(stateMachine));
    }
}