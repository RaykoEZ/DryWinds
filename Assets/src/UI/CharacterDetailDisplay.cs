using Curry.Explore;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

namespace Curry.UI
{
    public class CharacterDetailDisplay : MonoBehaviour
    {
        [SerializeField] PanelUIHandler m_anim = default;
        [SerializeField] CharacterHpBar m_hpBar = default;
        [SerializeField] TextMeshProUGUI m_name = default;
        [SerializeField] TextMeshProUGUI m_speed = default;
        [SerializeField] TextMeshProUGUI m_moveRange = default;
        [SerializeField] AbilityList m_abilities = default;
        [SerializeField] ModifierList m_modifiers = default;
        bool showing = false;
        public void Display(ICharacter character) 
        {
            StopAllCoroutines();
            StartCoroutine(Display_Internal(character));
        }
        IEnumerator Display_Internal(ICharacter character) 
        {
            m_name.text = character.Name;
            m_hpBar?.SetDisplayTarget(character);
            m_speed.text = character.Speed.ToString();
            m_moveRange.text = character.MoveRange.ToString();
            IReadOnlyList<AbilityContent> abilities = character.AbilityDetails;
            m_abilities.Setup(character, abilities);
            if (character is IModifiable modify) 
            {
                modify.Refresh();
                IReadOnlyList<ModifierContent> mods = modify.CurrentStats.GetCurrentModifierDetails();
                m_modifiers.Setup(character, mods);
            }

            yield return new WaitForEndOfFrame();
            // start animation for entry
            m_anim?.Show();
            showing = true;
        }
        public void EndDisplay() 
        {
            if (showing) 
            {
                StartCoroutine(EndDisplay_Internal());
            }
        }
        IEnumerator EndDisplay_Internal() 
        {
            // Start anim for exit
            m_anim?.Hide();
            yield return new WaitForSeconds(0.5f);
            m_abilities.Hide();
            m_modifiers.Hide();
            showing = false;
        }
    }
}