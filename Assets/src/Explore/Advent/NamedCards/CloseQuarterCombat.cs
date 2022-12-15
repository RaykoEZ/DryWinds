using UnityEngine;
namespace Curry.Explore
{
    public class CloseQuarterCombat: AdventCard , ITargetsPosition
    {
        public Vector3 Target { get; protected set; }
        public int Range => 1;
        public override bool Activatable 
        {
            get { return Satisfied; }
        }

        public bool Satisfied { get; protected set; }

        public void SetTarget(Vector3 target)
        {
            Target = target;
            Satisfied = true;
        }

        protected override void ActivateEffect(AdventurerStats user)
        {
            Debug.Log(name);
            base.ActivateEffect(user);
            OnExpend();
        }
    }
}
