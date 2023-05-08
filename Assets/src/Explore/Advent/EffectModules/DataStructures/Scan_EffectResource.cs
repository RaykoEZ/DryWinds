using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Scan_EffectResource : BaseEffectResource
    {
        [SerializeField] Scan m_scan = default;
        public Scan ScanModule => m_scan;
    }
}
