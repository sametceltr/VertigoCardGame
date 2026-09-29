using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


[CreateAssetMenu(fileName = "RewardConfigSO", menuName = "Custom/RewardConfigSO")]
public class RewardConfigSO : ScriptableObject
{
    [SerializeField] private RewardConfig[] rewardConfigs = new RewardConfig[0];
    private readonly Dictionary<RewardType, RewardConfig> configDictionary = new();
    public RewardConfig[] RewardConfigs => rewardConfigs;

    private void OnEnable() {
        configDictionary.Clear();

        for (int i = 0; i < rewardConfigs.Length; i++) {

            var config = rewardConfigs[i];

            try {
                configDictionary.Add(config.RewardType, config);
            }
            catch (System.ArgumentException) {
                Debug.LogError($"Duplicate RewardType found in configuration: {config.RewardType}. Please correct the array in the Inspector.");
            }
        }
    }

    public RewardConfig GetConfig(RewardType type) {
        if (configDictionary.ContainsKey(type)) return configDictionary[type];
        return null;
    }

}