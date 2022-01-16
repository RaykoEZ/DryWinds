﻿using UnityEngine;

namespace Curry.Ai
{
    public abstract class EmotionHandler
    {
        public virtual AiEmotion Default { get { return m_default; } }
        public virtual AiEmotion Current { get { return m_current; } }
        protected AiEmotion m_default;
        protected AiEmotion m_current;
        public virtual void Init() 
        {
            m_default = new AiEmotion();
            m_current = m_default;
        }
        public abstract void OnTakeDamage();
        public abstract void OnAllyDefeated();
        public abstract void OnThreatDetected();
        public abstract void OnHelped();

        public virtual void Update(out bool emotionChanged) 
        {
            AiEmotionState old = m_current.EmotionState;
            switch (Current.EmotionState) 
            {
                case AiEmotionState.Normal:
                {
                    m_current.Hatred -= 0.01f;
                    m_current.Fear -= 0.01f;
                    break;
                }
                case AiEmotionState.Hatred:
                {
                    m_current.Hatred -= 0.01f;
                    m_current.Fear -= 0.02f;
                    break;
                }
                case AiEmotionState.Fear:
                {
                    m_current.Hatred -= 0.02f;
                    m_current.Fear -= 0.01f;
                    break; 
                }
            }
            emotionChanged = old != m_current.EmotionState;
        }
    }

    public class BaseEmotionHandler : EmotionHandler
    {
        public override void OnAllyDefeated()
        {
            m_current += new AiEmotion(0f, 0.3f);
        }

        public override void OnTakeDamage()
        {
            m_current += new AiEmotion(0.2f, 0.1f);
        }

        public override void OnThreatDetected() 
        {
            m_current += new AiEmotion(0f, 0.1f);
        }

        public override void OnHelped()
        {
            m_current -= new AiEmotion(1f, 0.5f);
        }
    }

}
