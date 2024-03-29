﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Util
{
    [Serializable]
    public class RangeMap
    {
        public static RangeMap Self => new RangeMap( new List<Vector3Int> { Vector3Int.zero });
        public static RangeMap Adjacent => new RangeMap(
            new List<Vector3Int> { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right});
        //List for common operations
        [SerializeField] List<Vector3Int> m_offsetList;
        public IReadOnlyList<Vector3Int> OffsetsFromOrigin { get { return m_offsetList; } }
        public RangeMap(IEnumerable<Vector3Int> rangeTiles)
        {
            m_offsetList = new List<Vector3Int>(rangeTiles);
        }

        public RangeMap(List<Vector3Int> rangeTiles)
        {
            m_offsetList = rangeTiles;
        }

        public List<Vector3Int> ApplyRangeOffsets(Vector3Int origin)
        {
            List<Vector3Int> ret = new List<Vector3Int>();
            foreach (Vector3Int offset in m_offsetList)
            {
                ret.Add(origin + offset);
            }

            return ret;
        }
        public List<Vector3> ApplyRangeOffsets(Vector3 origin)
        {
            List<Vector3> ret = new List<Vector3>();
            foreach (Vector3 offset in m_offsetList)
            {
                ret.Add(origin + offset);
            }

            return ret;
        }

        public bool IsInRange(Vector3Int origin, Vector3Int target)
        {
            return m_offsetList.Contains(target - origin);
        }

        public bool IsInRange(Vector3Int offset)
        {
            return m_offsetList.Contains(offset);
        }
    }
}
