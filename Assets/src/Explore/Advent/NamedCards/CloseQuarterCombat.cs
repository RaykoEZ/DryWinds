using UnityEngine;
namespace Curry.Explore
{
    public class CloseQuarterCombat: AdventCard , ITargetsPosition
    {
        [SerializeField] LayerMask m_targetLayer = default;
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
            RaycastHit2D[] hits = Physics2D.RaycastAll(Target + Vector3.up, Vector3.down, m_targetLayer);
            foreach(RaycastHit2D hit in hits) 
            {
                if (hit && hit.transform.TryGetComponent(out TacticalEnemy enemy))
                {
                    enemy.TakeHit();
                    break;
                }
            }
            OnExpend();
        }
    }
}
