using UnityEngine.AddressableAssets;

namespace Curry.Util
{
    public interface IAssetLoader<T>
    {
        AssetReference AssetRef { get; }
        delegate void OnAssetLoadSuccess(T asset);
        event OnAssetLoadSuccess OnLoadSuccess;
        void LoadAsset();
    }
}
