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
        protected bool m_selected = false;
        public event OnChoose OnChosen;
        public event OnChoose OnUnchoose;
        public object Value { get; protected set; }

        // Instantiate a card choice object from a bespoke card
        public static CardChoice Create(GameObject cardRef, out GameObject instance)
        {
            if (cardRef == null) 
            {
                instance = null;
                return null;
            }

            // Make a clone of the card
            instance = Instantiate(cardRef);
            // Add a choice script and initialize it with the behaviour
            CardChoice choice = instance.AddComponent<CardChoice>();
            instance.GetComponent<DraggableCard>().enabled = false;
            if (instance.TryGetComponent(out AdventCard card)) 
            {
                choice.InitValue(card);
                //Disable drag events for the this card
            }
            else 
            {
                Debug.LogWarning("Card script not found, using root name for Value ref");
                choice.InitValue(instance.name);
            }
            return choice;
        }

        public override void Prepare()
        {
            Value = null;
            OnChosen = null;
            OnUnchoose = null;
        }
        public virtual void InitValue(object val) 
        {
            Value = val;
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