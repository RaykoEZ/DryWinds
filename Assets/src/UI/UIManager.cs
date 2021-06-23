using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] ResourceBar m_spBar = default;
        [SerializeField] ResourceBar m_hpBar = default;
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
            m_hpBar.SetMaxValue(m_playerContext.BaseStats.HP);
            m_hpBar.SetBarValue(m_playerContext.CurrentStats.HP);

            m_spBar.SetMaxValue(m_playerContext.BaseStats.SP);
            m_spBar.SetBarValue(m_playerContext.CurrentStats.SP);
        }
    }
}
