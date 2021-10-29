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
        protected BaseTracer m_currentTracer;
        public virtual float CooldownTime { get { return m_skillProperty.CooldownTime; } }
        public GameObject AssetRef { get; protected set; }
        public override bool IsUsable
        {
            get
            {
                return base.IsUsable && AssetRef != null;
            }
        }

        public override void Init(BaseCharacter user)
        {
            base.Init(user);
            m_traceRef.OnLoadSuccess += (obj) => 
            { 
                AssetRef = obj;
                m_instanceManager.PrepareNewInstance(AssetRef);
            };
            m_traceRef.LoadAsset();
        }

        protected override IEnumerator SkillEffect(IActionInput target) 
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log("Draw Skill");
        }

        public override void Execute(IActionInput param)
        {
            // If spawned traces is not ready, don't draw 
            if(!IsUsable) 
            {
                OnSkillFinish();
                return; 
            }
            if (param is VectorInput posParam) 
            {
                // start a new stroke if we hold LMB (already drawing) and is moving
                if (!ActionInProgress)
                {
                    // make new stroke
                    m_currentTracer = m_instanceManager.GetInstanceFromCurrentPool() as BaseTracer;
                    m_currentTracer.OnActivate += OnSkillEffectActivate;

                }
                float length = !ActionInProgress ? 0f : Vector2.Distance(posParam.Target, m_previousDrawPos);
                float totalCost = length * Properties.SpCost;
                //update mousePos log
                m_previousDrawPos = posParam.Target;
                ConsumeResource(totalCost);
                m_currentTracer.OnTrace(posParam.Target, length);
                ActionInProgress = true;
            }
        }

        protected virtual void OnSkillEffectActivate(IActionInput input) 
        {
            m_currentTracer.OnActivate -= OnSkillEffectActivate;
            OnSkillFinish();
            m_currentSkill = StartCoroutine(SkillEffect(input));
        }

        protected override void OnSkillFinish()
        {
            m_currentTracer.OnClear();
            CoolDown();
            base.OnSkillFinish();
        }
    }
}
