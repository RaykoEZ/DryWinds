using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public struct StrokeSetting 
    {
        public float DecayPerInterval;
        // how long until next decay interval (sec)
        public float DecayCountdowmLife;
        public bool IsMakingCollider;

        public StrokeSetting(float decayPerInterval, float life, bool isColliding = true) 
        {
            DecayPerInterval = decayPerInterval;
            DecayCountdowmLife = life;
            IsMakingCollider = isColliding;
        }
    }

    public class StrokeManager
    {
        public void Init() 
        { 
        
        
        }


    }
}
