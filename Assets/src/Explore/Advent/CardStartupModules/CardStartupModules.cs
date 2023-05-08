using UnityEngine;

namespace Curry.Explore
{
    // base class for all module that needs to be initialized when a card is loading from pool
    public abstract class CardStartupModules : MonoBehaviour 
    {
        public abstract void Init(CardAttribute card);
    }
}
