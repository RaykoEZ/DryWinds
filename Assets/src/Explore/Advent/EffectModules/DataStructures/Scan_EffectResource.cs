using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Scan_", menuName = "Curry/Effects/Scan", order = 1)]
    public class Scan_EffectResource : BaseEffectResource
    {
        [SerializeField] Scan m_scan = default;
        public Scan ScanModule => m_scan;
        public override void Activate(GameStateContext context)
        {
            m_scan?.ApplyEffect(context.Player, context.Player);
        }
    }
}
