using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{  
    public class JustWater : AdventCard, IConsumable
    {
        
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {        
            yield return null;
        }

        public IEnumerator OnExpend()
        {
            yield return null;
        }
    }
}