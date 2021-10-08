using System.Collections.Generic;
using Curry.Skill;
using Curry.Game;

namespace Curry.Ai
{
    public struct NpcWorldState 
    {
        public CharacterStats CurrentStats { get; set; }
        public List<BaseCharacter> Enemies { get; set; }
        public List<BaseCharacter> Allies { get; set; }
        public List<ICharacterAction<IActionInput, SkillProperty>> BasicSkills { get; set; }
        public List<ICharacterAction<IActionInput, SkillProperty>> DrawSkills { get; set; }

        public NpcWorldState(
            CharacterStats currentStats,
            List<BaseCharacter> enemies,
            List<BaseCharacter> allies,
            List<ICharacterAction<IActionInput, SkillProperty>> basicSkills,
            List<ICharacterAction<IActionInput, SkillProperty>> drawSkills)
        {
            CurrentStats = currentStats;
            Enemies = enemies;
            Allies = allies;
            BasicSkills = basicSkills;
            DrawSkills = drawSkills;
        }
    }
}
