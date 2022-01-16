using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    [Serializable]
    [CreateAssetMenu(menuName = "Curry/AiState/Wander", order = 0)]
    public class AIWander : AiAction<IActionInput>
    {
        [SerializeField] float m_averageWanderDistance = 3f;
        float WanderDistance { get { return UnityEngine.Random.Range(0.8f, 1.2f) * m_averageWanderDistance; } }

        public override bool PreCondition(AiWorldState args)
        {
            return args.Enemies.Count == 0;
        }

        public override ICharacterAction<IActionInput> Execute(NpcController controller, AiWorldState state)
        {
            Debug.Log("Wander");
            Vector2 randDir = GetDirection();
            Vector2 randPos = GetDestination(
                controller.Character.RigidBody.position, randDir);
            controller.Move(randPos);
            return null;
        }

        protected virtual Vector2 GetDirection() 
        {
            float randDirX = UnityEngine.Random.Range(-1, 1);
            float randDirY = UnityEngine.Random.Range(-1, 1);
            Vector2 dir = new Vector2(randDirX, randDirY);
            return dir.normalized;
        }

        protected virtual Vector2 GetDestination(Vector2 origin, Vector2 dir) 
        {
            float randDegree = UnityEngine.Random.Range(-180f, 180f);
            Vector2 randRot = Quaternion.AngleAxis(randDegree, Vector3.forward) * dir;
            Vector2 future = origin + WanderDistance * randRot;
            return future;
        }
    }
}
