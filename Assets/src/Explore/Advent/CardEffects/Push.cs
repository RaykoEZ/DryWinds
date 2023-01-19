using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Push : IEffectModule
    {
        [Range(1, 3)]
        [SerializeField] protected int m_pushPower = default;
        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            Vector2 diff = target.WorldPosition - user.WorldPosition;
            Vector2Int push = new Vector2Int((int)diff.x, (int)diff.y);
            target.Move(m_pushPower * push);
        }
    }
}
