using UnityEngine;
using Curry.Explore;
using System.Collections;

namespace Curry.UI
{
    // A shorthand range previewer for displaying danger zone highlights
    // for enemy skills and hazards
    public class DangerZonePreviewHanadler : RangePreviewHandler 
    {
        public void DisplayDangerZone(Transform origin, AbilityContent ability)
        {
            if (ability != null && ability != AbilityContent.None)
            {
                BeginDisplay(origin, ability.TargetingRange);
            }
            else 
            {
                // If we don't have range to display anymore, we clear danager
                ClearRangePattern(origin);
            }
        }
        public IEnumerator ClearDanagerZone(Transform origin)
        {
            ClearRangePattern(origin);
            yield return new WaitForEndOfFrame();
        }
    }
}