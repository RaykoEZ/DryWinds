using System.Collections.Generic;
using UnityEngine;

namespace Curry.UI
{
    public class PageIndicator : MonoBehaviour
    {
        [SerializeField] PageIcon m_pageIconToInstance = default;
        List<PageIcon> m_pageIcons = new List<PageIcon>();
        public List<PageIcon> PageIcons => m_pageIcons;
        public void Init(int numPages) 
        {
            m_pageIcons.Clear();
            for (int i = 0; i < numPages; i++)
            {
                PageIcon instance = Instantiate(m_pageIconToInstance, transform);
                m_pageIcons.Add(instance);
            }
        }
    }
}