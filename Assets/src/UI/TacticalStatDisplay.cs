using Curry.Explore;
using Curry.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Curry.UI
{
    public class TacticalStatDisplay : MonoBehaviour
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] CharacterHpBar m_hpBar = default;
        [SerializeField] TextMeshProUGUI m_speed = default;
        [SerializeField] TextMeshProUGUI m_moveRange = default;

        public void Display(ICharacter character) 
        {
            StartCoroutine(Display_Internal(character));
        }
        IEnumerator Display_Internal(ICharacter character) 
        {
            m_hpBar?.SetDisplayTarget(character);
            m_speed.text = character.Speed.ToString();
            m_moveRange.text = character.MoveRange.ToString();
            yield return new WaitForEndOfFrame();
            // start animation for entry
        }
        public void EndDisplay() 
        {
            StartCoroutine(EndDisplay_Internal());
        }
        IEnumerator EndDisplay_Internal() 
        {
            // Start anim for exit
            yield return new WaitForEndOfFrame();
        }
    }
    [Serializable]
    public struct AbilityContent 
    {
        [SerializeField] public string Name;
        [SerializeField] public string Description;
        // This sprite will be a grid pattern
        [SerializeField] public Sprite RangePattern;
        [SerializeField] public Sprite Icon;
    }
    // A list of a character's abilities
    public class AbilityCollection : MonoBehaviour 
    {
        List<AbilityContent> m_currentContent = new List<AbilityContent>();
        public void Setup(List<AbilityContent> contentList) { }
        public void StopDisplay() { }
        public void NextAbility() { }
        public void PreviousAbility() { }
        void ResetDisplay() { }
    }
    // A ui element for character ability 
    public class AbilityDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_name = default;
        [SerializeField] TextMeshProUGUI m_description = default;
        [SerializeField] Image m_icon = default;
        public void Setup(AbilityContent content) { }
        public void ResetDisplay() { }
        public void Show() { }
        public void Hide() { }
    }

    // For displaying a list of modifiers that is active on a character
    public class ModifierDisplay : MonoBehaviour 
    {
        List<ModifierIcon> m_currentIcons = new List<ModifierIcon>();
        public void DisplayNew() 
        { 
        
        }
        public void EndDisplay() 
        { 
        
        }
        void OnModifierAdd() 
        { 
        
        }
        void OnModifierRemove() 
        {
        
        }
        void ResetIcons()
        {
            
        }
    }
    // A UI item to display a bespoke character modifier
    public class ModifierIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] TextMeshProUGUI m_name = default;
        [SerializeField] TextMeshProUGUI m_description = default;
        [SerializeField] TextMeshProUGUI m_expire = default;
        [SerializeField] Image m_icon = default;

        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void ShowTip() { }
        public void HideTip() { }
        public void SetupIcon() 
        { 
        
        }
        public void ResetIcon()
        {
            
        }
    }
}