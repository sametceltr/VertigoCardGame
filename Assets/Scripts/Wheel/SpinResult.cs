public struct SpinResult
{
    public Reward Reward;
    public float TargetRotation;
    public float Duration;

    public SpinResult(Reward reward, float rotation, float duration) {
        Reward = reward;
        TargetRotation = rotation;
        Duration = duration;
    }
}