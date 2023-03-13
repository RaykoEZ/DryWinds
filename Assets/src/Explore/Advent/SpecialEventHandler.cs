using Curry.Events;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    // These events contain event cards to draw once only (until replenished)
    public class SpecialEventHandler : MonoBehaviour
    {
        [SerializeField] protected int m_specialEncounterId = default;
        public bool CanTrigger { get; set; } = true;
        public int EncounterId => m_specialEncounterId;
    }

}
