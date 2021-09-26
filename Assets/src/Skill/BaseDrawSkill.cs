using System.Collections;
using UnityEngine;
using Curry.Game;
using Curry.Util;

namespace Curry.Skill
{
    public class BaseDrawSkill : BaseSkill
    {
        [SerializeField] protected PrefabLoader m_traceRef = default;
        [SerializeField] protected InteractableInstanceManager m_instanceManager = default;

        protected Vector2 m_previousDrawPos;
        protected BaseTrace m_currentTracerBehaviour;
        public virtual float CooldownTime { get { return m_skillProperty.CooldownTime; } }
        public GameObject AssetRef { get; protected set; }
        public override bool SkillUsable
        {
            get
            {
                return base.SkillUsable && AssetRef != null;
            }
        }

        public override void Init(BaseCharacter user, bool hitBoxOn = false)
        {
            base.Init(user, hitBoxOn);
            m_traceRef.OnLoadSuccess += (obj) => 
            { 
                AssetRef = obj;
                m_instanceManager.PrepareNewInstance(AssetRef);
            };
            m_traceRef.LoadAsset();
        }

        protected override IEnumerator SkillEffect(SkillParam target = null) 
        {
            yield break;
        }

        public override void Execute(SkillParam param)
        {
            // If spawned traces is not ready, don't draw 
            if(!SkillUsable) 
            {
                OnSkillFinish();
                return; 
            }
            if (param is VectorParam posParam) 
            {
                // start a new stroke if we hold LMB (already drawing) and is moving
                if (!ActionInProgress)
                {
                    // make new stroke
                    m_currentTracerBehaviour = m_instanceManager.GetInstanceFromCurrentPool() as BaseTrace;
                    m_currentTracerBehaviour.OnFinish += () => { ActionInProgress = false; };
                }
                float length = !ActionInProgress ? 0f : Vector2.Distance(posParam.Target, m_previousDrawPos);
                float totalCost = length * SkillProperties.SpCost;
                //update mousePos log
                m_previousDrawPos = posParam.Target;
                ConsumeResource(totalCost);
                m_currentTracerBehaviour.Execute(posParam.Target, length);
                ActionInProgress = true;
            }
        }

        public override void Interrupt()
        {
            CoolDown();
            base.Interrupt();
        }

        protected override void OnSkillFinish()
        {
            CoolDown();
            base.OnSkillFinish();
        }
    }
}
