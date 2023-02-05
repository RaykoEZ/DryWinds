using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Curry.Util;
namespace Curry.Explore
{

    // A collection of all cards to be loaded
    // Also contains all decks  
    [Serializable]
    [CreateAssetMenu(fileName = "CardDatabase", menuName = "Curry/Card/Database", order = 1)]
    public class CardDatabase : ScriptableObject 
    {
        // Contains all card prefab refs for loading
        [SerializeField] protected List<AssetReference> m_cardAssets = default;
        // include all decks to load
        [SerializeField] List<Deck> m_decks = default;
        // includes all advent cards to load (listed from each advent collection)
        // These are for advent card prefab instantiations, 
        Dictionary<int, AdventCard> m_advents = new Dictionary<int, AdventCard>();
        Dictionary<string, Deck> m_deckDictionary = new Dictionary<string, Deck>();
        CardLoader m_cardLoader;
        Action onLoadFinish;
        public IReadOnlyDictionary<int, AdventCard> AdventList 
        { get { return m_advents; } }
        public IReadOnlyDictionary<string, Deck> AdventDecks 
        { get { return m_deckDictionary; } }
        public void Init(Action onFinishLoading = null)
        {
            onLoadFinish = onFinishLoading;
            // Load all cards
            m_cardLoader = new CardLoader(m_cardAssets, OnLoaded);
            m_cardLoader.LoadAssets();
            // Put all decks into a dictionary
            foreach(Deck deck in m_decks) 
            {
                m_deckDictionary.Add(deck.DeckId, deck);
            }
        }

        // Randomly draw card from a deck
        public static List<AdventCard> DrawCards(Deck deck, int numToDraw = 1) 
        {
            return deck.GetRandom(numToDraw);
        }
        // Draw a specific card
        public bool TryGetCard(int adventId, out AdventCard card) 
        {
            bool cardExists = m_advents.TryGetValue(adventId, out AdventCard ret);
            card = ret;
            return cardExists;
        } 

        protected virtual void OnLoaded(ICollection<GameObject> objs)
        {
            foreach(GameObject obj in objs) 
            {
                if (obj.TryGetComponent(out AdventCard deploy))
                {
                    m_advents.Add(deploy.Id, deploy);
                }
            }
            // when all asset loaded, init all collections
            InitCollections();
            onLoadFinish?.Invoke();
        }

        // Initialize all advent collections' assets
        protected void InitCollections() 
        {
            foreach (Deck c in m_decks)
            {
                c.Init(AdventList);
            }
        }
    }

}
