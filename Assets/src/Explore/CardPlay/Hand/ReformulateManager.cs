using UnityEngine;

namespace Curry.Explore
{
    public class ReformulateManager : MonoBehaviour
    {
        [SerializeField] Inventory m_inventory = default;
        [SerializeField] HandManager m_hand = default;
        [SerializeField] ReformulateUIHandler m_ui = default;
        public void BeginDisplay() 
        {
            if (m_ui.IsDisplaying) return;

            m_ui?.Show(m_hand, m_inventory);
        }
    }
}