using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Util;
using UnityEngine.InputSystem;
namespace Curry.Skill
{
    public abstract class BaseDrawSkill : BaseSkill
    {
        [SerializeField] protected PrefabLoader m_traceRef = default;
        protected Vector2 m_previousDrawPos;
        protected BaseTracer m_currentTracer;
        public virtual float CooldownTime { get { return m_skillProperty.CooldownTime; } }
        public GameObject TracerRef { get; protected set; }
        protected bool m_drawInProgress = false;
        public override bool IsUsable
        {
            get
            {
                return base.IsUsable && TracerRef != null;
            }
        }
        protected abstract void OnExecute(LineInput input);

        public override void Init(BaseCharacter user)
        {
            base.Init(user);
            m_traceRef.OnLoadSuccess += (obj) => 
            { 
                TracerRef = obj;
            };
            m_traceRef.LoadAsset();
        }

        public override void OnEnter(IActionInput param)
        {
            // If spawned traces is not ready, don't draw 
            if(!IsUsable) 
            {
                Interrupt();
                return; 
            }
            if (param is VectorInput posParam) 
            {
                Vector2 pos = posParam.Value;
                // start a new stroke if we hold LMB (already drawing) and is moving
                if (!m_drawInProgress)
                {
                    EndTracer();
                    m_previousDrawPos = pos;
                    // make new stroke
                    m_currentTracer = m_instanceManager.GetInstanceFromAsset(TracerRef) as BaseTracer;
                    m_currentTracer.OnActivate += OnSkillEffectActivate;
                    m_drawInProgress = true;
                }
                
                float dist = Vector2.Distance(pos, m_previousDrawPos);
                float totalCost = dist * Properties.SpCost;
                if (totalCost <= m_user.CurrentStats.SP)
                {
                    //update mousePos log
                    ConsumeResource(totalCost);
                    m_currentTracer.OnTrace(pos);
                }
                else if( m_user.CurrentStats.SP > 0f )
                {
                    float scale = m_user.CurrentStats.SP / totalCost;
                    Vector2 lerp = Vector2.Lerp(m_previousDrawPos, pos, scale);
                    ConsumeResource(m_user.CurrentStats.SP);
                    m_currentTracer.OnTrace(lerp);
                }
                m_previousDrawPos = pos;
            }
        }

        public override void Interrupt()
        {
            EndTracer();
            OnSkillFinish();
        }

        protected override void OnSkillFinish()
        {
            m_drawInProgress = false;
            base.OnSkillFinish();
        }

        protected virtual void EndTracer()
        {
            if(m_currentTracer != null && m_currentTracer.isActiveAndEnabled) 
            {
                m_currentTracer.ActivateEffect();
            } 
        }

        protected virtual void OnSkillEffectActivate(LineInput input) 
        {
            m_currentTracer.OnActivate -= OnSkillEffectActivate;
            m_currentTracer.OnClear();
            EndTracer();
            CoolDown();
            OnSkillFinish();
            m_animator.SetTrigger("Start");
            m_execute = StartCoroutine(ExecuteInternal(input));
        }

        protected override IEnumerator ExecuteInternal(IActionInput target)
        {
            if (target is LineInput input)
            {
                OnExecute(input);
            }
            yield return null;
        }
    }
}
