using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "ExhaustActionPoint_", menuName = "Curry/Effects/ExhaustActionPoint", order = 1)]
    public class ExhaustActionPoint_EffectResource : BaseEffectResource 
    {
        [SerializeField] ExhaustActionPoint m_exhaust = default;
        public ExhaustActionPoint Effect => m_exhaust;
        public override void Activate(GameStateContext context)
        {
            m_exhaust?.ApplyEffect(context.ActionCount, out _);
        }
    }
}
