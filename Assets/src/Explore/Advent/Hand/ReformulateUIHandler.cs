using System.Collections.Generic;
using UnityEngine;
using Curry.Util;
using TMPro;

namespace Curry.Explore
{
    public class ReformulateManager : MonoBehaviour
    {
        [SerializeField] Inventory m_inventory = default;
        [SerializeField] HandManager m_hand = default;
        [SerializeField] ReformulateUIHandler m_ui = default;

    }
    public delegate void OnReformulateFinish(List<AdventCard> handResult, List<AdventCard> inventoryResult);
    public class ReformulateUIHandler : MonoBehaviour 
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] TextMeshProUGUI m_handCapacityValue = default;
        [SerializeField] CardDropZone m_inventoryZone = default;
        [SerializeField] CardDropZone m_handZone = default;
        public void Show(Hand hand, List<AdventCard> inventoryCards) 
        {
            m_anim?.SetBool("show", true);
        }
        public void Hide() 
        {
            m_anim?.SetBool("show", false);
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