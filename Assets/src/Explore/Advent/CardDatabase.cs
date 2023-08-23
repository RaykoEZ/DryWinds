using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Util;
namespace Curry.Explore
{
    // A collection of all cards to be loaded
    // Also contains all decks  
    [Serializable]
    [CreateAssetMenu(fileName = "Carddb_", menuName = "Curry/Card/Database", order = 1)]
    public class CardDatabase : ScriptableObject 
    {
        [SerializeField] AssetLoader<CardAsset> m_cardLoader = default;
        // includes all advent cards to load (listed from each advent collection)
        // These are for advent card prefab instantiations, 
        Dictionary<string, CardAsset> m_advents = new Dictionary<string, CardAsset>();
        Action onLoadFinish;
        public IReadOnlyDictionary<string, CardAsset> AdventList 
        { get { return m_advents; } }
        public void LoadAsset(Action onFinishLoading = null)
        {
            onLoadFinish = onFinishLoading;
            // Load all cards
            m_cardLoader.LoadAssets(OnLoaded);
        }
        protected virtual void OnLoaded(ICollection<CardAsset> objs)
        {
            foreach(CardAsset obj in objs) 
            {
                m_advents.Add(obj.name, obj);
            }
            onLoadFinish?.Invoke();
        }
    }

}
