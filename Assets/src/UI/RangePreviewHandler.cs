using UnityEngine;
using Curry.Explore;
using System.Collections.Generic;
using Curry.Util;

namespace Curry.UI
{
    public class RangePreviewHandler : MonoBehaviour 
    {
        [SerializeField] protected GameObject m_tileRef = default;
        [SerializeField] protected RangeDisplayHandler m_rangeDisplay = default;
        protected List<string> m_patternNamesInPreview = new List<string>();
        protected virtual string PatternIdPrefix => "Detail/Zone";
        protected virtual string GetRangePatternId(Transform origin) 
        { 
            return $"{PatternIdPrefix}/{origin.gameObject.name}";
        }
        // returns the name/id of the current range pattern displayed
        public virtual void BeginDisplay(Transform origin, RangeMap range, bool overwrite = true) 
        {
            string name = GetRangePatternId(origin);
            if (overwrite)
            {
                ClearRangePattern(origin);
            }
            Vector2 pos = origin.position;
            // Display range preview on range Display
            m_rangeDisplay.ShowRange(
                name,
                m_tileRef,
                range);
            if (!m_patternNamesInPreview.Contains(name))
            {
                m_rangeDisplay.MoveRangeTile(name, m_tileRef, pos);
                m_patternNamesInPreview.Add(name);
            }
        }
        public virtual void ClearRangePattern(Transform origin)
        {
            string name = GetRangePatternId(origin);
            if (m_patternNamesInPreview.Contains(name)) 
            {
                m_rangeDisplay.ClearRangeTile(name, m_tileRef);
                m_patternNamesInPreview.Remove(name);
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