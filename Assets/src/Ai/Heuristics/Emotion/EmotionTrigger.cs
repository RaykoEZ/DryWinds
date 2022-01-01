namespace Curry.Ai
{
    public abstract class EmotionTrigger<T> where T : AiEmotion 
    {
        public abstract T OnTakeDamage(T current);
        public abstract T OnAllyDefeated(T current);
    }

    public class BaseEmotionTrigger : EmotionTrigger<AiEmotion>
    {
        public override AiEmotion OnAllyDefeated(AiEmotion current)
        {
            throw new System.NotImplementedException();
        }

        public override AiEmotion OnTakeDamage(AiEmotion current)
        {
            throw new System.NotImplementedException();
        }
    }

}
