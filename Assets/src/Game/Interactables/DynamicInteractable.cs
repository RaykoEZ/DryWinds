using UnityEngine;

namespace Curry.Game
{
    public abstract class DynamicInteractable : Interactable, INonStaticObject
    {
        public virtual void UpdatePathfinder()
        {
            Bounds bounds = GetComponent<Collider2D>().bounds;
            AstarPath.active.UpdateGraphs(bounds);
        }
    }
}
