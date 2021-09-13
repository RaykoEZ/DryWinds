using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public abstract class SkillHandler : MonoBehaviour 
    { 
        [SerializeField] PoolManager m_poolManager = default;
        string m_currentSkillAssetId = default;
        ObjectPool m_currentPool = default;

        public BaseSkill CurrentSkill
        {
            get;
            set;
        }

        public virtual void PrepareSkill(Asset skillAsset)
        {
            m_currentSkillAssetId = skillAsset.name;
            if (m_poolManager.ContainsPool(m_currentSkillAssetId))
            {
                m_currentPool = m_poolManager.GetPool(m_currentSkillAssetId);
            }
            else
            {
                m_currentPool = m_poolManager.AddPool(m_currentSkillAssetId, skillAsset.Prefab);
            }

            CurrentSkill = GetNewSkillInstance().GetComponent<BaseSkill>();
        }

        protected virtual GameObject GetNewSkillInstance() 
        {
            return m_currentPool?.GetItem();
        }

        public abstract void ActivateSkill(Vector2 targetPos, Dictionary<string, object> payload = null);

        protected virtual void ExecuteSkill(SkillParam param) 
        {
            CurrentSkill.gameObject.SetActive(true);
            CurrentSkill.Execute(param);
        }
    }
}
