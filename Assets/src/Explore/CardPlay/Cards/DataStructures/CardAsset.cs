using UnityEngine;
namespace Curry.Explore
{
    public abstract class CardAsset : ScriptableObject 
    {
        public abstract CardResource GetResource();
    }
}
