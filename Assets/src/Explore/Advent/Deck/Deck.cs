using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Deck_", menuName = "Curry/AdventDeck", order = 1)]
    public class Deck : ScriptableObject
    {
        [SerializeField] protected string m_deckId = default;
        [SerializeField] protected List<AdventCard> m_cardsInDeck = default;
        //
        protected HashSet<AdventCard> m_cardSet = new HashSet<AdventCard>();

        public string DeckId { get { return m_deckId; } }
        public IReadOnlyCollection<AdventCard> Cards { get { return m_cardSet; } }

        public virtual void Init(IReadOnlyDictionary<int, AdventCard> adventList) 
        {
            foreach (AdventCard card in m_cardsInDeck) 
            {
                if(adventList.TryGetValue(card.Id, out AdventCard advent)) 
                {
                    m_cardSet.Add(advent);
                }
            }
        }

        public List<AdventCard> GetRandom(int numToGet = 1) 
        {
            int rand;
            List<AdventCard> ret = new List<AdventCard>();
            List<AdventCard> cards = new List<AdventCard>(m_cardSet);
            for(int i = 0; i <  numToGet; ++i) 
            {
                rand = UnityEngine.Random.Range(0, m_cardSet.Count);
                ret.Add(cards[rand]);
            }
            return ret;
        }
    }

}
