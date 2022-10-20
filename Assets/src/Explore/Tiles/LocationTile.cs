using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "Location_", menuName = "Curry/Tiles/Location", order = 1)]
    public class LocationTile: WorldTile 
    {
        [SerializeField] protected List<AdventCard> m_events = default;
        // event cardids to give to player upon reaching this tile 
        public IReadOnlyList<AdventCard> Events { get { return m_events; } }
        public void AddEvent(AdventCard newEvent) 
        {
            if (newEvent == null) return;

            m_events.Add(newEvent);
        }
    }
}