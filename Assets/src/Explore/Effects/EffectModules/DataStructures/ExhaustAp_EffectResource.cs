using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "ExhaustAP_", menuName = "Curry/Effects/Exhaust AP", order = 1)]
    public class ExhaustAp_EffectResource : BaseEffectResource 
    {
        [SerializeField] ExhaustActionPoint m_exhaust = default;
        public ExhaustActionPoint Effect => m_exhaust;
        public override void Activate(GameStateContext context)
        {
            m_exhaust?.ApplyEffect(context.ActionCount, out _);
        }
    }
}
