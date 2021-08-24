using System;

namespace Curry.Game
{
    [Serializable]
    public class CharacterAdder : ContextModifier<CharacterContext>
    {
        public override ModifierOpType Type { get { return ModifierOpType.Add; } }

        public CharacterAdder(string name, CharacterContext value, float duration) :
            base(name, value, duration)
        {
            m_name = name;
            m_value = value;
            m_duration = duration;
        }


        public override CharacterContext Apply(CharacterContext baseVal)
        {
            if (baseVal == null)
            {
                return null;
            }

            return baseVal + m_value;
        }
        public override CharacterContext Revert(CharacterContext baseVal)
        {
            if (baseVal == null)
            {
                return null;
            }

            return baseVal - m_value;
        }
    }

    [Serializable]
    public class CharacterMultiplier : ContextModifier<CharacterContext>
    {
        public override ModifierOpType Type { get { return ModifierOpType.Multiply; } }

        public CharacterMultiplier(string name, CharacterContext value, float duration) :
            base(name, value, duration)
        {
            m_name = name;
            m_value = value;
            m_duration = duration;
        }


        public override CharacterContext Apply(CharacterContext baseVal)
        {
            if (baseVal == null)
            {
                return null;
            }

            return baseVal * m_value;
        }

        public override CharacterContext Revert(CharacterContext baseVal)
        {
            if (baseVal == null)
            {
                return null;
            }

            return baseVal / m_value;
        }
    }
}
