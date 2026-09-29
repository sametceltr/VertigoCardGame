using UnityEngine;

public class RandomSpinStrategy : IWheelSpinStrategy
{
    private readonly WheelView wheelView;
    private readonly float spinDuration = 3f;
    private readonly int minSpins = 2;
    private readonly int maxSpins = 5;

    public RandomSpinStrategy(WheelView view) {
        wheelView = view;
    }

    public SpinResult CalculateSpin() {
        var rewards = wheelView.GetCurrentRewards();

        float randomValue = Random.Range(0f, 1f);
        Reward selectedReward = rewards[0];
        int selectedIndex = 0;

        for (int i = 0; i < rewards.Length; i++) {
            randomValue -= rewards[i].Probability;
            if (randomValue <= 0) {
                selectedReward = rewards[i];
                selectedIndex = i;
                break;
            }
        }

        int fullSpins = Random.Range(minSpins, maxSpins);
        float targetDegree = -360 * fullSpins;
        float sliceAngle = 360f / rewards.Length;
        targetDegree -= (360 - sliceAngle * selectedIndex);

        return new SpinResult(selectedReward, targetDegree, spinDuration);
    }
}