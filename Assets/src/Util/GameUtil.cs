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
    }
}
