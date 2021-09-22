using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;

namespace Curry.Game
{
    public class NpcController : MonoBehaviour
    {
        [SerializeField] protected BaseNpc m_npc = default;
        [SerializeField] protected Animator m_anim = default;
        [SerializeField] protected DetectionHandler m_detector = default;
        protected Coroutine m_movingCall;
        protected Coroutine m_skillCall;

        public event OnCharacterDetected OnDetectCharacter;
        public event OnCharacterDetected OnCharacterExitDetection;

        public event OnCharacterTakeDamage OnTakingDamage;
        public BaseNpc Npc { get { return m_npc; } }

        protected virtual void Start() 
        {
            m_detector.OnDetected += OnCharacterDetected;
            m_detector.OnExitDetection += OnExitDetectionRange;

            m_npc.OnTakingDamage += OnTakeDamage;
            m_npc.OnDefeated += OnDefeat;
        }

        protected void OnCharacterDetected(BaseCharacter character) 
        {
            Debug.Log("detected" + character);
            OnDetectCharacter?.Invoke(character);
        }

        protected void OnExitDetectionRange(BaseCharacter character)
        {
            Debug.Log("exited" + character);
            OnCharacterExitDetection?.Invoke(character);
        }

        public virtual void ActivateSkill(ITargetable<Vector2> target, BaseSkill skill)
        {
            // Do not overlap skill calls
            if (m_skillCall != null) 
            {
                return;
            }

            m_skillCall = StartCoroutine(WindupSkill(target, skill));
        }

        public virtual void Wander()
        { 
        
        }

        public virtual void MoveTo(Vector2 targetPos) 
        {
            if (m_movingCall != null)
            {
                StopCoroutine(m_movingCall);
            }
            m_movingCall = StartCoroutine(OnMove(targetPos));
        }

        protected virtual IEnumerator OnMove(Vector2 targetPos) 
        {
            m_movingCall = null;
            yield return null;
        }  

        protected virtual IEnumerator WindupSkill(ITargetable<Vector2> target, BaseSkill skill) 
        {
            m_anim.SetBool("WindingUp", true);
            skill.SkillWindup();
            yield return new WaitForSeconds(skill.SkillProperties.MaxWindupTime);
            m_anim.SetBool("WindingUp", false);
            VectorParam param = new VectorParam(target);
            skill.Execute(param);
            m_skillCall = null;
        }

        protected virtual void OnTakeDamage(float damage) 
        {
            OnTakingDamage?.Invoke(damage);
        }

        protected virtual void OnDefeat() 
        { 

        }
    }
}
