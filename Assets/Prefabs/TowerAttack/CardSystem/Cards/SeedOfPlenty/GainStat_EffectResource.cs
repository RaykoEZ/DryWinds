using System;
using UnityEngine;
namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "GainStat_", menuName = "Curry/Effects/Gain Stat", order = 1)]
    public class GainStat_EffectResource : BaseEffectResource
    {
        [SerializeField] public StatUp m_gainStats = default;
        public StatUp Effect => m_gainStats;
    }
}