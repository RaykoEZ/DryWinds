using UnityEngine;
using Curry.UI;
using Curry.Events;

namespace Curry.Game
{
    public class PlayerStateManager : MonoBehaviour
    {        
        // Start is called before the first frame update
        void Awake()
        {
        }

        void OnDisable()
        {
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
