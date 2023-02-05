using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Curry.Events;

namespace Curry.Explore
{
    // Contained database for all available advent(cards and decks)
    // Quiries Tile info for a tile in its tilemap coordinate
    public class DeckManager : MonoBehaviour
    {
        [SerializeField] protected CardDatabase m_adventDb = default;
        [SerializeField] AdventInstanceManager m_instance = default;
        // After player moved, we draw card from player position, trigger this
        [SerializeField] CurryGameEventTrigger m_onCardDraw = default;      

        void Awake()
        {
            m_adventDb.Init(OnAdventLoadFinish);
        }

        // Instantiate cards and trigger game events OnCardDraw
        public void DrawCards(IReadOnlyList<AdventCard> cardsToDraw)
        {
            List<AdventCard> cardInstances = new List<AdventCard>();
            foreach (AdventCard cardRef in cardsToDraw)
            {
                // Instantiating cards to be drawn
                AdventCard cardInstance = InstantiateCard(cardRef);
                cardInstances.Add(cardInstance);
            }

            CardDrawInfo info = new CardDrawInfo().
                SetCardDraw(cardInstances);

            m_onCardDraw?.TriggerEvent(info);
        }

        AdventCard InstantiateCard(AdventCard cardRef)
        {
            AdventCard ret;
            ret = m_instance.GetInstanceFromAsset(cardRef.gameObject);
            return ret;
        }

        void OnAdventLoadFinish()
        {
            foreach (KeyValuePair<int, AdventCard> advent in m_adventDb.AdventList)
            {
                m_instance.PrepareNewInstance(advent.Value.gameObject);
            }
        }
    }

}
