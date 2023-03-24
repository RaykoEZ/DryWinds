using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public class SharpShooter : TacticalEnemy 
    {
        [SerializeField] protected StormMarrowRound m_stormAmmo = default;
        protected override bool ChooseAction_Internal(int dt, out IEnumerator action)
        {
            // if we see target, do basic action
            if (SpotsTarget)
            {
                action = ExecuteAction_Internal();
            }
            else
            {
                // find all enemies who can see a target
                action = Disengage();
            }
            return SpotsTarget;
        }
        protected IEnumerator Disengage() 
        {
            Hide();
            yield return null;
        }
        protected override IEnumerator ExecuteAction_Internal()
        {
            yield return base.ExecuteAction_Internal();
            foreach (IPlayer target in TargetsInSight)
            {
                yield return StartCoroutine(m_stormAmmo.FireAt(target.WorldPosition));
                break;
            }
            yield return null;
        }
    }
}
