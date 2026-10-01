using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Wheel
{
    [Serializable]
    public class SliceEntry
    {
        [SerializeField] private SliceType _type;
        [SerializeField] private int _landingWeight;
        [SerializeField] private PoolEntry[] _pool;

        public SliceType Type => _type;
        public int LandingWeight => _landingWeight;
        public IReadOnlyList<PoolEntry> Pool => _pool;
    }
}
