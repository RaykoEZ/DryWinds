using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Curry.Util
{
    public interface IAssetLoader<T> where T : Object
    {
        delegate void OnAssetLoadSuccess(T asset);
        event OnAssetLoadSuccess OnLoadSuccess;
        void LoadAsset();
    }
    public interface IAssetBatch<T> where T : Object 
    {
        delegate void OnAssetLoadSuccess(ICollection<T> asset);
        void LoadAssets(OnAssetLoadSuccess callback = null);
    }
}
