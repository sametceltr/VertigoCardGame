public class GuaranteedSpinStrategy : IWheelSpinStrategy
{
    private readonly WheelView wheelView;
    private readonly int guaranteedIndex;

    public GuaranteedSpinStrategy(WheelView view, int index) {
        wheelView = view;
        guaranteedIndex = index;
    }

    public SpinResult CalculateSpin() {
        var rewards = wheelView.GetCurrentRewards();
        var reward = rewards[guaranteedIndex];

        float sliceAngle = 360f / rewards.Length;
        float targetDegree = -360 * 3;
        targetDegree -= (360 - sliceAngle * guaranteedIndex);

        return new SpinResult(reward, targetDegree, 2f);
    }
}