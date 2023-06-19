using UnityEngine;
using UnityEngine.UI;
using Curry.Events;
namespace Curry.Explore
{
    public delegate void OnPlayerTurnEnd();
    public class EndTurnTrigger : MonoBehaviour 
    {
        public event OnPlayerTurnEnd OnTurnEnd;
        public void EndTurn() 
        {
            OnTurnEnd?.Invoke();
        }
    }
}