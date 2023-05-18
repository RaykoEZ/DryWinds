using System;
using UnityEngine;
using System.Collections;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Card_", menuName = "Curry/Card/New Card Resource", order = 1)]
    public class CardResource : ScriptableObject 
    {
        [SerializeField] CardAttribute m_attribute = default;
        public CardAttribute Attribute => m_attribute;
    }
}
