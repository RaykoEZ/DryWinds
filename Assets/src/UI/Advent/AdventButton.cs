using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Curry.Game;

namespace Curry.Explore 
{
    public class AdventButton : MonoBehaviour
    {
        [SerializeField] GameObject m_player = default;
        [SerializeField] AdventManager m_adventManager = default;
        [SerializeField] AdventHand m_hand = default;
        public void Adventure()
        {
            // Fetch advent deck from occupied tile
            Vector2 pos = m_player.transform.position;
            IReadOnlyDictionary<int, AdventCard> deck;
            bool deckExists = 
                m_adventManager.TryGetAdventInCollection(pos, out deck);
            if (deckExists && deck?.Count > 0) 
            {
                List<int> cardIds = new List<int>(deck.Keys);
                // Draw advent card from fetched deck
                List<AdventCard> cardsDrawn = DrawFromDeck(cardIds, 5);
                m_hand.OnCardDraw(cardsDrawn);
            }
            else 
            {
                Debug.Log("Nothing to see here");
            }
        }

        List<AdventCard> DrawFromDeck(List<int> cardIds, int numToDraw = 1) 
        {
            // Draw random cards from a list of cards
            List<int> cardIdsToDraw = new List<int>();
            int rand;
            for(int i = 0; i < numToDraw; ++i) 
            {
                rand = Random.Range(0, cardIds.Count - 1);
                cardIdsToDraw.Add(cardIds[rand]);
            }
            // Get the instance for each of the cards drawn
            List<AdventCard> ret = new List<AdventCard>();
            foreach (int id in cardIdsToDraw) 
            {
                AdventCard cardInstance = m_adventManager.GetAdventInstance(id);
                ret.Add(cardInstance);
            }
            return ret;
        }
    }
}


