using System;
using System.Collections;
using UnityEngine;

namespace Curry.Util
{
    public static class GameUtil
    { 
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
    }
}
