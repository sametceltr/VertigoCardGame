using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZoneConfigSO", menuName = "Custom/ZoneConfigSO")]
public class ZoneConfigSO : ScriptableObject
{
    [SerializeField] private ZoneConfig[] zoneConfigs = new ZoneConfig[3];
    private readonly Dictionary<ZoneType, ZoneConfig> configDictionary = new();

    private void OnEnable() {
        configDictionary.Clear();

        for (int i = 0; i < zoneConfigs.Length; i++) {

            var config = zoneConfigs[i];

            try {
                configDictionary.Add(config.ZoneType, config);
            }
            catch (System.ArgumentException) {
                Debug.LogError($"Duplicate WheelType found in configuration: {config.ZoneType}. Please correct the array in the Inspector.");
            }
        }
    }


    public ZoneConfig GetConfig(ZoneType type) {
        if (configDictionary.ContainsKey(type)) return configDictionary[type];
        return null;
    }

}