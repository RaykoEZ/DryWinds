using Curry.Explore;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.UI
{
    // For displaying a list of modifiers that is active on a character
    public class ModifierDisplay : MonoBehaviour 
    {
        [SerializeField] ModifierIcon m_iconRef = default;
        List<ModifierIcon> m_currentIcons = new List<ModifierIcon>();
        public void DisplayNew(List<ModifierContent> content) 
        {
            m_currentIcons = new List<ModifierIcon>(content.Count);
            foreach(var contentItem in content) 
            {
                OnModifierAdd(contentItem);
            }
        }
        public void EndDisplay() 
        {
            ResetIcons();
        }
        public void OnModifierAdd(ModifierContent content) 
        {
            ModifierIcon instance = Instantiate(m_iconRef, transform);
            instance.SetupIcon(content);
            m_currentIcons.Add(instance);
        }
        void ResetIcons()
        {
            foreach (var icon in m_currentIcons)
            {
                icon.ResetIcon();
                Destroy(icon);
            }
            m_currentIcons.Clear();
        }
    }
}