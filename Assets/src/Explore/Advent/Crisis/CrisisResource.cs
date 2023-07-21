using UnityEngine;

namespace Curry.Explore
{
    public abstract class CrisisResource : ScriptableObject
    {
        public abstract Crisis GetContent();
    }
}