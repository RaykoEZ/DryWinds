using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Curry.Explore;
using Curry.Util;

namespace Curry.UI
{
    // A ui element for character ability 
    public class AbilityDisplay : RangePreviewHandler
    {
        [SerializeField] TextMeshProUGUI m_name = default;
        [SerializeField] TextMeshProUGUI m_description = default;
        [SerializeField] Image m_icon = default;
        protected override string PatternIdPrefix => "Detail/Preview";
        public virtual void DisplayAbilityRange(Transform origin, AbilityContent ability) 
        {
            m_rangeDisplay.HidePrompt();
            m_name.text = ability.Name;
            m_description.text = ability.Description;
            m_icon.sprite = ability.Icon;
            BeginDisplay(origin, ability.TargetingRange);
        }
        public override void ResetDisplay() 
        {        
            m_name.text = "";
            m_description.text = "";
            m_icon.sprite = null;
            //Reset Range display
            base.ResetDisplay();
        }
    }
}