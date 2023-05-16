using System.Collections.Generic;
using UnityEngine;

namespace Curry.Util
{
    public static class RangeMapping
    {
        /// <summary>
        /// This function gets the offsets needed for defining a range of valid positions to move into, given the movement range. 
        /// </summary>
        /// <param name="range"></param> The max number of squares allowed to move from origin.
        /// <returns></returns> A set of offsets to define valid positions to in a range 
        public static RangeMap GetNeighbourRangeMap(int range)
        {
            if (range < 1)
            {
                return new RangeMap
                (new List<Vector3Int> { Vector3Int.zero });
            }

            // Getting the immediate neighbours is quicker here
            if (range == 1)
            {
                return GetImmediateNeighbourhood();
            }

            /// We process each positive coords within range, then we reflect them in each x-y region to create a square range. 
            /// n-squared complexity but called once for each different ranges.

            ///  This algorithm will generate 50% duplicate values 
            ///  when: i == 0 OR range - i == 0, so we need to check them
            /// 2
            /// 1 1
            /// 0 0 0
            List<Vector3Int> neighbours = new List<Vector3Int>();
            for (int i = range; i >= 0; --i)
            {
                for (int j = range - i; j >= 0; --j)
                {
                    if (i == 0 && j == 0)
                    {
                        break;
                    }
                    // Prevent duplicate allocation on edge case.
                    if (i == 0)
                    {
                        neighbours.Add(new Vector3Int(j, 0, 0));
                        neighbours.Add(new Vector3Int(-j, 0, 0));
                    }
                    else if (j == 0)
                    {
                        neighbours.Add(new Vector3Int(0, -i, 0));
                        neighbours.Add(new Vector3Int(0, i, 0));
                    }
                    else
                    {
                        neighbours.Add(new Vector3Int(j, i, 0));
                        neighbours.Add(new Vector3Int(j, -i, 0));
                        neighbours.Add(new Vector3Int(-j, i, 0));
                        neighbours.Add(new Vector3Int(-j, -i, 0));
                    }
                }
            }

            RangeMap map = new RangeMap(neighbours);
            return map;
        }
        public static List<Vector3> GetRangePositions(int range, Vector3 origin) 
        {
            RangeMap rangeMap = GetNeighbourRangeMap(range);
            return rangeMap.ApplyRangeOffsets(origin);
        }
        public static RangeMap GetRangeMap(int range)
        {
            return GetNeighbourRangeMap(range);
        }
        public static List<Vector3> GetRangePositions(RangeMap range, Vector3 origin)
        {
            return range.ApplyRangeOffsets(origin);
        }
        static RangeMap GetImmediateNeighbourhood()
        {
            List<Vector3Int> neighbours = new List<Vector3Int>();
            neighbours.Add(new Vector3Int(0, 1, 0));
            neighbours.Add(new Vector3Int(0, -1, 0));
            neighbours.Add(new Vector3Int(1, 0, 0));
            neighbours.Add(new Vector3Int(-1, 0, 0));

            RangeMap map = new RangeMap(neighbours);
            return map;
        }

    }
}
