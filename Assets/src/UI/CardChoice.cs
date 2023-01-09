using Curry.Explore;
using Curry.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Curry.UI
{
    public class CardChoice : PoolableBehaviour, IChoice, 
        IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, 
        IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] Image m_cardImage = default;
        [SerializeField] Image m_cardBorder = default;
        protected bool m_selected = false;
        public event OnChoose OnChosen;
        public event OnChoose OnUnchoose;
        public object Value { get; protected set; }

        public override void Prepare()
        {
            Value = null;
            OnChosen = null;
            OnUnchoose = null;
        }
        public virtual void Init(AdventCard card) 
        {
            Value = card;
        }
        public void DisplayChoice(Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
        }
        public void Choose()
        {
            OnChosen?.Invoke(this);
        }
        public void UnChoose()
        {
            OnUnchoose?.Invoke(this);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_selected) 
            {
                UnChoose();
            } 
            else 
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
            m_anim.SetBool("highlight", true);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            m_anim.SetBool("highlight", false);
        }
    }
}