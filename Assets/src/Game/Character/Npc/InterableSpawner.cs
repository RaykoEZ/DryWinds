using UnityEngine;

namespace Curry.Game
{
    public class InterableSpawner : MonoBehaviour 
    {
        [SerializeField] protected Transform m_defaultParent = default;
        [SerializeField] InteractableInstanceManager m_instanceManager = default;
        public virtual void Spawn(Interactable objRef)
        {

        }
    }

}

