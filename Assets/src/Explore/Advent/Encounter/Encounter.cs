using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    // A card that activates itself upon player drawing the card
    public abstract class Encounter : AdventCard
    {
        public virtual void OnDrawEffect(AdventurerStats user)
        {
            ActivateEffect(user);
        }
        protected override void ActivateEffect(AdventurerStats user)
        {
            Debug.Log("Encounter Activate!!!!!!!!!");
            OnExpend();
        }
    }
}