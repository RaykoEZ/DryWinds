using UnityEngine;
namespace Curry.Explore
{
    public class CloseQuarterCombat: AdventCard 
    {
        protected override void ActivateEffect(AdventurerStats user)
        {
            Debug.Log(name);
            base.ActivateEffect(user);
            OnExpend();
        }
    }
}
