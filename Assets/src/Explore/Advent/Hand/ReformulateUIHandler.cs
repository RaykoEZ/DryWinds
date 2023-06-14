using System.Collections.Generic;
using UnityEngine;
using Curry.Util;
using TMPro;
using System;

namespace Curry.Explore
{
    public class ReformulateManager : MonoBehaviour
    {
        [SerializeField] Inventory m_inventory = default;
        [SerializeField] HandManager m_hand = default;
    }
    public delegate void OnReformulateFinish(List<AdventCard> handResult, List<AdventCard> inventoryResult);
    public class ReformulateUIHandler : MonoBehaviour 
    {
        [SerializeField] TextMeshProUGUI m_handMaxCapacity = default;
        [SerializeField] TextMeshProUGUI m_handHoldingValue = default;

        [SerializeField] CardDropZone m_invcntoryZone = default;
        [SerializeField] CardDropZone m_handZone = default;

        public void Show(HandManager.Hand hand, List<AdventCard> inventoryCards) 
        { 
        
        }
        public void Hide() 
        { 
        
        }
        public void DropToInventory(AdventCard drop) 
        {
        
        }
        public void DropToHand(AdventCard drop) 
        {
        
        }
        void ReturnToOrigin(AdventCard drop) 
        { 
        
        }
    }
}