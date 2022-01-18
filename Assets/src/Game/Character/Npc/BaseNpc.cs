using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Ai;

namespace Curry.Game
{
    public delegate void OnNpcEvaluate();
    public class BaseNpc : BaseCharacter
    {
        [SerializeField] protected CharacterDetector m_detector = default;
        BaseEmotionHandler m_emotions = new BaseEmotionHandler();
        float m_timer = 0f;
        protected CharacterContextFactory m_contextFactory = new CharacterContextFactory();
        protected HashSet<BaseCharacter> m_enemies = new HashSet<BaseCharacter>();
        protected HashSet<BaseCharacter> m_allies = new HashSet<BaseCharacter>();
        public event OnCharacterDetected OnDetectCharacter;
        public event OnCharacterDetected OnCharacterExitDetection;
        public event OnNpcEvaluate OnEvaluate;

        public virtual EmotionHandler Emotion { get { return m_emotions; } } 
        public List<BaseCharacter> Enemies { get { return new List<BaseCharacter>(m_enemies); } }
        public List<BaseCharacter> Allies { get { return new List<BaseCharacter>(m_allies); } }

        protected virtual void Update() 
        {
            m_timer += Time.deltaTime;
            if(m_timer > 1f) 
            {
                m_timer = 0f;
                m_emotions.Update(out bool emotionChanged);
                //Debug.Log($"{Emotion.Current}");
                OnEvaluate?.Invoke();
            }
        }
        public override void Init(CharacterContextFactory contextFactory)
        {
            base.Init(contextFactory);
            m_emotions.Init();
        }

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
                m_emotions.OnThreatDetected();
            }
            else
            {
                m_allies.Add(character);
            }
            OnDetectCharacter?.Invoke(character);
            OnEvaluate?.Invoke();
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
            OnEvaluate?.Invoke();
        }

        public override void OnTakeDamage(float damage)
        {
            base.OnTakeDamage(damage);
            m_emotions.OnTakeDamage();
            OnEvaluate?.Invoke();
        }
    }
}
