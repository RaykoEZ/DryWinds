using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Ai;

namespace Curry.Game
{
    public class BaseNpc : BaseCharacter
    {
        [SerializeField] float m_averageReactionTime = default;
        [SerializeField] protected CharacterDetector m_detector = default;

        protected CharacterContextFactory m_contextFactory = new CharacterContextFactory();
        protected HashSet<BaseCharacter> m_enemies = new HashSet<BaseCharacter>();
        protected HashSet<BaseCharacter> m_allies = new HashSet<BaseCharacter>();

        public event OnCharacterDetected OnDetectCharacter;
        public event OnCharacterDetected OnCharacterExitDetection;
        protected virtual float ReactionTime
        {
            get
            {
                return UnityEngine.
                    Random.Range(0.9f * m_averageReactionTime, 1.1f * m_averageReactionTime);
            }
        }

        public List<BaseCharacter> Enemies { get { return new List<BaseCharacter>(m_enemies); } }
        public List<BaseCharacter> Allies { get { return new List<BaseCharacter>(m_allies); } }

        public override void Prepare()
        {
            Init(m_contextFactory);
            m_detector.OnDetected += OnTargetDetected;
            m_detector.OnExitDetection += OnLosingTarget;
        }

        protected virtual void OnDisable() 
        {
            m_detector.OnDetected -= OnTargetDetected;
            m_detector.OnExitDetection -= OnLosingTarget;
        }

        // Methods for adding/removing enemies in detection range.
        protected virtual void OnTargetDetected(BaseCharacter character)
        {
            bool isFoe = character.Relations != Relations;
            if (isFoe)
            {
                m_enemies.Add(character);
                OnDetectCharacter?.Invoke(character);
            }
            else
            {
                m_allies.Add(character);
                OnDetectCharacter?.Invoke(character);
            }
        }

        protected virtual void OnLosingTarget(BaseCharacter character)
        {
            bool isFoe = character.Relations != Relations;
            if (isFoe)
            {
                m_enemies.Remove(character);
                OnCharacterExitDetection?.Invoke(character);

            }
            else
            {
                m_allies.Remove(character);
                OnCharacterExitDetection?.Invoke(character);
            }
        }
    }
}
