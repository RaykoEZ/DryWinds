using UnityEngine;
using UnityEngine.UI;
using Curry.Events;
namespace Curry.Explore
{
    public delegate void OnPlayerTurnEnd();
    public class EndTurnTrigger : SceneInterruptBehaviour 
    {
        [SerializeField] TimeManager m_time = default;
        public event OnPlayerTurnEnd OnTurnEnd;
        public void EndTurn() 
        {
            StartInterrupt();
            m_time.TrySpendTime(5);
            OnTurnEnd?.Invoke();
        }
    }
}