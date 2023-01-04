using UnityEngine;
using Curry.Events;

namespace Curry.Explore
{
    public interface IRescue
    {
        // When player interacts with vip, do this
        void Rescue();
    }
    public class Vip : MonoBehaviour, IRescue
    {
        [SerializeField] RescueObjective m_objective = default;
        // Happens when player finds clue/comms locations
        public virtual void Rescue() 
        {
            Debug.Log("tyty");
            m_objective.OnRescue();
        }
        public virtual void Capture() 
        {
            Debug.Log("oof");
            m_objective.OnFailure();
        }
    }
}
