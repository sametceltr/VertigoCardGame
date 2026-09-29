using System;
using UnityEngine;

[Serializable]
public class Reward  
{
    [SerializeField] private RewardType _rewardType;
    private float _value;
    [SerializeField] private int _amount;
    [SerializeField] private float _probability;

    public Reward(RewardType rewardType, float value, int amount, float probability = 0) {
        _rewardType = rewardType;
        _value = amount;
        _amount = amount;
        _probability = probability;
    }

    public RewardType RewardType => _rewardType;
    public float Value => _value;
    public int Amount => _amount;
    public float Probability {  get { return _probability; } set { _probability = value; } }
}