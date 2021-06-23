using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.UI;

namespace Curry.Game
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] UIManager m_playerUI = default;
        [SerializeField] Player m_player = default;
        PlayerContextFactory m_playerContextFactory = default;

        // Start is called before the first frame update
        void Start()
        {
            PlayerContextFactory playerContextFactory = new PlayerContextFactory();
            m_playerContextFactory = playerContextFactory;
            m_playerUI.Init(playerContextFactory);
            m_player.Init(playerContextFactory);
        }

        void OnDisable()
        {
            m_playerUI.Shutdown(m_playerContextFactory);
            m_player.Shutdown();
        }
    }
}
