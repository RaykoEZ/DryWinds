using Curry.Explore;
using Curry.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Curry.UI
{
    public class CardChoice : MonoBehaviour, IChoice, 
        IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, 
        IPointerEnterHandler, IPointerExitHandler
    {
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
        public static void DetachFromCard(CardChoice choiceRef) 
        {
            if (choiceRef == null) 
            {
                return;
            }
            choiceRef.gameObject.GetComponent<DraggableCard>().enabled = true;
            Destroy(choiceRef);
        }
        protected virtual void InitValue(object val)
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
            GetComponent<Animator>()?.SetBool("selected", true);
            OnChosen?.Invoke(this);
        }
        public void UnChoose()
        {
            GetComponent<Animator>()?.SetBool("selected", false);
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
            GetComponent<Animator>()?.SetBool("highlight", true);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            GetComponent<Animator>()?.SetBool("highlight", false);
        }

    }
}