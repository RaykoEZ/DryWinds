using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Ai;

namespace Curry.Game
{
    public delegate void OnNpcInteraction(InteractionContext c);
    public delegate void OnNpcKnockout();
    public enum InteractionType 
    { 
        OnDetect,
        OnLosingTarget,
        TakeDamage
    }
    public struct InteractionContext 
    { 
        public InteractionType Type { get { return m_type; } }
        InteractionType m_type;

        public InteractionContext(InteractionType t) 
        {
            m_type = t;
        }
    }

    public class BaseNpc : BaseCharacter
    {
        [SerializeField] protected CharacterDetector m_detector = default;
        bool m_knockedout = false;
        protected HashSet<BaseCharacter> m_enemies = new HashSet<BaseCharacter>();
        protected HashSet<BaseCharacter> m_allies = new HashSet<BaseCharacter>();
        protected List<NpcTerritory> m_territories = new List<NpcTerritory>();

        public event OnCharacterDetected OnDetectCharacter;
        public event OnCharacterDetected OnCharacterExitDetection;
        public event OnNpcInteraction OnEvaluate;
        public event OnNpcKnockout OnKnockout;
        public event OnNpcKnockout OnKnockoutRecover;
        public List<BaseCharacter> Enemies { get { return new List<BaseCharacter>(m_enemies); } }
        public List<BaseCharacter> Allies { get { return new List<BaseCharacter>(m_allies); } }
        public IReadOnlyList<NpcTerritory> Territories { get { return m_territories; } }


        protected override void OnWeakpointBreak(BodyPart part)
        {
            if (!m_knockedout)
            {
                OnKnockedout();
            }
        }

        protected virtual void OnKnockedout() 
        {
            m_knockedout = true;
            OnKnockout?.Invoke();
            StartCoroutine(KnockoutRecovery());
        }

        protected virtual IEnumerator KnockoutRecovery() 
        {
            CharacterStats stat = m_statusManager.CurrentStats.CharacterStats;
            float rand = UnityEngine.Random.Range(5f, 8f);
            yield return new WaitForSeconds(rand * stat.HitRecoveryTime);
            m_knockedout = false;
            OnKnockoutRecover?.Invoke();
        }

        public virtual void SetupTerritory(List<NpcTerritory> territories) 
        {
            m_territories = territories;
        }
        public virtual NpcTerritory ChooseRetreatDestination()
        {
            if(m_territories.Count < 1) 
            {
                return null;
            }

            NpcTerritory ret = m_territories[0];
            float leastDistance = Vector2.Distance(transform.position, ret.transform.position);
            for (int i = 1; i < m_territories.Count; ++i) 
            {
                float distance = Vector2.Distance(transform.position, m_territories[i].transform.position);
                if (distance < leastDistance) 
                {
                    leastDistance = distance;
                    ret = m_territories[i];
                }
            }
            return ret;
        }
        public override void Prepare()
        {
            base.Prepare();
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
            bool isFoe = character.name != name;
            if (isFoe)
            {
                m_enemies.Add(character);
            }
            else
            {
                m_allies.Add(character);
            }
            OnDetectCharacter?.Invoke(character);
            InteractionContext c = new InteractionContext(InteractionType.OnDetect);
            OnEvaluate?.Invoke(c);
        }

        protected virtual void OnLosingTarget(BaseCharacter character)
        {
            bool isFoe = character.name != name;
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
            InteractionContext c = new InteractionContext(InteractionType.OnLosingTarget);
            OnEvaluate?.Invoke(c);
        }

        public virtual void Retreat()
        {
            Despawn();
        }

        protected override void OnTakeDamage(float damage, int partDamage = 0)
        {
            base.OnTakeDamage(damage);
            InteractionContext c = new InteractionContext(InteractionType.TakeDamage);
            OnEvaluate?.Invoke(c);
        }
    }
}
