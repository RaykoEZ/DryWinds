using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Curry.Explore;
namespace Curry.Util
{

    [Serializable]
    public class PrefabLoader : IAssetLoader<GameObject>
    {
        [SerializeField] protected AssetReference m_assetRef = default;

        public virtual event IAssetLoader<GameObject>.OnAssetLoadSuccess OnLoadSuccess;
        public AssetReference AssetRef { get { return m_assetRef; }}

        public void LoadAsset() 
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

    [Serializable]
    public class CardLoader : PrefabLoader
    {
        public int CardId { get; protected set; }
        public override event IAssetLoader<GameObject>.OnAssetLoadSuccess OnLoadSuccess;

        public CardLoader(AssetReference assetRef, IAssetLoader<GameObject>.OnAssetLoadSuccess callback) : base(assetRef, callback)
        {
        }

        protected override void OnLoaded(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded && obj.Result.TryGetComponent(out AdventCard card))
            {
                CardId = card.Id;
                OnLoadSuccess?.Invoke(obj.Result);
            }
        }

    }
}
