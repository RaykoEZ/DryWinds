using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] ResourceBar m_spBar = default;
        [SerializeField] ResourceBar m_stamBar = default;
        PlayerContext m_playerContext = default;

        public void Init(PlayerContextFactory contextFactory)
        {
            contextFactory.Listen(OnPlayerStatUpdate);
        }

        public void Shutdown(PlayerContextFactory contextFactory) 
        {
            contextFactory.Unlisten(OnPlayerStatUpdate);
        }

        void OnPlayerStatUpdate(PlayerContext c) 
        {
            m_playerContext = c;
            UpdateUI();
        }

        void UpdateUI() 
        {
            m_stamBar.SetMaxValue(m_playerContext.CharacterStats.MaxStamina);
            m_stamBar.SetBarValue(m_playerContext.CharacterStats.Stamina);

            m_spBar.SetMaxValue(m_playerContext.CharacterStats.MaxSP);
            m_spBar.SetBarValue(m_playerContext.CharacterStats.SP);
        }
    }
}
