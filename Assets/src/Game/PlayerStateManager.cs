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
        [SerializeField] ObjectiveManager m_objectiveManager = default;
        CharacterContextFactory m_playerContextFactory = default;

        // Start is called before the first frame update
        void Awake()
        {
            m_gameEventListener.OnPlayerKnockout += OnPlayerKnockout;
            m_gameEventListener.OnInteract += OnPlayerInteract;
            m_gameEventListener.OnInteractNPC += OnPlayerInteractNPC;
            m_gameEventListener.OnItemObtained += OnItemObtained;
            m_gameEventListener.OnFloraObtained += OnFloraObtained;

            CharacterContextFactory playerContextFactory = new CharacterContextFactory();
            m_playerContextFactory = playerContextFactory;
            m_playerUI.Init(playerContextFactory, m_player);
            m_player.Init(playerContextFactory);
        }

        void OnDisable()
        {
            m_gameEventListener.OnPlayerKnockout -= OnPlayerKnockout;
            m_gameEventListener.OnInteract -= OnPlayerInteract;
            m_gameEventListener.OnInteractNPC -= OnPlayerInteractNPC;
            m_gameEventListener.OnItemObtained -= OnItemObtained;
            m_gameEventListener.OnFloraObtained -= OnFloraObtained;

            m_playerUI.Shutdown(m_playerContextFactory, m_player);
            m_player.Shutdown();
        }
        void OnPlayerKnockout(object sender, PlayerArgs args)
        {
            // Handle Lose calls
            Debug.Log("Game Over.");
            Time.timeScale = 0f;
        }

        void OnPlayerInteract(object sender, InteractableArgs args) 
        { 
        
        }

        void OnPlayerInteractNPC(object sender, NPCArgs args)
        {

        }

        void OnItemObtained(object sender, ItemArgs args) 
        {
        
        }

        void OnFloraObtained(object sender, FloraArgs args) 
        {
            Debug.Log("collected: " + args.Flora.Property.Name);
            m_objectiveManager.OnObjectiveAchieve();
        }
    }
}
