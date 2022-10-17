using System;
using UnityEngine;

namespace Curry.Util
{
    public static class VectorExtension
    {
        static readonly Vector3[] s_cardinal = new Vector3[] 
        { Vector3.up, Vector3.down, Vector3.left, Vector3.right };
        public static Vector3 RandomCardinalVector() 
        {
            int rand = UnityEngine.Random.Range(0, s_cardinal.Length);
            Vector3 ret = s_cardinal[rand];
            return ret;
        }

        public static Vector2[] ToVector2Array(Vector3[] v3)
        {
            return Array.ConvertAll(v3, ToVec2);
        }
        public static Vector3[] ToVector3Array(Vector2[] v3)
        {
            return Array.ConvertAll(v3, ToVec3);
        }

        //Converters for vecs
        public static Vector2 ToVec2(Vector3 v3)
        {
            return new Vector2(v3.x, v3.y);
        }

        public static Vector3 ToVec3(Vector2 v2)
        {
            return new Vector3(v2.x, v2.y, 0f);
        }

        public static Vector3 VectorFromDegree(float degree)
        {
            float rad = degree * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
        }

        public static float DegreeFromDirection(Vector2 dir) 
        {
            float degree = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x);
            return degree;
        }
    }
}
