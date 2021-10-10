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

        public override bool PreCondition(NpcWorldState args)
        {
            return args.Enemies.Count == 0;
        }

        public override ICharacterAction<IActionInput> Execute(NpcController controller, NpcWorldState state)
        {
            float randDegree = UnityEngine.Random.Range(-180f, 180f);
            Vector2 randRot = Quaternion.AngleAxis(randDegree, Vector3.forward) * controller.FaceDirection;
            Vector2 future = controller.Character.RigidBody.position + WanderDistance * randRot;
            controller.MoveTo(future);
            return null;
        }

        protected virtual Vector2 RandomDirection() 
        {
            return Vector2.zero;
        }
    }
}
