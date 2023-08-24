using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "AddCardToHand_", menuName = "Curry/Effects/AddCardToHand", order = 1)]
    public class AddCardToHand_EffectResource : BaseEffectResource 
    {
        [SerializeField] AddCardToHand m_addCard = default;
        public AddCardToHand AddCard => m_addCard;
        public override void Activate(GameStateContext context)
        {
            m_addCard?.ApplyEffect(context.Deck);
        }
    }
}
