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
        float m_hate = 0f;
        float m_fear = 0f;
        public virtual float Hatred { 
            get { return m_hate; } 
            set { m_hate = Mathf.Clamp(value, 0f, 1f); }
        }
        public virtual float Fear { 
            get { return m_fear; }
            set { m_fear = Mathf.Clamp(value, 0f, 1f); }
        }
        public virtual AiEmotionState EmotionState 
        { 
            get
            {
                float diff = Hatred - Fear;
                float magnitude = Mathf.Abs(diff);
                if (diff < 0f && magnitude > 0.2f) 
                {
                    return AiEmotionState.Fear;
                }
                else if(diff > 0f && magnitude > 0.2f)
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
                Mathf.Clamp(a.Hatred / b.Hatred, 0f, 1f),
                Mathf.Clamp(a.Fear / b.Fear, 0f, 1f));
        }
        public override string ToString() => $"State: {EmotionState} \n Emnity: {Hatred},  Fear: {Fear}";
    }
}
