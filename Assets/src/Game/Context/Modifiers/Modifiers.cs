using System;

namespace Curry.Game
{
    [Serializable]
    public class CharacterAdder : CharacterModifier
    {
        public override ModifierOpType Type { get { return ModifierOpType.Add; } }
        public CharacterAdder(
            string name,
            CharacterModifierProperty value, 
            float duration ) :
            base(name, value, duration)
        {
        }

        public override CharacterModifierProperty Process(CharacterModifierProperty baseVal)
        {
            return baseVal + m_value;
        }
        public override CharacterModifierProperty Revert(CharacterModifierProperty baseVal)
        {
            return baseVal - m_value;
        }
    }

    [Serializable]
    public class CharacterMultiplier : CharacterModifier
    {
        public override ModifierOpType Type { get { return ModifierOpType.Multiply; } }

        public CharacterMultiplier(string name, CharacterModifierProperty value, float duration) :
            base(name, value, duration)
        {
        }


        public override CharacterModifierProperty Process(CharacterModifierProperty baseVal)
        {
            return baseVal * m_value;
        }

        public override CharacterModifierProperty Revert(CharacterModifierProperty baseVal)
        {
            return baseVal / m_value;
        }
    }
}
