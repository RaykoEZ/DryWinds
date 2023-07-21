using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Curry.Explore
{
    [SerializeField]
    public abstract class Crisis 
    {
        [SerializeField] string m_name = default;
        [SerializeField] int m_timeUntilTrigger = default;
        public string Name => m_name;
        public int TimeUntilTrigger => m_timeUntilTrigger;
        public abstract IEnumerator Activate(GameStateContext context, Tilemap terrain);
    }
}