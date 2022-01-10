using System;
using UnityEngine;

namespace Curry.Ai
{
    [Serializable]
    public class AiEmotion
    {
        public virtual float Emnity { get; protected set; }
        public virtual float Fear { get; protected set; }

        public virtual bool IsFearful { get { return Fear > 0f; } }
        public virtual bool IsHostile { get { return Emnity > 0f; } }

        public AiEmotion()
        {
            Emnity = 0f;
            Fear = 0f;
        }

        public AiEmotion(float emnity, float fear)
        {
            Emnity = emnity;
            Fear = fear;
        }

        public static AiEmotion operator -(AiEmotion a) => new AiEmotion(-a.Emnity, -a.Fear);
        public static AiEmotion operator -(AiEmotion a, AiEmotion b) => a + (-b);
        public static AiEmotion operator +(AiEmotion a, AiEmotion b)
        => new AiEmotion(
            Mathf.Clamp(a.Emnity + b.Emnity,-1f, 1f), 
            Mathf.Clamp(a.Fear + b.Fear, -1f, 1f));
        public static AiEmotion operator *(AiEmotion a, AiEmotion b)
        => new AiEmotion(
            Mathf.Clamp(a.Emnity * b.Emnity, -1f, 1f),
            Mathf.Clamp(a.Fear * b.Fear, -1f, 1f));
        public static AiEmotion operator /(AiEmotion a, AiEmotion b)
        {
            if (b.Emnity == 0f || b.Fear == 0f)
            {
                throw new DivideByZeroException();
            }
            return new AiEmotion(
                Mathf.Clamp(a.Emnity / b.Emnity, -1f, 1f),
                Mathf.Clamp(a.Fear / b.Fear, -1f, 1f));
        }
        public override string ToString() => $"Emnity: {Emnity}/n Fear: {Fear}";
    }
}
