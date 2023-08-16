using UnityEngine;
using System.Collections;
using Curry.UI;

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