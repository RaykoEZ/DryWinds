using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Skill;
using System.Collections;

namespace Curry.Ai
{
    [Serializable]
    [AddComponentMenu("Curry/Ai Action/Wander")]
    public partial class AIWander : AiAction<IActionInput>
    {
        public float WanderDistance { get { return UnityEngine.Random.Range(0.8f, 1.2f) * 3f; } }
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
        public override bool PreCondition(AiWorldState args)
        {
            return args.Enemies.Count == 0;
        }

        public override void Execute(AiActionInput param)
        {
            if (param.Controller.IsReady) 
            {
                Vector2 randDir = GetDirection();
                Vector2 randPos = GetDestination(
                    param.Controller.Character.RigidBody.position, randDir);
                param.Controller.Move(randPos);
            }
        }

        protected override IEnumerator ExecuteInternal(AiActionInput param)
        {
            throw new NotImplementedException();
        }
    }
}
