using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.UI;
using Curry.Events;

namespace Curry.Game
{
    public class PlayerStateManager : MonoBehaviour
    {
        [SerializeField] PlayerHUDManager m_playerUI = default;
        [SerializeField] Player m_player = default;
        [SerializeField] GameEventHandler m_gameEventListener = default;
        CharacterContextFactory m_playerContextFactory = default;

        // Start is called before the first frame update
        void Start()
        {
            m_gameEventListener.OnPlayerKnockout += OnPlayerKnockout;

            CharacterContextFactory playerContextFactory = new CharacterContextFactory();
            m_playerContextFactory = playerContextFactory;
            m_playerUI.Init(playerContextFactory, m_player);
            m_player.Init(playerContextFactory);
        }

        void OnDisable()
        {
            m_gameEventListener.OnPlayerKnockout -= OnPlayerKnockout;
            m_playerUI.Shutdown(m_playerContextFactory, m_player);
            m_player.Shutdown();
        }

        void OnPlayerKnockout(object sender, PlayerEventArgs args) 
        {
            // Handle Lose calls
            Debug.Log("Game Over.");
            Application.Quit();
        }

    }
}
