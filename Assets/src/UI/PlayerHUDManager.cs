using UnityEngine;
using Curry.Game;

namespace Curry.UI
{
    public class PlayerHUDManager : MonoBehaviour
    {
        [SerializeField] ResourceBar m_spBar = default;
        [SerializeField] ResourceBar m_stamBar = default;
        [SerializeField] ItemSlotCollection m_heldItems = default;
        CharacterContext m_playerContext = default;

        public void Init(CharacterContextFactory contextFactory, Player player)
        {
            contextFactory.OnUpdate += OnPlayerStatUpdate;
            player.OnCollect += m_heldItems.LoadItemToSlot;

        }

        public void Shutdown(CharacterContextFactory contextFactory, Player player) 
        {
            contextFactory.OnUpdate -= OnPlayerStatUpdate;
            player.OnCollect -= m_heldItems.LoadItemToSlot;
        }

        void OnPlayerStatUpdate(CharacterContext c) 
        {
            m_playerContext = c;
            UpdateUI();
        }

        void UpdateUI() 
        {
            m_stamBar.SetMaxValue(m_playerContext.CharacterStats.MaxStamina);
            m_spBar.SetMaxValue(m_playerContext.CharacterStats.MaxSP);

            m_stamBar.SetBarValue(m_playerContext.CharacterStats.Stamina);
            m_spBar.SetBarValue(m_playerContext.CharacterStats.SP);
        }
    }
}
