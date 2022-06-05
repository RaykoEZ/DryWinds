using UnityEngine;
using Curry.UI;
using Curry.Events;

namespace Curry.Game
{
    public class PlayerStateManager : MonoBehaviour
    {
        [SerializeField] GameEventManager m_gameEventListener = default;
        
        // Start is called before the first frame update
        void Awake()
        {
            m_gameEventListener.OnPlayerKnockout += OnPlayerKnockout;
            m_gameEventListener.OnInteract += OnPlayerInteract;
            m_gameEventListener.OnInteractNPC += OnPlayerInteractNPC;
            m_gameEventListener.OnItemObtained += OnItemObtained;
            m_gameEventListener.OnFloraObtained += OnFloraObtained;
        }

        void OnDisable()
        {
            m_gameEventListener.OnPlayerKnockout -= OnPlayerKnockout;
            m_gameEventListener.OnInteract -= OnPlayerInteract;
            m_gameEventListener.OnInteractNPC -= OnPlayerInteractNPC;
            m_gameEventListener.OnItemObtained -= OnItemObtained;
            m_gameEventListener.OnFloraObtained -= OnFloraObtained;
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
        }
    }
}
