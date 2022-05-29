using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Curry.Util;
using Curry.Game;

namespace Curry.Skill
{
    public delegate void OnSkillLoadFinish();
    // A container class for skill assets
    public class SkillInventory
    {
        protected int m_equippedIndex = 0;
        protected int m_loadFinishedCount = 0;
        protected int m_numLoadSceduled = 0;

        protected Transform m_parent;
        protected BaseCharacter m_userRef;
        protected List<ICharacterAction<IActionInput>> m_skillSet = new List<ICharacterAction<IActionInput>>();
        protected List<PrefabLoader> m_loaders = new List<PrefabLoader>();
        public event OnSkillLoadFinish OnFinish;
        public List<ICharacterAction<IActionInput>> Skills { get { return new List<ICharacterAction<IActionInput>>(m_skillSet); } }
        public ICharacterAction<IActionInput> CurrentSkill { get { return Skills[EquippedIndex]; } }

        public bool SkillAssetsLoaded { get; protected set; }
        public int EquippedIndex { 
            get { return m_equippedIndex; } 
            set { m_equippedIndex = Mathf.Clamp(value, 0, m_skillSet.Count - 1); } }

        public void Init(BaseCharacter user, List<AssetReference> skillRefs, Transform parent) 
        {
            m_userRef = user;
            m_parent = parent;
            foreach (AssetReference skillRef in skillRefs)
            {
                m_loaders.Add(new PrefabLoader(skillRef, OnLoadFinish));
            }
            LoadInitialAssets();
        }

        protected void LoadInitialAssets() 
        {
            foreach (PrefabLoader loader in m_loaders)
            {
                loader.LoadAsset();
            }
        }

        public void AddSkill(ICharacterAction<IActionInput> skill) 
        {
            m_skillSet.Add(skill);
        }

        protected virtual void OnLoadFinish(GameObject obj) 
        {
            GameObject skillInstance = Object.Instantiate(obj, m_parent);
            skillInstance.transform.position = Vector3.zero;
            BaseSkill skill = skillInstance.GetComponent<BaseSkill>();
            skill.Init(m_userRef);
            ICharacterAction<IActionInput> skillAction = skill;
            m_skillSet.Add(skillAction);
            ++m_loadFinishedCount;
            LoadFinishCheck();
        }

        protected void LoadFinishCheck() 
        {
            bool finished = m_loadFinishedCount == m_loaders.Count;
            if (finished)
            {
                SkillAssetsLoaded = true;
                OnFinish?.Invoke();
            }
        }
    }
}
