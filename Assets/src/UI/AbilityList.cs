using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Curry.Explore;
using static UnityEngine.GraphicsBuffer;

namespace Curry.UI
{
    public interface ICharacterUIContentList<T> 
    {
        void Setup(ICharacter displayTarget, IReadOnlyList<T> content);
        void Show();
        void Hide();
    }
    // A list of a character's abilities
    public class AbilityList : MonoBehaviour , ICharacterUIContentList<AbilityContent>
    {
        // Handles list iterating animations
        [SerializeField] Animator m_anim = default;
        [SerializeField] AbilityDisplay m_display = default;
        List<AbilityContent> m_currentContent;
        int m_currentIndex = 0;
        ICharacter m_target;
        public void Setup(ICharacter displayTarget, IReadOnlyList<AbilityContent> contentList) 
        {
            m_currentIndex = 0;
            m_target = displayTarget;
            m_currentContent = new List<AbilityContent>(contentList);
            Show();
        }
        public void Show()
        {
            if (m_currentContent.Count > 0) 
            {
                m_display?.Setup(m_target, m_target.WorldPosition, m_currentContent[m_currentIndex]);
            }
        }
        public void Hide() 
        {
            ResetDisplay();
        }
        public void Next() 
        {
            StartCoroutine(Next_Internal());
        }
        public void Previous() 
        {
            StartCoroutine(Previous_Internal());
        }
        void ResetDisplay() 
        {
            m_target = null;
            m_currentContent.Clear();
            m_currentIndex = 0;
            m_display?.ResetDisplay();
        }

        IEnumerator Previous_Internal() 
        {
            m_anim?.ResetTrigger("moveRight");
            m_anim?.SetBool("introReady", false);
            m_anim?.SetTrigger("moveRight");
            yield return new WaitForEndOfFrame();
            m_currentIndex = m_currentIndex == 0 ? m_currentContent.Count - 1 : m_currentIndex - 1;
            m_anim?.SetBool("introReady", true);
            yield return new WaitForSeconds(0.25f);
            Show();
        }
        IEnumerator Next_Internal() 
        {
            m_anim?.ResetTrigger("moveLeft");
            m_anim?.SetBool("introReady", false);
            m_anim?.SetTrigger("moveLeft");
            yield return new WaitForEndOfFrame();
            m_currentIndex = m_currentIndex < m_currentContent.Count - 1 ? m_currentIndex + 1 : 0;
            m_anim?.SetBool("introReady", true);
            yield return new WaitForSeconds(0.25f);
            Show();
        }
    }
}