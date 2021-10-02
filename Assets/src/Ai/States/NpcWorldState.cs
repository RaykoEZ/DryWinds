using System.Collections.Generic;
using Curry.Skill;
using Curry.Game;

namespace Curry.Ai
{
    public struct NpcWorldState 
    {
        public CharacterStats CurrentStats { get; private set; }
        public List<BaseCharacter> Enemies { get; private set; }
        public List<BaseCharacter> Allies { get; private set; }
        public List<BaseSkill> BasicSkills { get; private set; }
        public List<BaseSkill> DrawSkills { get; private set; }

        public NpcWorldState(
            CharacterStats currentStats,
            List<BaseCharacter> enemies,
            List<BaseCharacter> allies,
            List<BaseSkill> basicSkills,
            List<BaseSkill> drawSkills)
        {
            CurrentStats = currentStats;
            Enemies = enemies;
            Allies = allies;
            BasicSkills = basicSkills;
            DrawSkills = drawSkills;
        }
    }
}
