using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "DropLoot_", menuName = "Curry/Effects/DropLoot", order = 1)]
    public class DropLoot_EffectResource : BaseEffectResource
    {
        [SerializeField] DropLoot m_loot = default;
        public override void Activate(GameStateContext context)
        {
            m_loot?.ApplyEffect(context.LootManager);
        }
    }
}
