using Curry.Explore;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.UI.Tutorial
{
    [RequireComponent(typeof(HideableUI))]
    public class PageTopicManager : MonoBehaviour
    {
        [SerializeField] List<UIPage> m_pages = default;
        [SerializeField] PageIndicator m_pageIndicator = default;
        HideableUI UIAnim => GetComponent<HideableUI>();
        int m_currentPage = 0;
        void Start()
        {
            UIAnim?.Hide();
            m_pageIndicator?.Init(m_pages.Count);
        }
        public void BeginDisplay() 
        {
            m_currentPage = 0;
            UIAnim?.Show();
            m_pages[m_currentPage]?.Play();
        }
        public void EndDisplay() 
        {
            m_pages[m_currentPage]?.Stop();
            UIAnim?.Hide();
            m_currentPage = 0;
        }
        public void NextPage() 
        {
            m_pageIndicator?.PageIcons[m_currentPage]?.StopHighlight();
            m_pages[m_currentPage]?.Stop();
            m_currentPage = m_currentPage < m_pages.Count - 1? ++m_currentPage : 0;
            m_pages[m_currentPage]?.Play();
            m_pageIndicator?.PageIcons[m_currentPage]?.Highlight();
        }
        public void PreviousPage() 
        {
            m_pageIndicator?.PageIcons[m_currentPage]?.StopHighlight();
            m_pages[m_currentPage]?.Stop();
            m_currentPage = m_currentPage > 0 ? --m_currentPage : m_pages.Count - 1;
            m_pages[m_currentPage]?.Play();
            m_pageIndicator?.PageIcons[m_currentPage]?.Highlight();
        }
    }
}