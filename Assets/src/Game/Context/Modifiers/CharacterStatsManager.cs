using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class CharacterStatsManager : MonoBehaviour
    {
        List<ContextModifier<CharacterContext>> m_adders = new List<ContextModifier<CharacterContext>>();
        List<ContextModifier<CharacterContext>> m_multipliers = new List<ContextModifier<CharacterContext>>();

        CharacterContext m_baseStats;
        CharacterContext m_effectiveStats;

        CharacterContext m_resultantAdder;
        CharacterContext m_resultantMultiplier;

        public virtual CharacterContext BaseStats { get { return m_baseStats; } }
        public virtual CharacterContext EffectiveStats { get { return m_effectiveStats; } }

        public virtual CharacterContext ResultantAdder { get { return m_resultantAdder; } }
        public virtual CharacterContext ResultantMultiplier { get { return m_resultantMultiplier; }}


        public virtual void ApplyAdder(ContextModifier<CharacterContext> modifier) 
        { 
            if(modifier == null) 
            { 
                return; 
            }
        }

        public virtual void ApplyMultiplier(ContextModifier<CharacterContext> modifier) 
        {
            if (modifier == null) 
            { 
                return; 
            }
        }
    }
}
