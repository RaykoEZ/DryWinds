using UnityEngine;
using Curry.Explore;
using System.Collections.Generic;

namespace Curry.UI
{
    public class RangePreviewHandler : MonoBehaviour 
    {
        [SerializeField] protected GameObject m_tileRef = default;
        [SerializeField] protected RangeDisplayHandler m_rangeDisplay = default;
        protected List<string> m_patternNamesInPreview = new List<string>();
        protected virtual string PatternIdPrefix => "Detail/Zone";
        // returns the name/id of the current range pattern displayed
        public virtual string BeginDisplay(Vector3 origin, AbilityContent ability) 
        {
            string name = $"{PatternIdPrefix}/{origin}/{ability.Name}";
            // Display range preview on range Display
            m_rangeDisplay.ShowRange(
                name,
                m_tileRef,
                ability.TargetingRange);
            if (!m_patternNamesInPreview.Contains(name))
            {
                m_rangeDisplay.MoveRangeTile(name, m_tileRef, origin);
                m_patternNamesInPreview.Add(name);
            }
            return name;
        }
        public virtual void ClearRangePattern(string tileName) 
        {
            if (m_patternNamesInPreview.Contains(tileName)) 
            {
                m_rangeDisplay.ClearRangeTile(tileName, m_tileRef);
            }
        }
        // Clear all range tiles currently displayed
        public virtual void ResetDisplay()
        {
            foreach (string name in m_patternNamesInPreview)
            {
                m_rangeDisplay.ClearRangeTile(name, m_tileRef);
            }
            m_patternNamesInPreview.Clear();
            m_rangeDisplay?.HidePrompt();
        }
    }
}