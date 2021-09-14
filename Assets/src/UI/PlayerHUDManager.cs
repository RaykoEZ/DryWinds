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
        CharacterContext m_playerContext = default;

        public void Init(CharacterContextFactory contextFactory)
        {
            contextFactory.OnUpdate += OnPlayerStatUpdate;
        }

        public void Shutdown(CharacterContextFactory contextFactory) 
        {
            contextFactory.OnUpdate -= OnPlayerStatUpdate;
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
