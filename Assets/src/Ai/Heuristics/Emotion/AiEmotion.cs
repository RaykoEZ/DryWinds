using System;
using UnityEngine;

namespace Curry.Ai
{
    [Serializable]
    public class AiEmotion
    {
        public virtual float EmnityValue { get; protected set; }
        public virtual float FearValue { get; protected set; }

        public virtual bool IsFearful { get { return FearValue > 0f; } }
        public virtual bool IsHostile { get { return EmnityValue > 0f; } }

        public AiEmotion()
        {
            EmnityValue = 0f;
            FearValue = 0f;
        }

        public AiEmotion(float emnity, float fear)
        {
            EmnityValue = emnity;
            FearValue = fear;
        }

        public static AiEmotion operator -(AiEmotion a) => new AiEmotion(-a.EmnityValue, -a.FearValue);
        public static AiEmotion operator -(AiEmotion a, AiEmotion b) => a + (-b);
        public static AiEmotion operator +(AiEmotion a, AiEmotion b)
        => new AiEmotion(
            Mathf.Clamp(a.EmnityValue + b.EmnityValue,-1f, 1f), 
            Mathf.Clamp(a.FearValue + b.FearValue, -1f, 1f));
        public static AiEmotion operator *(AiEmotion a, AiEmotion b)
        => new AiEmotion(
            Mathf.Clamp(a.EmnityValue * b.EmnityValue, -1f, 1f),
            Mathf.Clamp(a.FearValue * b.FearValue, -1f, 1f));
        public static AiEmotion operator /(AiEmotion a, AiEmotion b)
        {
            if (b.EmnityValue == 0f || b.FearValue == 0f)
            {
                throw new DivideByZeroException();
            }
            return new AiEmotion(
                Mathf.Clamp(a.EmnityValue / b.EmnityValue, -1f, 1f),
                Mathf.Clamp(a.FearValue / b.FearValue, -1f, 1f));
        }
        public override string ToString() => $"Emnity: {EmnityValue}/n Fear: {FearValue}";
    }
}
