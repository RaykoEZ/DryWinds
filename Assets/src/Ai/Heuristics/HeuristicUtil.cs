using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    public static class HeuristicUtil
    {
        public static BaseCharacter WeakestCharacter(List<BaseCharacter> characters) 
        {
            if(characters == null || characters.Count == 0) 
            {
                return null;
            }
            else
            {
                BaseCharacter target = characters[0];
                for (int i = 1; i < characters.Count; ++i)
                {
                    BaseCharacter current = characters[i];
                    if (target.CurrentStats.Stamina > current.CurrentStats.Stamina)
                    {
                        target = current;
                    }
                }
                return target;
            }
        }
    }
}
