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
        [SerializeField] Animator m_anim = default;
        [SerializeField] Encounter m_encounter = default;
        public delegate void OnEncounterChosen(Dialogue dialogue);
        public event OnEncounterChosen OnChosen;
        public string Description => m_description;
        protected bool m_detail = false;
        protected GameStateContext m_context;
        public virtual bool IsOptionAvailable { get; protected set; } = true;
        public virtual void Init(GameStateContext context) 
        {
            m_context = context;
        }
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!m_detail) 
            {
                ShowDetail();
            } 
            else 
            {
                Dialogue result = Choose(m_context);
                OnChosen?.Invoke(result);
            }
        }
        public virtual void OnPointerUp(PointerEventData eventData)
        {
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }
        public virtual bool IsAvailable(GameStateContext conditions)
        {
            return true;
        }
        protected virtual void ShowDetail()
        {
            m_detail = true;
        }
        protected virtual void HideDetail()
        {
            m_detail = false;
        }
        protected virtual Dialogue Choose(GameStateContext context)
        {
            return m_encounter.OnChoose(context);
        }
    }
}