using UnityEngine;

namespace Curry.Explore
{
    public class ImperialCompany : Deployable
    {
        protected override void ActivateAbility()
        {
            Debug.Log("AbiAbi");
        }

        protected override void OnDeploy()
        {
            Debug.Log("ImpImp");
        }
    }

}
