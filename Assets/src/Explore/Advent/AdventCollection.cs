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
        [SerializeField] protected int m_id = default;
        [SerializeField] protected List<AdventDetail> m_adventLoad = default;
        //
        protected List<AdventCard> m_advents = new List<AdventCard>();

        public int Id { get { return m_id; } }
        public List<AdventDetail> AdventDetails { get { return m_adventLoad; } }
        public IReadOnlyList<AdventCard> AdventDictionary { get { return m_advents; } }

        public virtual void Init(IReadOnlyDictionary<int, AdventCard> adventList) 
        {
            foreach(AdventDetail detail in m_adventLoad) 
            {
                if(adventList.TryGetValue(detail.Id, out AdventCard advent)) 
                {
                    m_advents.Add(advent);
                }
            }
        }

        public List<AdventCard> GetRandom(int numToGet = 1) 
        {
            int rand;
            List<AdventCard> ret = new List<AdventCard>();
            for(int i = 0; i <  numToGet; ++i) 
            {
                rand = UnityEngine.Random.Range(0, m_advents.Count - 1);
                ret.Add(AdventDictionary[rand]);
            }
            return ret;
        }
    }

}
