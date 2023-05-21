using Curry.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "Loc_", menuName = "Curry/Tiles/Location", order = 1)]
    public class LocationTile : WorldTile 
    {
        [SerializeField] protected List<PossibleEncounter> m_possibleEncounters = default;
        public EncounterDetail GetEncounter()
        {
            return SamplingUtil.SampleWithWeights(m_possibleEncounters).Value;
        }
    }
}