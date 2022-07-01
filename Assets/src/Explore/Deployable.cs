using Curry.Game;
using System;
using UnityEngine;

namespace Curry.Explore
{
    public class Deployable : Explorer, IPathExplorer
    {
        public DeploymentResource DeployCost = default;

    }
}
