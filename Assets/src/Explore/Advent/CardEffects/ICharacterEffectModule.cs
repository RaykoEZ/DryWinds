using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public interface ICharacterEffectModule 
    {
        void ApplyEffect(ICharacter target, ICharacter user);
    }
    public interface ITileEffectModule 
    {
        void ApplyEffect(Vector3 targetTileWorldPos);
    }
}
