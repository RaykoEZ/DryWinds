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
        [SerializeField] AbilityDisplay m_displaySample = default;
        List<AbilityContent> m_currentContent;
        List<AbilityDisplay> m_abilityDisplays;
        int m_currentIndex = 0;
        public void Setup(List<AbilityContent> contentList) 
        {
            m_currentContent = contentList;
            m_abilityDisplays = new List<AbilityDisplay>(contentList.Count);
            foreach(var content in m_currentContent) 
            {
                AbilityDisplay instance = Instantiate(m_displaySample, transform);
                instance.Setup(content);
                m_abilityDisplays.Add(instance);
            }
        }
        public void StopDisplay() 
        {
            m_abilityDisplays[m_currentIndex]?.Hide();
            ResetDisplay();
        }
        public void NextAbility() 
        {
            m_abilityDisplays[m_currentIndex]?.Hide();
            m_currentIndex = m_currentIndex < m_abilityDisplays.Count - 1 ? m_currentIndex + 1 : 0;
            m_abilityDisplays[m_currentIndex]?.Show();

        }
        public void PreviousAbility() 
        {
            m_abilityDisplays[m_currentIndex]?.Hide();
            m_currentIndex = m_currentIndex == 0 ? m_abilityDisplays.Count - 1 : m_currentIndex - 1;
            m_abilityDisplays[m_currentIndex]?.Show();
        }
        void ResetDisplay() 
        {
            m_currentContent.Clear();
            m_currentIndex = 0;
            foreach (AbilityDisplay display in m_abilityDisplays)
            {
                display.ResetDisplay();
                Destroy(display);
            }
            m_abilityDisplays.Clear();
        }
    }
    // A ui element for character ability 
    public class AbilityDisplay : MonoBehaviour
    {
        [SerializeField] PanelUIHandler m_uiHandler = default;
        [SerializeField] TextMeshProUGUI m_name = default;
        [SerializeField] TextMeshProUGUI m_description = default;
        [SerializeField] Image m_icon = default;
        [SerializeField] Image m_rangePattern = default;
        public void Setup(AbilityContent content) 
        {
            m_name.text = content.Name;
            m_description.text = content.Description;
            m_icon.sprite = content.Icon;
            m_rangePattern.sprite = content.RangePattern;
        }
        public void ResetDisplay() 
        {
            m_name.text = "";
            m_description.text = "";
        }
        public void Show() 
        {
            m_uiHandler?.Show();
        }
        public void Hide() 
        {
            m_uiHandler?.Hide();
        }
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
        [SerializeField] PanelUIHandler m_uiHandler = default;
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

        public void Show() 
        {
            m_uiHandler?.Show();
        }
        public void Hide() 
        {
            m_uiHandler?.Hide();
        }
        public void SetupIcon() 
        { 
        
        }
        public void ResetIcon()
        {
            
        }
    }
}