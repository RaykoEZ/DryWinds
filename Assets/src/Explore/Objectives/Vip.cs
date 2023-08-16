using UnityEngine;
using Curry.Events;

namespace Curry.Explore
{
    public interface IRescue
    {
        // When player interacts with vip, do this
        void Rescue();
    }
    public class Vip : MonoBehaviour, IRescue, IStepOnTrigger
    {
        [SerializeField] RescueObjective m_objective = default;
        // Happens when player finds clue/comms locations
        public virtual void Rescue() 
        {
            m_objective.OnRescue();
        }
        public void Trigger(ICharacter overlapping)
        {
            if(overlapping is IPlayer) 
            {
                Rescue();
            }
        }
        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out ICharacter character))
            {
                Trigger(character);
            }
        }
    }
}
