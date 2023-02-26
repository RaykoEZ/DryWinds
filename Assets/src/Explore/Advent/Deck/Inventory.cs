using Curry.UI;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Curry.Explore
{
    public class CompareCardByName : IComparer<AdventCard>
    {
        public int Compare(AdventCard x, AdventCard y)
        {
            return string.Compare(x.Name, y.Name);
        }
    }
    public class CompareCardByType : IComparer<AdventCard>
    {
        public int Compare(AdventCard x, AdventCard y)
        {
            return x.Type.CompareTo(y.Type);
        }
    }
    public class Inventory : MonoBehaviour
    {
        [SerializeField] PanelUIHandler m_anim = default;
        protected List<AdventCard> m_cardsInStock;
        public IReadOnlyList<AdventCard> CardsInStock => m_cardsInStock;
        CompareCardByName m_sortByCardName;
        CompareCardByType m_sortByCardType;
        void Awake() 
        {
            m_cardsInStock = new List<AdventCard>();
            m_sortByCardName = new CompareCardByName();
            m_sortByCardType = new CompareCardByType();
        }
        public void ToggleDisplay(bool isOn) 
        {
            if (isOn) 
            { 
                Show(); 
            } 
            else 
            { 
                Hide(); 
            }
        }
        public void Show() 
        {
            m_anim.Show();
        }
        public void Hide() 
        {
            m_anim.Hide();
        }
        public AdventCard TakeCard(AdventCard take)
        {
            if (m_cardsInStock.Remove(take))
            {
                return take;
            }
            else
            {
                return null;
            }
        }
        public List<AdventCard> TakeCards(List<AdventCard> take)
        {
            List<AdventCard> ret = new List<AdventCard>();
            foreach (AdventCard toTake in take)
            {
                AdventCard taken = TakeCard(toTake);
                if (taken != null)
                {
                    ret.Add(taken);
                }
            }
            return ret;
        }
        public IReadOnlyList<AdventCard> FilterInventory(Predicate<AdventCard> filter) 
        {
            if (filter == null) return CardsInStock;
            List<AdventCard> ret = m_cardsInStock.FindAll(filter);
            return ret;
        }
        public void AddRange(List<AdventCard> add) 
        {
            foreach(AdventCard card in add) 
            {
                card.transform.SetParent(transform, false);
                card.GetComponent<CardInteractionController>()?.SetInteractionMode(CardInteractMode.Inspect);
            }
            m_cardsInStock?.AddRange(add);
            SortCardByType();
        }
        public void Add(AdventCard add) 
        {
            add.transform.SetParent(transform, false);
            add.GetComponent<CardInteractionController>()?.SetInteractionMode(CardInteractMode.Inspect);
            m_cardsInStock?.Add(add);
            SortCardByType();
        }
        public void SortCardsByName() 
        {
            m_cardsInStock?.Sort(m_sortByCardName);
        }
        public void SortCardByType() 
        {
            m_cardsInStock?.Sort(m_sortByCardType);
        }
    }

}
