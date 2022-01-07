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
            m_current = new AiEmotion();
        }
        public abstract void OnTakeDamage();
        public abstract void OnAllyDefeated();
        public abstract void OnThreatDetected();
        public abstract void OnHelped();
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
