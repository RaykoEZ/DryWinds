using UnityEngine;
using UnityEngine.UI;
using Curry.Events;
namespace Curry.Explore
{
    public delegate void OnPlayerTurnEnd();
    public class EndTurnTrigger : MonoBehaviour 
    {
        [SerializeField] TimeManager m_time = default;
        public event OnPlayerTurnEnd OnTurnEnd;
        public void EndTurn() 
        {
            m_time.TrySpendTime(5);
            OnTurnEnd?.Invoke();
        }
    }
}