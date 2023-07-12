using System;
using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public interface IEffectInHand 
    {
        public void InHandEffect(GameStateContext c);
        public void OnLeaveHand(GameStateContext c);
    }
    public class SeedOfPlenty : AdventCard, IEffectInHand
    {
        public override bool IsActivatable(GameStateContext c)
        { return false; }
        public override IEnumerator ActivateEffect(ICharacter user, GameStateContext context)
        {       
            yield return null;
        }

        public void InHandEffect(GameStateContext c)
        {
            throw new NotImplementedException();
        }

        public void OnLeaveHand(GameStateContext c)
        {
            throw new NotImplementedException();
        }
    }
}