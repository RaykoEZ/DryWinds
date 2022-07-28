using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Curry.Game;

namespace Curry.Explore 
{
    public class AdventButton : MonoBehaviour
    {
        [SerializeField] Player m_player = default;
        [SerializeField] AdventManager m_adventManager = default;
        public void Adventure()
        {
            // Fetch advent deck from occupied tile
            Vector2 pos = m_player.transform.position;
            IReadOnlyDictionary<int, AdventCard> deck;
            bool deckExists = 
                m_adventManager.TryGetAdventInCollection(pos, out deck);
            if (deckExists && deck?.Count > 0) 
            {
                // Draw advent card from fetched deck
            }
            else 
            {
                Debug.Log("Nothing to see here");
            }
        }
    }
}


