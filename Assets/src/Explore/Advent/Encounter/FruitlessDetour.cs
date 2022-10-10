using UnityEngine;
using Curry.Events;
namespace Curry.Explore
{
    public class FruitlessDetour : Encounter
    {
        protected override void ActivateEffect(AdventurerStats user)
        {
            Debug.Log("Wasting time is cool too");
            OnExpend();
        }
    }
}