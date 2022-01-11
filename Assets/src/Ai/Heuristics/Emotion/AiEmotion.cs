using System;
using UnityEngine;

namespace Curry.Ai
{
    public enum AiEmotionState
    {
        Normal,
        Hatred,
        Fear,
    }

    [Serializable]
    public class AiEmotion
    {
        public virtual float Hatred { get; protected set; }
        public virtual float Fear { get; protected set; }
        public virtual AiEmotionState EmotionState 
        { 
            get
            {
                float diff = Hatred - Fear;
                float magnitude = Mathf.Abs(diff);
                if (diff < 0f && magnitude > 0.75f) 
                {
                    return AiEmotionState.Fear;
                }
                else if(diff > 0f && magnitude > 0.75f)
                {
                    return AiEmotionState.Hatred;
                }
                else 
                {
                    return AiEmotionState.Normal;
                }
            } 
        }

        public AiEmotion()
        {
            Hatred = 0f;
            Fear = 0f;
        }

        public AiEmotion(float hatred, float fear)
        {
            Hatred = hatred;
            Fear = fear;
        }

        public static AiEmotion operator -(AiEmotion a) => new AiEmotion(-a.Hatred, -a.Fear);
        public static AiEmotion operator -(AiEmotion a, AiEmotion b) => a + (-b);
        public static AiEmotion operator +(AiEmotion a, AiEmotion b)
        => new AiEmotion(
            Mathf.Clamp(a.Hatred + b.Hatred,-1f, 1f), 
            Mathf.Clamp(a.Fear + b.Fear, -1f, 1f));
        public static AiEmotion operator *(AiEmotion a, AiEmotion b)
        => new AiEmotion(
            Mathf.Clamp(a.Hatred * b.Hatred, -1f, 1f),
            Mathf.Clamp(a.Fear * b.Fear, -1f, 1f));
        public static AiEmotion operator /(AiEmotion a, AiEmotion b)
        {
            if (b.Hatred == 0f || b.Fear == 0f)
            {
                throw new DivideByZeroException();
            }
            return new AiEmotion(
                Mathf.Clamp(a.Hatred / b.Hatred, -1f, 1f),
                Mathf.Clamp(a.Fear / b.Fear, -1f, 1f));
        }
        public override string ToString() => $"Emnity: {Hatred}/n Fear: {Fear}";
    }
}
