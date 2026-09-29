using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WheelConfigSO", menuName = "Custom/WheelConfigSO")]
public class WheelConfigSO : ScriptableObject
{
    [SerializeField] private WheelConfig[] wheelConfigurations = new WheelConfig[3];
    private readonly Dictionary<WheelType, WheelConfig> configDictionary = new();


    private void OnEnable() {
        configDictionary.Clear();

        for (int i=0; i<wheelConfigurations.Length; i++) {

            var config = wheelConfigurations[i];

            try {
                configDictionary.Add(config.WheelType, config);
            }
            catch (System.ArgumentException) {
                Debug.LogError($"Duplicate WheelType found in configuration: {config.WheelType}. Please correct the array in the Inspector.");
            }
        }
    }

    public WheelConfig GetConfig(WheelType type) {
        if (configDictionary.ContainsKey(type)) return configDictionary[type];
        return null;
    }
}