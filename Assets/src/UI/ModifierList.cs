using Curry.Explore;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.UI
{
    // For displaying a list of modifiers that is active on a character
    // This displays a snapshot so it doesn't update lodifier live
    public class ModifierList : MonoBehaviour , ICharacterUIContentList<ModifierContent>
    {
        [SerializeField] ModifierIcon m_iconRef = default;
        // icons we are not using right now
        [SerializeField] List<ModifierIcon> m_idleIcons = default;
        List<ModifierIcon> m_iconsInUse = new List<ModifierIcon>();
        public void Setup(ICharacter displayTarget, IReadOnlyList<ModifierContent> content) 
        {
            foreach (var item in content) 
            {
                OnModifierAdd(item);
            }
        }
        public void Show() 
        {
            // if we need to display button in the furure, implement to show all icons
        }
        public void Hide() 
        {
            ResetIcons();
        }
        void OnModifierAdd(ModifierContent content) 
        {
            ModifierIcon use;
            //if we have icons not in use, use idle icons
            if (m_idleIcons.Count > 0) 
            {
                use = m_idleIcons[0];
                SetupIdleIcon(use, content);
            } 
            else 
            {
                // instantiate new icon if we don't have idle icon
                use = Instantiate(m_iconRef, transform);
                use.SetupIcon(content);
                m_iconsInUse.Add(use);
            }
            use.gameObject.transform.SetAsFirstSibling();

        }
        void SetupIdleIcon(ModifierIcon use, ModifierContent content) 
        {
            use.SetupIcon(content);
            m_idleIcons.Remove(use);
            m_iconsInUse.Add(use);
        }
        void ResetIcons()
        {
            foreach (var icon in m_iconsInUse)
            {
                icon.ResetIcon();
                m_idleIcons.Add(icon);
            }
            m_iconsInUse.Clear();
        }
    }
}