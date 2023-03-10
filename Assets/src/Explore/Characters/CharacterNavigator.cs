using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Curry.Explore
{
    [Serializable]
    public class CharacterNavigator : MonoBehaviour
    {
        [SerializeField] NavMeshAgent m_movementAgent = default;
        public Vector3 AgentPosition => m_movementAgent.pathEndPosition;
        public IEnumerator MoveTo(Vector3 target, bool cancelCurrent = true)
        {
            if (cancelCurrent) 
            {
                ToggleMovement();
                m_movementAgent.ResetPath();
            }
            yield return StartCoroutine(Movement(target));
        }
        public void ToggleMovement(bool isStopped = true)
        {
            m_movementAgent.isStopped = isStopped;
        }
        IEnumerator Movement(Vector3 target) 
        {
            if (m_movementAgent.SetDestination(target)) 
            {
                yield return new WaitUntil(() => m_movementAgent.isStopped || 
                m_movementAgent.velocity.sqrMagnitude == 0f);
            }
        }
    }
}
