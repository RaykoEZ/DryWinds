using UnityEngine;
using Curry.Events;
namespace Curry.Explore
{
    public class FruitlessDetour : Encounter
    {
        protected override void ActivateEffect(IPlayer user)
        {
            OnExpend();
        }
    }
}