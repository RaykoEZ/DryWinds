using UnityEngine;
namespace Curry.Explore
{

    // A basic card that requires targeting a tile in close range to activate
    public class Assault: AdventCard , ITargetsPosition
    {
        [SerializeField] protected LayerMask m_targetLayer = default;
        public Vector3 Target { get; protected set; }
        public virtual int Range => 1;
        public override bool Activatable 
        {
            get { return Satisfied; }
        }
        public bool Satisfied { get; protected set; }

        public override void Prepare()
        {
            Satisfied = false;
        }

        public void SetTarget(Vector3 target)
        {
            Target = target;
            Satisfied = true;
        }

        protected override void ActivateEffect(IPlayer user)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(Target - 5f * Vector3.forward, Vector3.forward, m_targetLayer);
            foreach(RaycastHit2D hit in hits) 
            {
                if (hit && hit.transform.TryGetComponent(out IEnemy enemy))
                {
                    enemy.TakeHit(1);
                    break;
                }
            }
            OnExpend();
        }
    }
}
