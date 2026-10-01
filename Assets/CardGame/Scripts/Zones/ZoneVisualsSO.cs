using System;
using UnityEngine;

namespace CardGame.Zones
{
    [CreateAssetMenu(fileName = "ZoneVisuals", menuName = "CardGame/Zone Visuals")]
    public class ZoneVisualsSO : ScriptableObject
    {
        [SerializeField] private ZoneVisuals _normal;
        [SerializeField] private ZoneVisuals _safe;
        [SerializeField] private ZoneVisuals _super;

        public ZoneVisuals For(ZoneType type) {
            switch (type) {
                case ZoneType.Normal: return _normal;
                case ZoneType.Safe: return _safe;
                case ZoneType.Super: return _super;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
