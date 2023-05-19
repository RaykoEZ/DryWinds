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
    [CreateAssetMenu(fileName = "Carddb_", menuName = "Curry/Card/Database", order = 1)]
    public class CardDatabase : ScriptableObject 
    {
        [SerializeField] AssetLoader m_cardLoader = default;
        // includes all advent cards to load (listed from each advent collection)
        // These are for advent card prefab instantiations, 
        Dictionary<int, AdventCard> m_advents = new Dictionary<int, AdventCard>();
        Action onLoadFinish;
        public IReadOnlyDictionary<int, AdventCard> AdventList 
        { get { return m_advents; } }
        public void LoadAsset(Action onFinishLoading = null)
        {
            onLoadFinish = onFinishLoading;
            // Load all cards
            m_cardLoader.LoadAssets(OnLoaded);
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
            onLoadFinish?.Invoke();
        }
    }

}
