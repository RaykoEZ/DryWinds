using System;
using System.Collections;
using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    public class Explorer : BaseCharacter, IPathExplorer
    {
        protected ExplorePath m_currentPath = new ExplorePath();
        protected Vector2 m_currentDest = new Vector2();

        public ExplorePath CurrentPath { get { return m_currentPath; } }
        public Vector2 CurrentDestination { get { return m_currentDest; } }

        public virtual void PauseExploration()
        {
            throw new NotImplementedException();
        }

        public virtual void StartExploration()
        {
            throw new NotImplementedException();
        }

        public virtual void OnDestinationReached()
        {
            throw new NotImplementedException();
        }

    }


    
}
