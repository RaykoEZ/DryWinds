using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Push_", menuName = "Curry/Effects/Push", order = 1)]
    public class Push_EffectResource : BaseEffectResource
    {
        [SerializeField] Push m_push = default;
        public Push PushModule => m_push;

        public override void Activate(GameStateContext context)
        {
            // do nothing for now, TODO: implement this if we need it later
        }
    }
}
