using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Curry.Util;

namespace Curry.Skill
{
    public delegate void OnSkillLoadFinish();
    // A container class for skill assets
    public class SkillInventory
    {
        protected int m_equippedIndex = 0;
        protected int m_loadFinishedCount = 0;
        protected Transform m_parent;
        protected HashSet<BaseSkill> m_skillSet = new HashSet<BaseSkill>();
        protected List<PrefabLoader> m_loaders = new List<PrefabLoader>();
        public event OnSkillLoadFinish OnFinish;
        public List<BaseSkill> Skills { get { return new List<BaseSkill>(m_skillSet); } }
        public bool SkillAssetsLoaded { get; protected set; }
        public int EquippedTraceIndex { 
            get { return m_equippedIndex; } 
            set { m_equippedIndex = Mathf.Clamp(value, 0, m_skillSet.Count - 1); } }

        public BaseSkill EquippedSkill 
        { get 
            { 
                return Skills[EquippedTraceIndex];
            } 
        }

        public void Init(List<AssetReference> skillRefs, Transform parent) 
        {
            m_parent = parent;
            foreach (AssetReference skillRef in skillRefs)
            {
                m_loaders.Add(new PrefabLoader(skillRef, OnLoadFinish));
            }

            foreach (PrefabLoader loader in m_loaders)
            {
                loader.LoadAsset();
            }
        }

        public void AddSkill(BaseSkill skill) 
        {
            m_skillSet.Add(skill);
        }

        protected virtual void OnLoadFinish(GameObject obj) 
        {
            GameObject skillInstance = Object.Instantiate(obj, m_parent);
            skillInstance.transform.position = Vector3.zero;
            m_skillSet.Add(skillInstance.GetComponent<BaseSkill>());
            ++m_loadFinishedCount;

            if(m_loadFinishedCount == m_loaders.Count) 
            {
                SkillAssetsLoaded = true;
                OnFinish?.Invoke();
            }
        }
    }
}
