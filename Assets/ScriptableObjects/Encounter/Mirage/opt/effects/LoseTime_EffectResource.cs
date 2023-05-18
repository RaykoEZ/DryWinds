using System;
using UnityEngine;
namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "LoseTime_", menuName = "Curry/Effects/LoseTime", order = 1)]
    public class LoseTime_EffectResource : BaseEffectResource
    {     
        [SerializeField] LoseTime m_effect = default;
        public override void Activate(GameStateContext context)
        {
            m_effect?.ApplyEffect();
        }

    }
}