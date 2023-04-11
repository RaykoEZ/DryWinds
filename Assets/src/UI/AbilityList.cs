using System.Collections.Generic;
using UnityEngine;

namespace Curry.UI
{
    public interface IUIContentList<T> 
    {
        void Setup(List<T> content);
        void Show();
        void Hide();
    }

    // A list of a character's abilities
    public class AbilityList : MonoBehaviour , IUIContentList<AbilityContent>
    {
        // Handles list iterating animations
        [SerializeField] Animator m_anim = default;
        [SerializeField] AbilityDisplay m_display = default;
        List<AbilityContent> m_currentContent;
        int m_currentIndex = 0;
        public void Setup(List<AbilityContent> contentList) 
        {
            m_currentContent = contentList;
            Show();
        }
        public void Show()
        {
            m_display?.Setup(m_currentContent[m_currentIndex]);
        }
        public void Hide() 
        {
            ResetDisplay();
        }
        public void Next() 
        {
            m_currentIndex = m_currentIndex < m_currentContent.Count - 1 ? m_currentIndex + 1 : 0;
            Show();
        }
        public void Previous() 
        {
            m_currentIndex = m_currentIndex == 0 ? m_currentContent.Count - 1 : m_currentIndex - 1;
            Show();
        }
        void ResetDisplay() 
        {
            m_currentContent.Clear();
            m_currentIndex = 0;
            m_display?.ResetDisplay();
        }
    }
}