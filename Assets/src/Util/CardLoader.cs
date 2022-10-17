using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Curry.Explore;

namespace Curry.Util
{
    [Serializable]
    public class CardLoader : IAssetBatch<GameObject>
    {
        [SerializeField] List<AssetReference> m_assets = default;

        public event IAssetBatch<GameObject>.OnAssetLoadSuccess OnLoadSuccess;
        List<AsyncOperationHandle<GameObject>> m_loadedCardHandles = new List<AsyncOperationHandle<GameObject>>();
        int m_numToLoad;
        public CardLoader(List<AssetReference> assetRefs, IAssetBatch<GameObject>.OnAssetLoadSuccess callback)
        {
            m_numToLoad = assetRefs.Count;
            m_assets = assetRefs;
            OnLoadSuccess += callback;
        }

        public void LoadAssets()
        {
            foreach(AssetReference assetRef in m_assets) 
            {
                assetRef.LoadAssetAsync<GameObject>().Completed += OnLoaded;
            }

        }
        protected void OnLoaded(AsyncOperationHandle<GameObject> obj)
        {
            // store unique card handles
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                m_loadedCardHandles.Add(obj);
                --m_numToLoad;
            }

            if (m_numToLoad == 0) 
            {
                List<GameObject> ret = new List<GameObject>();
                foreach(AsyncOperationHandle<GameObject> handle in m_loadedCardHandles) 
                {
                    ret.Add(handle.Result);
                }
                OnLoadSuccess?.Invoke(ret);
            }
        }
    }
}
