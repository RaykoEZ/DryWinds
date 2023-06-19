using System;
using UnityEngine;
using UnityEngine.UI;

namespace Curry.Explore
{
    // Used to dynamically set layout group spacing depending on number of items in the horizontal layout group
    [Serializable]
    public class LayoutSpaceSetting
    {
        [SerializeField] float m_minSpacing = default;
        [SerializeField] float m_maxSpacing = default;
        [SerializeField] float itemWidth = default;
        [SerializeField] HorizontalLayoutGroup m_layout = default;
        [SerializeField] RectTransform m_layoutLimit = default;

        // Used to calculate the ideal spacing for cards in a horizontal list of cards
        public void UpdateSpacing()
        {
            float numItem = m_layout.transform.childCount;
            float newSpacing = ((m_layoutLimit.rect.width - itemWidth) / (numItem - 1)) - itemWidth;
            newSpacing = Mathf.Clamp(newSpacing, m_minSpacing, m_maxSpacing);
            m_layout.spacing = newSpacing;
        }
    }
}