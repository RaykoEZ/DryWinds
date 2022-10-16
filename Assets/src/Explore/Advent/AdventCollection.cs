using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Util;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "AdventCollection", menuName = "Curry/AdventCollection", order = 1)]
    public class AdventCollection : ScriptableObject
    {
        [SerializeField] protected string m_deckId = default;
        [SerializeField] protected List<AdventDetail> m_adventLoad = default;
        //
        protected HashSet<AdventCard> m_cards = new HashSet<AdventCard>();

        public string DeckId { get { return m_deckId; } }
        public IReadOnlyList<AdventDetail> AdventDetails { get { return m_adventLoad; } }
        public IReadOnlyCollection<AdventCard> Cards { get { return m_cards; } }

        public virtual void Init(IReadOnlyDictionary<int, AdventCard> adventList) 
        {
            foreach (AdventDetail detail in m_adventLoad) 
            {
                if(adventList.TryGetValue(detail.DeployableLoader.CardId, out AdventCard advent)) 
                {
                    m_cards.Add(advent);
                }
            }
        }

        public List<AdventCard> GetRandom(int numToGet = 1) 
        {
            int rand;
            List<AdventCard> ret = new List<AdventCard>();
            List<AdventCard> cards = new List<AdventCard>(m_cards);
            for(int i = 0; i <  numToGet; ++i) 
            {
                rand = UnityEngine.Random.Range(0, m_cards.Count);
                ret.Add(cards[rand]);
            }
            return ret;
        }
    }

}
