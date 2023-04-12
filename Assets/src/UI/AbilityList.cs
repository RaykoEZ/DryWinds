using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Curry.Explore;

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
            m_anim?.SetBool("introReady", true);
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
            ResetDisplay();
            m_currentIndex = m_currentIndex == 0 ? m_currentContent.Count - 1 : m_currentIndex - 1;
            Show();
            yield return null;
        }
        IEnumerator Next_Internal() 
        {
            m_anim?.ResetTrigger("moveLeft");
            m_anim?.SetBool("introReady", false);
            m_anim?.SetTrigger("moveLeft");
            yield return new WaitForEndOfFrame();
            ResetDisplay();
            m_currentIndex = m_currentIndex < m_currentContent.Count - 1 ? m_currentIndex + 1 : 0;
            Show();
            yield return null;
        }
    }
}