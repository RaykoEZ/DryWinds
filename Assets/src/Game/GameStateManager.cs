using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.UI;
using Curry.Events;

namespace Curry.Game
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] PlayerHUDManager m_playerUI = default;
        [SerializeField] Player m_player = default;
        [SerializeField] GameEventHandler m_gameEventListener = default;
        [SerializeField] Camera m_mainCamera = default;

        PlayerContextFactory m_playerContextFactory = default;

        // Start is called before the first frame update
        void Start()
        {
            m_gameEventListener.OnPlayerKnockout += OnPlayerKnockout;

            PlayerContextFactory playerContextFactory = new PlayerContextFactory();
            m_playerContextFactory = playerContextFactory;
            m_playerUI.Init(playerContextFactory);
            m_player.Init(playerContextFactory, m_mainCamera);

            // Setup Iframe collision ignore
            int iframeLayer = LayerMask.NameToLayer("IFrame");
            Physics.IgnoreLayerCollision(0, iframeLayer, true);
        }

        void OnDisable()
        {
            m_gameEventListener.OnPlayerKnockout -= OnPlayerKnockout;
            m_playerUI.Shutdown(m_playerContextFactory);
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