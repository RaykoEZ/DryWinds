using UnityEngine;

namespace Curry.Explore
{
    public class Skater : Deployable
    {
        protected override void ActivateAbility()
        {
            Debug.Log("Skate Ability");
        }

        protected override void OnDeploy()
        {
            Debug.Log("SkateSkate");
            ActivateAbility();
        }
    }
}
