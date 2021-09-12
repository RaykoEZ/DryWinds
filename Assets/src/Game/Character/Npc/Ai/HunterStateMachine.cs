using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    public class HunterStateMachine : AiStateMachine 
    {
        [SerializeField] protected AiState m_chase = default;
        [SerializeField] protected AiState m_skill = default;

        protected HashSet<BaseCharacter> m_enemies = new HashSet<BaseCharacter>();
        protected HashSet<BaseCharacter> m_allies = new HashSet<BaseCharacter>();

        protected override void Start()
        {
            base.Start();
            m_controller.OnDetectCharacter += OnTargetDetected;
            m_controller.OnCharacterExitDetection += OnTargetLost;
        }

        protected override void Evaluate()
        {

        }

        // Methods for adding/removing enemies in detection range.
        protected virtual void OnTargetDetected(BaseCharacter character) 
        {
            bool isFoe = character.Relations != m_controller.Npc.Relations;
            if (isFoe) 
            {
                m_enemies.Add(character);
            }
            else 
            {
                m_allies.Add(character);
            }
        }

        protected virtual void OnTargetLost(BaseCharacter character)
        {
            bool isFoe = character.Relations != m_controller.Npc.Relations;
            if (isFoe)
            {
                m_enemies.Remove(character);
            }
            else
            {
                m_allies.Remove(character);
            }
        }
    }
}
