using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class Push : PropertyAttribute, ICharacterEffectModule
    {
        [Range(1, 3)]
        [SerializeField] protected int m_pushPower = default;
        public void ApplyEffect(ICharacter target, ICharacter user)
        {
            if(target is IMovable movable) 
            {
                Vector2 diff = target.WorldPosition - user.WorldPosition;
                // Limit to unit values
                diff.x = Mathf.Clamp(diff.x, -1f, 1f);
                diff.y = Mathf.Clamp(diff.y, -1f, 1f);
                Vector2Int push = new Vector2Int((int)diff.x, (int)diff.y);
                push *= m_pushPower;
                Vector3 targetPos = target.WorldPosition + new Vector3(push.x, push.y, 0f);
                movable.Move(targetPos);
            }
        }
    }
}
