using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class MovementPointChange : PropertyAttribute
    {
        [SerializeField] int m_changeValue = default;
        public void ApplyEffect(ActionCounter target)
        {
            target.UpdateMoveCountDisplay(target.CurrentActionPoint + m_changeValue);
        }
    }
}
