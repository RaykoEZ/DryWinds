using UnityEngine;

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

        public virtual void Update() 
        {
            switch (Current.EmotionState) 
            {
                case AiEmotionState.Normal:
                {
                    float newHate = Mathf.Lerp(m_current.Hatred, 0f, 0.05f);
                    float newFear = Mathf.Lerp(m_current.Fear, 0f, 0.05f);
                    m_current = new AiEmotion(newHate, newFear);
                    break;
                }
                case AiEmotionState.Hatred:
                {
                    float newHate = Mathf.Max(0f, m_current.Hatred - 0.05f);
                    float newFear = Mathf.Lerp(m_current.Fear, 0f, 0.01f);
                    m_current = new AiEmotion(newHate, newFear);
                    break;
                }
                case AiEmotionState.Fear:
                {
                    float newHate = Mathf.Lerp(m_current.Hatred, 0f, 0.01f);
                    float newFear = Mathf.Max(0f, m_current.Fear - 0.05f);
                    m_current = new AiEmotion(newHate, newFear);
                    break; 
                }
            }    
        }
    }

    public class BaseEmotionHandler : EmotionHandler
    {
        public override void OnAllyDefeated()
        {
            m_current += new AiEmotion(0f, 0.5f);
        }

        public override void OnTakeDamage()
        {
            m_current += new AiEmotion(0.5f, 1f);
        }

        public override void OnThreatDetected() 
        {
            m_current += new AiEmotion(0f, 0.5f);
        }

        public override void OnHelped()
        {
            m_current -= new AiEmotion(1f, 0.5f);
        }
    }

}
