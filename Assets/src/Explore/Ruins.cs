﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace Curry.Explore
{
    public class Ruins : ExploreNode
    {
        protected override void OnDiscovered()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnLeave()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnReached()
        {
            Debug.Log(name);
        }
    }

}