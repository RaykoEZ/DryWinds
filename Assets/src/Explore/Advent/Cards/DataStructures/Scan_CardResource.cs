using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Scan_CardResource : BaseCardResourceContainer 
    {
        [SerializeField] Scan_EffectResource m_scan = default;
        public Scan ScanModule => m_scan.ScanModule;
    }
}
