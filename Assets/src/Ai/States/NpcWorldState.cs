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
        public List<BaseSkill> Skills { get; private set; }

        public NpcWorldState(
            CharacterStats currentStats,
            List<BaseCharacter> enemies,
            List<BaseCharacter> allies,
            List<BaseSkill> skills)
        {
            CurrentStats = currentStats;
            Enemies = enemies;
            Allies = allies;
            Skills = skills;
        }
    }
}
