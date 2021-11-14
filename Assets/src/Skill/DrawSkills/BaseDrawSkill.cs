using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Util;

namespace Curry.Skill
{
    public abstract class BaseDrawSkill : BaseSkill
    {
        [SerializeField] protected PrefabLoader m_traceRef = default;
        [SerializeField] protected InteractableInstanceManager m_instanceManager = default;

        protected Vector2 m_previousDrawPos;
        protected BaseTracer m_currentTracer;
        public virtual float CooldownTime { get { return m_skillProperty.CooldownTime; } }
        public GameObject TracerRef { get; protected set; }

        public override bool IsUsable
        {
            get
            {
                return base.IsUsable && TracerRef != null;
            }
        }

        public override void Init(BaseCharacter user)
        {
            base.Init(user);
            m_traceRef.OnLoadSuccess += (obj) => 
            { 
                TracerRef = obj;
            };
            m_traceRef.LoadAsset();
        }

        public override void Execute(IActionInput param)
        {
            // If spawned traces is not ready, don't draw 
            if(!IsUsable) 
            {
                Interrupt();
                OnSkillFinish();
                return; 
            }
            if (param is VectorInput posParam) 
            {
                // start a new stroke if we hold LMB (already drawing) and is moving
                if (!ActionInProgress)
                {
                    EndTracer();
                    m_previousDrawPos = posParam.Target;
                    // make new stroke
                    m_currentTracer = m_instanceManager.GetInstanceFromAsset(TracerRef) as BaseTracer;
                    m_currentTracer.OnActivate += OnSkillEffectActivate;
                    ActionInProgress = true;                   
                }

                float dist = Vector2.Distance(posParam.Target, m_previousDrawPos);
                float totalCost = dist * Properties.SpCost;
                if (totalCost <= m_user.CurrentStats.SP)
                {
                    //update mousePos log
                    ConsumeResource(totalCost);
                    m_currentTracer.OnTrace(posParam.Target);
                }
                else if( m_user.CurrentStats.SP > 0f )
                {
                    float scale = m_user.CurrentStats.SP / totalCost;
                    Vector2 lerp = Vector2.Lerp(m_previousDrawPos, posParam.Target, scale);
                    ConsumeResource(m_user.CurrentStats.SP);
                    m_currentTracer.OnTrace(lerp);
                }
                m_previousDrawPos = posParam.Target;
            }
        }

        public override void Interrupt()
        {
            EndTracer();
            base.Interrupt();
        }

        protected virtual void EndTracer()
        {
            if(m_currentTracer != null && m_currentTracer.isActiveAndEnabled) 
            {
                m_currentTracer.ActivateEffect();
            } 
        }

        protected virtual void OnSkillEffectActivate(RegionInput input) 
        {
            m_currentTracer.OnActivate -= OnSkillEffectActivate;
            m_currentTracer.OnClear();
            EndTracer();
            CoolDown();
            OnSkillFinish();
            m_animator.SetTrigger("Start");
            StartCoroutine(SkillEffect(input));

        }
    }
}
