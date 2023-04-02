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
                StopMovement();
            }
            m_movementAgent.isStopped = false;
            yield return StartCoroutine(Movement(target));
        }
        public void StopMovement()
        {
            m_movementAgent.isStopped = true;
            m_movementAgent.velocity = Vector3.zero;
            m_movementAgent.ResetPath();
        }
        IEnumerator Movement(Vector3 target) 
        {
            if (m_movementAgent.SetDestination(target)) 
            {
                yield return new WaitUntil(() => m_movementAgent.hasPath &&
                (m_movementAgent.isStopped || 
                m_movementAgent.velocity.sqrMagnitude == 0f));
            }
        }
    }
}
