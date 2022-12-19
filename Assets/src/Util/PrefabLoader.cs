using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
namespace Curry.Util
{

    [Serializable]
    public class PrefabLoader : IAssetLoader<GameObject>
    {
        [SerializeField] protected AssetReference m_assetRef = default;

        public virtual event IAssetLoader<GameObject>.OnAssetLoadSuccess OnLoadSuccess;
        public AssetReference AssetRef { get { return m_assetRef; }}

        public virtual void LoadAsset() 
        {
            Addressables.LoadAssetAsync<GameObject>(AssetRef).Completed += OnLoaded;       
        }

        public PrefabLoader(AssetReference assetRef, IAssetLoader<GameObject>.OnAssetLoadSuccess callback) 
        {
            m_assetRef = assetRef;
            OnLoadSuccess += callback;
        }

        protected virtual void OnLoaded(AsyncOperationHandle<GameObject> obj) 
        { 
            if (obj.Status == AsyncOperationStatus.Succeeded) 
            {
                OnLoadSuccess?.Invoke(obj.Result);
            }
        }
    }
}
