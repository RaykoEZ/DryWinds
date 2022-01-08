using System.Collections.Generic;
using Curry.Skill;
using Curry.Game;

namespace Curry.Ai
{
    public struct AiWorldState 
    {
        public AiEmotion Emotion { get; set; }
        public CharacterStats CurrentStats { get; set; }
        public List<BaseCharacter> Enemies { get; set; }
        public List<BaseCharacter> Allies { get; set; }
        public List<ICharacterAction<IActionInput>> BasicSkills { get; set; }
        public List<ICharacterAction<IActionInput>> DrawSkills { get; set; }

        public AiWorldState(
            AiEmotion emotion,
            CharacterStats currentStats,
            List<BaseCharacter> enemies,
            List<BaseCharacter> allies,
            List<ICharacterAction<IActionInput>> basicSkills,
            List<ICharacterAction<IActionInput>> drawSkills)
        {
            Emotion = emotion;
            CurrentStats = currentStats;
            Enemies = enemies;
            Allies = allies;
            BasicSkills = basicSkills;
            DrawSkills = drawSkills;
        }
    }
}
