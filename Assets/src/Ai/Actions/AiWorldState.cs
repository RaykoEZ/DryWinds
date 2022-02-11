using System.Collections.Generic;
using Curry.Skill;
using Curry.Game;

namespace Curry.Ai
{
    public struct AiWorldState 
    {
        public AiEmotionState EmotionState { get; set; }
        public PathState MovementState { get; set; }
        public CharacterStats CurrentStats { get; set; }
        public List<BaseCharacter> Enemies { get; set; }
        public List<BaseCharacter> Allies { get; set; }
        public List<ICharacterAction<IActionInput>> BasicSkills { get; set; }
        public List<ICharacterAction<IActionInput>> DrawSkills { get; set; }

        public AiWorldState(
            AiEmotionState emotion,
            PathState movement,
            CharacterStats currentStats,
            List<BaseCharacter> enemies,
            List<BaseCharacter> allies,
            List<ICharacterAction<IActionInput>> basicSkills,
            List<ICharacterAction<IActionInput>> drawSkills)
        {
            EmotionState = emotion;
            MovementState = movement;
            CurrentStats = currentStats;
            Enemies = enemies;
            Allies = allies;
            BasicSkills = basicSkills;
            DrawSkills = drawSkills;
        }
    }
}
