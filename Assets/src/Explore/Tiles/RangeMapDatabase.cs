using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Util;
using Newtonsoft.Json;

namespace Curry.Explore
{
    // A storage for all range patterns for game pieces in the game
    [CreateAssetMenu(fileName = "RangeTileMapDatabase", menuName = "Curry/Create range tile cache", order = 1)]
    public class RangeMapDatabase : ScriptableObject
    {
        // Internal data structure to deserialize json string into, need to convert to external type for general use.
        [Serializable]
        private class RangeOffsetMap_Internal
        {
            public List<Vector3Int> OffsetsFromOrigin = default;

            [JsonConstructor]
            public RangeOffsetMap_Internal(List<Vector3Int> offsets)
            {
                OffsetsFromOrigin = offsets;
            }

            public RangeOffsetMap_Internal(RangeMap map)
            {
                OffsetsFromOrigin = new List<Vector3Int>(map.OffsetsFromOrigin);
            }

            public RangeMap ToExternal()
            {
                return new RangeMap(OffsetsFromOrigin);
            }
        }

        [SerializeField] List<RangeOffsetMap_Internal> m_squareRadiusRangeMaps = new List<RangeOffsetMap_Internal>();

        Dictionary<T, RangeMap> ToExternal<T>(Dictionary<T, RangeOffsetMap_Internal> internalCollection)
        {
            Dictionary<T, RangeMap> ret = new Dictionary<T, RangeMap>();
            foreach (KeyValuePair<T, RangeOffsetMap_Internal> kvp in internalCollection)
            {
                ret[kvp.Key] = kvp.Value.ToExternal();
            }
            return ret;
        }

        public RangeMap GetSquareRadiusMap(int range)
        {
            if (range < 0 || range > m_squareRadiusRangeMaps.Count)
            {
                return m_squareRadiusRangeMaps[0].ToExternal();
            }

            return m_squareRadiusRangeMaps[range].ToExternal();

        }

        public void GenerateSquareRadiusMaps(int maxRange)
        {
            m_squareRadiusRangeMaps.Clear();
            for (int i = 0; i < maxRange + 1; ++i)
            {
                RangeOffsetMap_Internal mask = new RangeOffsetMap_Internal(RangeMapping.GetNeighbourRangeMap(i));
                m_squareRadiusRangeMaps.Add(mask);
            }
        }
    }
}