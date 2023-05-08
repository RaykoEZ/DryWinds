using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class AddCardToHand_EffectResource : BaseEffectResource 
    {
        [SerializeField] AddCardToHand m_addCard = default;
        public AddCardToHand AddCard => m_addCard;
    }
}
