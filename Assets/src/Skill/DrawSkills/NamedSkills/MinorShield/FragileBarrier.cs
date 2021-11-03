using Curry.Game;

namespace Curry.Skill
{
    public interface ISkillSpawn 
    {
        void OnSpawn(IActionInput param);
    }

    public class FragileBarrier : FragileObject, ITimeLimit, ISkillSpawn
    {
        public float Duration { get; }
        public float TimeElapsed { get; }

        public void OnSpawn(IActionInput param) 
        { 
            
        }

        public virtual void OnExpire() 
        {
            
        }

        public override void OnDefeat()
        {
            OnExpire();
            base.OnDefeat();
        }
    }
}
