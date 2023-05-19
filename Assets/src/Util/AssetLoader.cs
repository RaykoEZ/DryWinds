using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Curry.Explore;

namespace Curry.Util
{
    [Serializable]
    public class AssetLoader : IAssetBatch<GameObject>
    {
        [SerializeField] List<AssetReference> m_assets = default;

        protected event IAssetBatch<GameObject>.OnAssetLoadSuccess OnLoadSuccess;
        List<AsyncOperationHandle<GameObject>> m_loadedCardHandles = new List<AsyncOperationHandle<GameObject>>();
        int m_numToLoad = 0;
        public void LoadAssets(IAssetBatch<GameObject>.OnAssetLoadSuccess callback = null)
        {
            m_numToLoad = m_assets.Count;
            OnLoadSuccess += callback;
            foreach (AssetReference assetRef in m_assets) 
            {
                if (assetRef.IsDone) 
                {
                    --m_numToLoad;
                }
                else 
                {
                    assetRef.LoadAssetAsync<GameObject>().Completed += OnLoaded;
                }
            }
            // If we already loaded these assets, finish now
            if(m_numToLoad == 0) 
            {
                List<GameObject> ret = AddResultToList();
                OnLoadSuccess?.Invoke(ret);
                OnLoadSuccess -= callback;
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
                List<GameObject> ret = AddResultToList();
                OnLoadSuccess?.Invoke(ret);
                OnLoadSuccess = null;
            }
        }
        List<GameObject> AddResultToList() 
        {
            List<GameObject> ret = new List<GameObject>();
            foreach (AsyncOperationHandle<GameObject> handle in m_loadedCardHandles)
            {
                ret.Add(handle.Result);
            }
            return ret;
        }
    }
}
