﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "AdventDatabase", menuName = "Curry/AdventDatatbase", order = 1)]
    public class AdventDatabase : ScriptableObject 
    {
        // include all decks to load
        [SerializeField] List<AdventCollection> m_collections = default;
        // includes all advent cards to load (listed from each advent collection)
        Dictionary<int, AdventCard> m_advents = new Dictionary<int, AdventCard>();
        public IReadOnlyDictionary<int, AdventCard> AdventList { get { return m_advents; } }
        int m_numToLoad = 0;
        int m_numLoaded = 0;
        void Awake()
        {
            // Get unique advent catalog
            HashSet<AdventDetail> loadSet = new HashSet<AdventDetail>();
            foreach (AdventCollection deck in m_collections)
            {
                loadSet.UnionWith(deck.AdventDetails);
            }
            m_numToLoad = loadSet.Count;
            // load all advent references
            foreach (AdventDetail detail in loadSet) 
            {
                detail.DeployableLoader.OnLoadSuccess += OnLoaded;
                detail.DeployableLoader.LoadAsset();
            }
        }

        protected virtual void OnLoaded(GameObject obj)
        {
            if (obj.TryGetComponent(out AdventCard deploy))
            {
                m_advents.Add(deploy.Id, deploy);
                ++m_numLoaded;
            }
            // when all asset loaded, init all collections
            if (m_numLoaded == m_numToLoad) 
            {
                InitCollections();
            }
        }

        protected void InitCollections() 
        {
            foreach (AdventCollection c in m_collections)
            {
                c.Init(AdventList);
            }
            m_numToLoad = 0;
            m_numLoaded = 0;
        }
    }

}
