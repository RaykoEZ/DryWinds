using System;

namespace Curry.Explore
{
    public interface IEffectModule 
    {
        void ApplyEffect(ICharacter target, ICharacter user);
    }
}
