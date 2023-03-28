using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Curry.Util
{
    public static class GameUtil
    { 
        public static List<int> RandomRangeUnique(int min, int max, int numToGet) 
        {
            List<int> ret = new List<int>();
            int index;
            while (ret.Count < numToGet)
            {
                index = UnityEngine.Random.Range(0, max);
                if (!ret.Contains(index))
                {
                    ret.Add(index);
                }
            }
            return ret;
        }
        // look for target hits in world position
        public static RaycastHit2D[] SearchTargetPosition(Vector3 targetPos, LayerMask searchFilter) 
        { 
            return Physics2D.CircleCastAll(targetPos, 0.01f, Vector2.zero, 0f, searchFilter);
        }
        // search for a specific target of type : T_Target, in world position
        public static bool TrySearchTarget<T_Target>(Vector3 targetPos, LayerMask searchFilter, out T_Target target) 
        {
            target = default;
            RaycastHit2D[] hits = SearchTargetPosition(targetPos, searchFilter);
            foreach(RaycastHit2D hit in hits) 
            {
                if (hit.transform.TryGetComponent(out T_Target found))
                {
                    target = found;
                    return true;
                }
            }
            return false;
        }
        /// returns ratio of two velocities
        public static float ScaleFactior(float v1, float v2, float minFactor, float maxFactor) 
        {
            float ret = v1 / (v2 + 0.1f);

            return Mathf.Clamp(ret, minFactor, maxFactor);      
        }

        // Calculate the surface area of an enclosured shape using cross
        public static float AreaOfEnclosure(Vector2[] verts) 
        {
            float temp = 0;       
            for (int i = 0; i < verts.Length; i++)
            {
                if (i != verts.Length - 1)
                {
                    float mulA = verts[i].x * verts[i + 1].y;
                    float mulB = verts[i + 1].x * verts[i].y;
                    temp = temp + (mulA - mulB);
                }
                else
                {
                    float mulA = verts[i].x * verts[0].y;
                    float mulB = verts[0].x * verts[i].y;
                    temp = temp + (mulA - mulB);
                }
            }
            temp *= 0.5f;
            return Mathf.Abs(temp);
        }

        public static void RenderLine(List<Vector2> verts,  LineRenderer line, EdgeCollider2D col = null) 
        {
            col?.SetPoints(verts);
            Vector3[] pos = VectorExtension.ToVector3Array(verts.ToArray());
            line.positionCount = pos.Length;
            line.SetPositions(pos);
        }
    }
}
