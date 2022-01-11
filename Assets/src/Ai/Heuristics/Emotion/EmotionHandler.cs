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
                        if (m_current.Hatred > 0.2f || m_current.Hatred < -0.2f) 
                        {
                            float newHate = Mathf.Lerp(m_current.Hatred, 0f, Time.deltaTime);
                            m_current = new AiEmotion(newHate, m_current.Fear);
                        }

                        if (m_current.Fear > 0.2f || m_current.Fear < -0.2f)
                        {
                            float newFear = Mathf.Lerp(m_current.Fear, 0f, Time.deltaTime);
                            m_current = new AiEmotion(m_current.Hatred, newFear);
                        }
                        break;
                    }
                case AiEmotionState.Hatred:
                {
                        float newHate = Mathf.Max(0f, m_current.Hatred - Time.deltaTime);
                        m_current = new AiEmotion(newHate, m_current.Fear);
                        break;
                }
                case AiEmotionState.Fear:
                {
                        float newFear = Mathf.Max(0f, m_current.Fear - Time.deltaTime);
                        m_current = new AiEmotion(m_current.Hatred, newFear);
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
