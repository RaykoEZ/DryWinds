using Curry.Explore;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Curry.UI
{
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_damage = default;
        [SerializeField] TextMeshProUGUI m_heal = default;
        [SerializeField] Animator m_anim = default;
        [SerializeField] TacticalCharacter m_target = default;
        void OnEnable()
        {
            m_target.TakeDamage += TriggerDamagePopup;
            m_target.RecoverHp += TriggerHealPopup;
        }
        void OnDisable()
        {
            m_target.TakeDamage -= TriggerDamagePopup;
            m_target.RecoverHp -= TriggerHealPopup;
        }
        void TriggerDamagePopup(int damage, int _) 
        {
            m_damage.text = damage.ToString();
            m_anim?.ResetTrigger("takeDamage");
            m_anim?.SetTrigger("takeDamage");
        }
        void TriggerHealPopup(int heal, int _)
        {
            m_heal.text = heal.ToString();
            m_anim?.ResetTrigger("heal");
            m_anim?.SetTrigger("heal");
        }
    }
}