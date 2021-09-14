using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Curry.Game;

namespace Curry.Skill
{
    public class BaseTraceBrush : MonoBehaviour
    {
        [SerializeField] PoolManager m_poolManager = default;
        [SerializeField] TraceInventory m_inventory = default;
        [SerializeField] protected float m_cooldownTime = default;

        protected bool m_isDrawing = false;
        protected bool m_onCD = false;
        protected string m_currentTraceId = default;
        protected Vector2 m_previousDrawPos;
        protected BaseCharacter m_user;
        protected BaseTrace m_currentTracerBehaviour;
        protected ObjectPool m_currentPool = default;

        public TraceInventory Inventory { 
            get { return m_inventory; }
            protected set 
            { 
                if (value != null) 
                {
                    m_inventory = value;
                    PrepareTrace();
                }
            }
        }

        public virtual float CooldownTime { get { return m_cooldownTime; } }

        public virtual void Init(BaseCharacter user) 
        {
            m_user = user;
            PrepareTrace();
        }

        protected virtual void PrepareTrace()
        {
            TraceAsset trace = Inventory.EquippedTrace;
            m_currentTraceId = trace.name;
            if (m_poolManager.ContainsPool(m_currentTraceId))
            {
                m_currentPool = m_poolManager.GetPool(m_currentTraceId);
            }
            else
            {
                m_currentPool = m_poolManager.AddPool(m_currentTraceId, trace.Prefab);
            }
        }

        public void ChangeTrace(int index)
        {
            Inventory.EquippedTraceIndex = index;
            PrepareTrace();
        }
        public void NextTrace()
        {
            Inventory.EquippedTraceIndex++;
            PrepareTrace();
        }

        public void PreviousTrace()
        {
            Inventory.EquippedTraceIndex--;
            PrepareTrace();
        }

        public virtual void Draw(CharacterStats stats, Vector2 drawTo)
        {
            // don't draw when we are on cooldown
            if (m_onCD)
            {
                return;
            }

            // start a new stroke if we hold LMB and is moving
            if (!m_isDrawing)
            {
                // make new stroke, can be pooled objects in the future
                GameObject newTraceBehaviour = m_currentPool?.GetItem();
                m_currentTracerBehaviour = newTraceBehaviour.GetComponent<BaseTrace>();
                m_currentTracerBehaviour.Init(m_user, true);
            }
            
            if (!m_currentTracerBehaviour.SkillUsable)
            {
                OnTraceEnd();
                return;
            }

            float length = !m_isDrawing ? 0f : Vector2.Distance(drawTo, m_previousDrawPos);
            //update mousePos log
            m_previousDrawPos = drawTo;
            SkillTargetParam param =
                new SkillTargetParam(
                    drawTo,
                    new Dictionary<string, object> { { "length", length} });

            m_isDrawing = true;
            m_currentTracerBehaviour.Execute(param);
            m_currentTracerBehaviour.gameObject.SetActive(true);
        }

        public virtual void OnTraceEnd()
        {
            m_isDrawing = false;
            StartCoroutine(OnCoolDown());
        }

        protected virtual IEnumerator OnCoolDown() 
        {
            m_onCD = true;
            //start cooldown and reset skill states
            yield return new WaitForSeconds(m_cooldownTime);
            m_onCD = false;
        }

    }
}
