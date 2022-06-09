using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Util;

namespace Curry.Skill
{
    public class MinorProtection : BaseDrawSkill, ISummonSkill<LineInput>
    {
        [SerializeField] float m_shieldDuration = default;
        [SerializeField] protected PrefabLoader m_barrierSpawn = default;
        
        public virtual ISkillObject<LineInput> SummonObject { get; protected set; }

        protected FragileBarrier m_currentBarrierInstance;
        protected override void OnExecute(LineInput input)
        {
            Summon(input);
        }
        public override void Init(BaseCharacter user)
        {
            base.Init(user);
            m_barrierSpawn.OnLoadSuccess += (obj) =>
            {
                SummonObject = obj.GetComponent<FragileBarrier>();
            };
            m_barrierSpawn.LoadAsset();
        }

        public virtual void Summon(LineInput param) 
        {
            if(param.Payload == null)
            {
                param.Payload = new Dictionary<string, object>();
            }
            param.Payload.Add("duration", m_shieldDuration);
            m_currentBarrierInstance = m_instanceManager.GetInstanceFromAsset(SummonObject.go) as FragileBarrier;
            m_currentBarrierInstance?.Begin(param);
        }

        public virtual void Unsummon()
        {
            m_currentBarrierInstance?.End();
        }
    }
}
