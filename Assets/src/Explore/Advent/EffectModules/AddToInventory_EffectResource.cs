using System;
using UnityEngine;
namespace Curry.Explore
{
    // Scriptable object to store an instance of Effect Module(s) for any use (cards, abilities, encounters)
    [Serializable]
    [CreateAssetMenu(fileName = "AddToInventory_", menuName = "Curry/Effects/AddToInventory", order = 1)]
    public class AddToInventory_EffectResource : BaseEffectResource
    {      
        [SerializeField] AddToInventory m_effect = default;
        public AddToInventory Effect => m_effect;
        public override void Activate(GameStateContext context)
        {
            m_effect?.ApplyEffect(context.Deck);
        }

    }
}