using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public struct ExplorePath
    {
        public Queue<Vector2> Destinations { get { return m_destinations; } }
        public bool Finished { get { return Destinations.Count == 0; } }
        Queue<Vector2> m_destinations;
        public ExplorePath(Queue<Vector2> dest) 
        {
            m_destinations = dest;
        }
    }

    public interface IPathExplorer 
    {
        ExplorePath CurrentPath { get; }
        Vector2 CurrentDestination { get; }

        void StartExploration();
        void PauseExploration();
        void OnDestinationReached();
    }
}