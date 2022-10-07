using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    public class AdventSkaters : AdventCard
    {
        protected override void ActivateEffect(Adventurer user)
        {
            //Activatable = false;
            Debug.Log("Skate activate: "+ user.name);
            OnExpend();
        }

        // On instance init
        public override void Prepare()
        {
            Activatable = true;
        }
    }
}
