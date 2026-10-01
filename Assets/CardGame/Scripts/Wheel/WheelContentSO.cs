using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Wheel
{
    [CreateAssetMenu(fileName = "WheelContent", menuName = "CardGame/Wheel Content")]
    public class WheelContentSO : ScriptableObject
    {
        [SerializeField] private SliceEntry[] _slices;

        public IReadOnlyList<SliceEntry> Slices => _slices;

        private void OnValidate() {
            if (_slices == null) return;

            int totalLandingWeight = 0;
            int bombCount = 0;
            for (int i = 0; i < _slices.Length; i++) {
                var slice = _slices[i];
                if (slice.LandingWeight < 0) Debug.LogWarning($"{name}: slice {i + 1} has a negative landing weight.", this);
                totalLandingWeight += slice.LandingWeight;

                if (slice.Type == SliceType.Bomb) {
                    bombCount++;
                    if (slice.Pool != null && slice.Pool.Count > 0) Debug.LogWarning($"{name}: bomb slice {i + 1} has pool entries; they are never used.", this);
                } else {
                    ValidatePool(slice, i);
                }
            }

            if (bombCount > 1) Debug.LogWarning($"{name}: has {bombCount} bomb slices; a wheel can have at most one.", this);
            if (_slices.Length > 0 && totalLandingWeight <= 0) Debug.LogWarning($"{name}: no slice has a positive landing weight.", this);
        }

        private void ValidatePool(SliceEntry slice, int sliceIndex) {
            if (slice.Pool == null || slice.Pool.Count == 0) {
                Debug.LogWarning($"{name}: reward slice {sliceIndex + 1} has an empty pool.", this);
                return;
            }

            int totalAppearanceWeight = 0;
            foreach (var entry in slice.Pool) {
                if (entry.Reward == null) Debug.LogWarning($"{name}: slice {sliceIndex + 1} has a pool entry without a reward.", this);
                if (entry.AppearanceWeight < 0) Debug.LogWarning($"{name}: slice {sliceIndex + 1} has a negative appearance weight.", this);
                totalAppearanceWeight += entry.AppearanceWeight;
            }

            if (totalAppearanceWeight <= 0) Debug.LogWarning($"{name}: slice {sliceIndex + 1} has no pool entry with a positive appearance weight.", this);
        }
    }
}
