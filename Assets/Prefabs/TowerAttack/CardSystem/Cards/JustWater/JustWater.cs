using System;
using System.Collections;

namespace Curry.Explore
{
    [Serializable]
    public class JustWater : CardResource, IConsumable
    {
        public override bool IsActivatable(GameStateContext c)
        { return false; }
        public JustWater(CardResource effect) : base(effect)
        {
        }
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