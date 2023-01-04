using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    // A card that activates itself upon player drawing the card
    public abstract class Encounter : AdventCard
    {
        protected override void ActivateEffect(IPlayer user)
        {
            Debug.Log("Encounter Activate!!!!!!!!!");
            OnExpend();
        }
    }
}