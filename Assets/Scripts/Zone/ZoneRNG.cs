using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class ZoneRNG
{

    [Header("Reward Settings")]
    [SerializeField] private RewardConfigSO rewardSO;

    [Header("Zone Value Settings")]
    [SerializeField] private AnimationCurve zoneValueCurve;
    [SerializeField] private AnimationCurve sliceValueProbablityCurve;

    [SerializeField] private int maxZone = 60;
    [SerializeField] private float zoneValueScale = 10000f;
    [SerializeField] private float sliceValueScale = 10f;
    [SerializeField] private int integrationSteps = 100;

    private bool isInitialized = false;
    private float logMin;
    private float logMax;
    private float logRange;

    private int sliceCount = 8;

    private float _totalArea = 0f;

    private System.Random rng = new System.Random();

    private void Initialize() {
        logMin = -Mathf.Log10(sliceValueScale);
        logMax = Mathf.Log10(sliceValueScale);
        logRange = logMax - logMin;

        _totalArea = CalculateTotalArea(sliceValueProbablityCurve, integrationSteps);
    }


    private float CalculateTotalArea(AnimationCurve curve, int steps) {
        float area = 0f;
        float stepSize = 1f / steps;

        for (int i = 0; i < steps; i++) {
            float midpointX = i * stepSize + (stepSize / 2f);
            area += curve.Evaluate(midpointX) * stepSize;
        }
        return area;
    }

    private float SampleCurve(AnimationCurve curve, float totalArea, int steps) {
        if (totalArea <= 0f) return 0.5f;

        float targetArea = UnityEngine.Random.Range(0f, totalArea);
        float accumulatedArea = 0f;
        float stepSize = 1f / steps;

        for (int i = 0; i < steps; i++) {
            float midpointX = i * stepSize + (stepSize / 2f);
            float sliceArea = curve.Evaluate(midpointX) * stepSize;

            if (accumulatedArea + sliceArea >= targetArea) {
                return midpointX;
            }

            accumulatedArea += sliceArea;
        }

        return 1.0f;
    }

    public float GetWeightedValueScale() {
        float weightedLogInput = SampleCurve(sliceValueProbablityCurve, _totalArea, integrationSteps);

        float resultingLogValue = logMin + (weightedLogInput * logRange);

        float finalNumber = Mathf.Pow(10f, resultingLogValue);

        return finalNumber;
    }


    public Reward[] GenerateZoneRewards(Zone zone) {
        if (!isInitialized) Initialize();

        var zoneIndex = zone.Index;

        float timeValue = (float)zoneIndex / maxZone;
        float zoneValue = zoneValueCurve.Evaluate(timeValue) * zoneValueScale;

        var sortedRewardTypes = rewardSO.RewardConfigs.Where(item => (item.BaseMultiplier <= zoneValue) && item.RewardType != RewardType.BOMB).ToArray();
        Debug.Log("--------------------------------------");
        foreach (var rewardType in sortedRewardTypes) Debug.Log(rewardType.RewardType);

        Reward[] slices = new Reward[sliceCount];
        float realTotalValue = 0;
        int bombIndex = zone.Type == ZoneType.NORMAL ? rng.Next(sliceCount): sliceCount;

        for (int i = 0; i < sliceCount; i++) {
            if (i == bombIndex) {
                slices[i] = new Reward(
                        RewardType.BOMB,
                        0,
                        0
                    );
                continue;
            }

            var reward = sortedRewardTypes[rng.Next(sortedRewardTypes.Length)];

            var weightedValueScale = GetWeightedValueScale();
            float sliceValue = zoneValue * weightedValueScale;
            realTotalValue += sliceValue;

            float amount = sliceValue / reward.BaseMultiplier;

            amount = CoolRound(amount);

            slices[i] = new Reward(
                reward.RewardType,
                sliceValue,
                (int)amount
            );
        }

        int bombProbabilityIndex = zone.Type == ZoneType.NORMAL ? rng.Next(sliceCount) : -1;

        while (bombProbabilityIndex == bombIndex) {
            bombProbabilityIndex = rng.Next(sliceCount);
        }

        for (int i = 0; i < slices.Length; i++) {
            if (i == bombIndex) continue;
            if (i == bombProbabilityIndex) {
                slices[bombIndex].Probability = (slices[i].Value / realTotalValue) / 2;
                slices[i].Probability = (slices[i].Value / realTotalValue) / 2;
                continue;
            }
            slices[i].Probability = slices[i].Value / realTotalValue;
        }

        float currentProbSum = 0f;
        for (int i = 0; i < slices.Length; i++) {
            currentProbSum += slices[i].Probability;
        }

        if (currentProbSum > 0) {
            for (int i = 0; i < slices.Length; i++) {
                slices[i].Probability /= currentProbSum;
            }
        }

        return slices;
    }

    private float CoolRound(float value) {
        if (value < 1000) return value;
        int digits = (int)Mathf.Floor(Mathf.Log10(value));
        float pow = Mathf.Pow(10, digits);
        float leading = Mathf.Floor(value / pow);
        return Mathf.Max(1, leading * pow);
    }
}
