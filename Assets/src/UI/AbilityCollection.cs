using System.Collections.Generic;
using UnityEngine;

namespace Curry.UI
{
    // A list of a character's abilities
    public class AbilityCollection : MonoBehaviour 
    {       
        [SerializeField] AbilityDisplay m_display = default;
        List<AbilityContent> m_currentContent;
        int m_currentIndex = 0;
        public void BeginDisplay(List<AbilityContent> contentList) 
        {
            m_currentContent = contentList;
            Show();
        }
        void Show() 
        {
            m_display?.Hide();
            m_display?.Setup(m_currentContent[m_currentIndex]);
            m_display?.Show();
        }
        public void StopDisplay() 
        {
            m_display?.Hide();
            ResetDisplay();
        }
        public void NextAbility() 
        {
            m_currentIndex = m_currentIndex < m_currentContent.Count - 1 ? m_currentIndex + 1 : 0;
            Show();
        }
        public void PreviousAbility() 
        {
            m_currentIndex = m_currentIndex == 0 ? m_currentContent.Count - 1 : m_currentIndex - 1;
            Show();
        }
        void ResetDisplay() 
        {
            m_currentContent.Clear();
            m_currentIndex = 0;
            m_display?.Hide();
            m_display?.ResetDisplay();
        }
    }
}