using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.UI
{
    public class PlayerHUDManager : MonoBehaviour
    {
        [SerializeField] ResourceBar m_spBar = default;
        [SerializeField] ResourceBar m_stamBar = default;
        PlayerContext m_playerContext = default;

        public void Init(PlayerContextFactory contextFactory)
        {
            contextFactory.OnUpdate += OnPlayerStatUpdate;
        }

        public void Shutdown(PlayerContextFactory contextFactory) 
        {
            contextFactory.OnUpdate -= OnPlayerStatUpdate;
        }

        void OnPlayerStatUpdate(PlayerContext c) 
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
