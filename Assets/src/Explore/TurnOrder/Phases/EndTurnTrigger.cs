using UnityEngine;
using System.Collections;
using Curry.UI;

namespace Curry.Explore
{
    public delegate void OnPlayerTurnEnd();
    public class EndTurnTrigger : SceneInterruptBehaviour 
    {
        public event OnPlayerTurnEnd OnTurnEnd;
        public void EndTurn() 
        {
            StartInterrupt();
            OnTurnEnd?.Invoke();
        }
    }
}