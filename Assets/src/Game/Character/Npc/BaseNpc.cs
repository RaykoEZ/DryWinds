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

        public HashSet<BaseCharacter> Enemies { get { return m_enemies; } }
        public HashSet<BaseCharacter> Allies { get { return m_allies; } }

        protected virtual void OnEnable()
        {
            m_statusManager.Init(this, m_contextFactory);
            m_detector.OnDetected += OnTargetDetected;
            m_detector.OnExitDetection += OnLosingTarget;
        }

        protected virtual void OnDisable() 
        {
            m_detector.OnDetected -= OnTargetDetected;
            m_detector.OnExitDetection -= OnLosingTarget;
        }

        public override void OnKnockback(Vector2 direction, float knockback) 
        {
            base.OnKnockback(direction, knockback);
        }

        // Methods for adding/removing enemies in detection range.
        protected virtual void OnTargetDetected(BaseCharacter character)
        {
            bool isFoe = character.Relations != Relations;
            Action action;
            if (isFoe)
            {
                action = () =>
                {
                    m_enemies.Add(character);
                    OnDetectCharacter?.Invoke(character);
                };
            }
            else
            {
                action = () =>
                {
                    m_allies.Add(character);
                    OnDetectCharacter?.Invoke(character);
                };
            }
            StartCoroutine(Reaction(action));
        }

        protected virtual void OnLosingTarget(BaseCharacter character)
        {
            bool isFoe = character.Relations != Relations;
            Action action;
            if (isFoe)
            {
                action = () => {
                    m_enemies.Remove(character);
                    OnCharacterExitDetection?.Invoke(character);
                };
            }
            else
            {
                action = () => {
                    m_allies.Remove(character);
                    OnCharacterExitDetection?.Invoke(character);
                };
            }
            StartCoroutine(Reaction(action));
        }

        protected virtual IEnumerator Reaction(Action action)
        {
            yield return new WaitForSeconds(ReactionTime);
            // if character is still in range of view after some time, detect target.
            action?.Invoke();
        }
    }
}
