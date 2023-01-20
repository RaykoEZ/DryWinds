using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    // A card that activates itself upon player drawing the card
    public abstract class Encounter : AdventCard
    {
        public override IEnumerator ActivateEffect(IPlayer user)
        {
            yield return null;
            OnExpend();
        }
    }
}