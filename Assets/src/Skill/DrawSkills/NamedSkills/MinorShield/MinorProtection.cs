using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;
using Curry.Util;

namespace Curry.Skill
{
    public interface ISummonSkill<T> where T : IActionInput
    { 
        ISummonableObject<T> SummonObject { get; }
        void Summon(T param);
    }

    public class MinorProtection : BaseDrawSkill, ISummonSkill<RegionInput>
    {
        [SerializeField] float m_shieldDuration = default;
        [SerializeField] protected PrefabLoader m_barrierSpawn = default;
        
        public ISummonableObject<RegionInput> SummonObject { get; protected set; }

        protected FragileBarrier m_currentBarrier;

        public virtual void Summon(RegionInput param) 
        {
            if(param.Payload == null)
            {
                param.Payload = new Dictionary<string, object>();
            }
            param.Payload.Add("duration", m_shieldDuration);
            m_currentBarrier = m_instanceManager.GetInstanceFromAsset(SummonObject.Self) as FragileBarrier;
            m_currentBarrier?.OnSummon(param);
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

        protected override IEnumerator ExecuteInternal(IActionInput target)
        {
            if(target is RegionInput input)
            {
                Summon(input);
            }
            // Start animation
            yield return null;
        }
    }
}
