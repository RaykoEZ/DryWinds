using Curry.Explore;
using Curry.Game;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Curry.UI
{
    [Flags]
    [Serializable]
    public enum CardInteractMode 
    {
        Play = 1 << 0,
        Inspect = 1 << 1,
        Select = 1 << 2
    }
    // Allows a card to be dragged/pointer hover/selected/inspected
    public class CardInteractionController : MonoBehaviour, IChoice,
        IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, 
        IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] CardInteractMode m_interactionMode = default;
        public delegate void OnCardInspect(RectTransform cardTranform);
        AdventCard m_card = default;
        UITransitionBuffer m_buffer = new UITransitionBuffer();
        protected bool m_selected = false;
        public event OnChoose OnChosen;
        public event OnChoose OnUnchoose;
        public event OnCardInspect OnInspect;
        public CardInteractMode InteractMode { get { return m_interactionMode; } protected set { m_interactionMode = value; } }
        public object Value { get => m_card; protected set => m_card = value as AdventCard; }
        public bool Choosable { get; set; } = true;
        protected virtual void Start()
        {
            if (m_card == null && TryGetComponent(out AdventCard card)) 
            {
                Init(card);
            }
        }
        // Add an instantiated CardChoice to a instance of card,
        // this does not instantiate a new card gameobject
        public static CardInteractionController AttachToCard(AdventCard cardInstance) 
        {
            if (cardInstance == null)
            {
                return null;
            }
            cardInstance.GetComponent<DraggableCard>().enabled = false;
            CardInteractionController choice = cardInstance.gameObject.AddComponent<CardInteractionController>();
            choice.Init(cardInstance);           
            return choice;
        }
        public virtual void SetInteractionMode(CardInteractMode mode) 
        {
            InteractMode = mode;
            DraggableCard drag = GetComponent<DraggableCard>();
            drag.enabled = (InteractMode & CardInteractMode.Play) != 0;
            drag.Draggable =
                (InteractMode & CardInteractMode.Play) != 0;
        }
        public virtual void Init(AdventCard card, CardInteractMode mode = CardInteractMode.Inspect) 
        {
            m_selected = false;
            Choosable = true;
            m_card = card;
            SetInteractionMode(mode);
        }
        public void DisplayChoice(Transform parent)
        {
            transform.SetParent(parent, false);
            GetComponent<Animator>()?.SetBool("selected", false);
            transform.localPosition = Vector3.zero;
        }
        public void Choose()
        {
            m_selected = true;
            Animator anim = GetComponent<Animator>();
            anim?.SetBool("selected", Choosable);

            OnChosen?.Invoke(this);
        }
        public void UnChoose()
        {
            m_selected = false;
            Animator anim = GetComponent<Animator>();
            anim?.SetBool("selected", false);
            OnUnchoose?.Invoke(this);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            bool notSelectable = (InteractMode & CardInteractMode.Select) == 0;
            if (notSelectable) 
            { 
                return;
            }
            if (m_selected) 
            {
                UnChoose();
            } 
            else if(!m_selected && Choosable)
            {
                Choose();
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
        }
        public void OnPointerDown(PointerEventData eventData)
        {
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (m_buffer.Buffering) return;
            Animator anim = GetComponent<Animator>();
            anim?.SetBool("selected", Choosable);
            if ((InteractMode & CardInteractMode.Inspect) != 0) 
            {
                anim?.SetBool("inspecting", true);
                anim?.SetBool("detail", true);
                OnInspect?.Invoke(GetComponent<RectTransform>());
            }
            StartCoroutine(m_buffer.Buffer());
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            Animator anim = GetComponent<Animator>();
            anim?.SetBool("selected", m_selected);
            anim?.SetBool("inspecting", false);
            anim?.SetBool("detail", false);
        }
    }
}