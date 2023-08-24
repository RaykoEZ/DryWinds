using UnityEngine;
using Curry.Explore;

namespace Curry.UI
{
    // For showing and hiding ui overlays for player to view board state when using an overlay
    public class OverlayViewToggle : MonoBehaviour
    {
        [SerializeField] PanelUIHandler m_panelAnim = default;
        public void OnToggle(bool isOn) 
        {
            if (isOn) 
            {
                m_panelAnim.Hide();
            } 
            else 
            {
                m_panelAnim.Show();
            }
        }
    }
}