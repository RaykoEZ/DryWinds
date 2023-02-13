using UnityEngine;
using Curry.Util;
using System.Collections;

namespace Curry.Explore
{
    public abstract class PositionTargetCard : AdventCard, ITargetsPosition 
    {
        [SerializeField] protected LayerMask m_targetLayer = default;
        public Vector3 Target { get; protected set; }
        public virtual int Range => 1;
        public override bool Activatable
        {
            get { return Satisfied; }
        }
        public virtual bool Satisfied { get; protected set; }
        protected RaycastHit2D[] ValidTargets => GameUtil.SearchTargetPosition(Target, m_targetLayer);
        public override void Prepare()
        {
            Satisfied = false;
        }
        public virtual void SetTarget(Vector3 target)
        {
            Target = target;
            Satisfied = true;
        }

        public override IEnumerator ActivateEffect(IPlayer user)
        {
            foreach(RaycastHit2D hit in ValidTargets) 
            {
                if (hit.transform.TryGetComponent(out ICharacter target))
                {
                    Effect_Internal(target, user);
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        protected abstract void Effect_Internal(ICharacter target, IPlayer user);
    }
}
