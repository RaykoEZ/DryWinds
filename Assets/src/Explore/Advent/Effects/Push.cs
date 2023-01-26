using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Push : ICharacterEffectModule
    {
        [Range(1, 3)]
        [SerializeField] protected int m_pushPower = default;
        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            Vector2 diff = target.WorldPosition - user.WorldPosition;
            // Limit to unit values
            diff.x = Mathf.Clamp(diff.x, -1f, 1f);
            diff.y = Mathf.Clamp(diff.y, -1f, 1f);
            Vector2Int push = new Vector2Int((int)diff.x, (int)diff.y);
            target.Move(m_pushPower * push);
        }
    }
}
