using Curry.Explore;
using Curry.Util;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

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
}