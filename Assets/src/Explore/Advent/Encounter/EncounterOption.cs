using Curry.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Curry.Explore
{
    [Serializable]
    public class EncounterOption : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] string m_description = default;
        [SerializeField] Encounter m_encounter = default;
        public event OnChoose OnChosen;
        public event OnChoose OnUnchoose;
        public object Value => m_encounter;
        public string Description => m_description;

        public void OnPointerClick(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
        public virtual bool IsAvailable(GameStateContext conditions)
        {
            return true;
        }
        protected virtual void Preview()
        {
            throw new NotImplementedException();
        }
        protected virtual void Choose()
        {
            throw new NotImplementedException();
        }
        protected virtual void UnChoose()
        {
            throw new NotImplementedException();
        }
    }
}