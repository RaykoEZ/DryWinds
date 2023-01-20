using UnityEngine;
using Curry.Events;
using System.Collections;

namespace Curry.Explore
{
    public class FruitlessDetour : Encounter
    {
        public override IEnumerator ActivateEffect(IPlayer user)
        {
            Debug.Log("Maidenless");
            yield return base.ActivateEffect(user);
        }
    }
}