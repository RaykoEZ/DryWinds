using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Curry.Explore;

namespace Curry.Util
{
    [Serializable]
    public class AssetLoader<T> : IAssetBatch<T> where T : UnityEngine.Object
    {
        [SerializeField] List<AssetReference> m_assets = default;

        protected event IAssetBatch<T>.OnAssetLoadSuccess OnLoadSuccess;
        List<AsyncOperationHandle<T>> m_loadedCardHandles = new List<AsyncOperationHandle<T>>();
        int m_numToLoad = 0;
        public void LoadAssets(IAssetBatch<T>.OnAssetLoadSuccess callback = null)
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
                    assetRef.LoadAssetAsync<T>().Completed += OnLoaded;
                }
            }
            // If we already loaded these assets, finish now
            if(m_numToLoad == 0) 
            {
                List<T> ret = AddResultToList();
                OnLoadSuccess?.Invoke(ret);
                OnLoadSuccess -= callback;
            }
        }
        protected void OnLoaded(AsyncOperationHandle<T> obj)
        {
            // store unique card handles
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                m_loadedCardHandles.Add(obj);
                --m_numToLoad;
            }

            if (m_numToLoad == 0) 
            {
                List<T> ret = AddResultToList();
                OnLoadSuccess?.Invoke(ret);
                OnLoadSuccess = null;
            }
        }
        List<T> AddResultToList() 
        {
            List<T> ret = new List<T>();
            foreach (AsyncOperationHandle<T> handle in m_loadedCardHandles)
            {
                ret.Add(handle.Result);
            }
            return ret;
        }
    }
}
