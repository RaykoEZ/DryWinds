using UnityEngine;
using Curry.UI;

namespace Curry.Game
{
    public class NpcAnimationManager : MonoBehaviour
    {
        [SerializeField] OverlayAnimatorHandler m_uiAnim = default;
        [SerializeField] ResourceBar m_stamBar = default;
        [SerializeField] BaseNpc m_npc = default;

        void Start()
        {
            m_npc.OnTakingDamage += OnNpcStaminaUpdate;
            m_npc.OnHealing += OnNpcStaminaUpdate;

            m_stamBar.SetMaxValue(m_npc.CurrentStats.MaxStamina);
            m_stamBar.SetBarValue(m_npc.CurrentStats.Stamina);
        }

        void OnDestroy()
        {
            m_npc.OnTakingDamage -= OnNpcStaminaUpdate;
        }

        void OnNpcStaminaUpdate(float damage)
        {
            m_uiAnim.OnShow();
            m_stamBar.SetBarValue(m_npc.CurrentStats.Stamina);
            m_uiAnim.SceduleHide();
        }
    }
}
