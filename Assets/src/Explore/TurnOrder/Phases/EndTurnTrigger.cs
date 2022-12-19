using UnityEngine;
using UnityEngine.UI;
using Curry.Events;
namespace Curry.Explore
{
    public delegate void OnPlayerTurnEnd();
    public class EndTurnTrigger : MonoBehaviour 
    {
        [SerializeField] Button m_button = default;
        public event OnPlayerTurnEnd OnTurnEnd;

        public void SetInteractable (bool val)
        {
            m_button.interactable = val;
        }
        public void EndTurn() 
        {
            OnTurnEnd?.Invoke();
        }
    }
}