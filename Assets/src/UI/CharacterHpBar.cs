using UnityEngine;
using Curry.Explore;
using TMPro;

namespace Curry.UI
{
    public class CharacterHpBar : MonoBehaviour 
    {
        [SerializeField] TacticalCharacter m_initTarget = default;
        [SerializeField] ResourceBar m_bar = default;
        [SerializeField] TextMeshProUGUI m_hpText = default;
        ICharacter m_currentTarget;
        void Start()
        {
            SetDisplayTarget(m_initTarget);
        }
        public void SetDisplayTarget(ICharacter target) 
        {
            if (target != null)
            {
                m_currentTarget = target;
                m_currentTarget.TakeDamage += UpdateHp;
                m_currentTarget.RecoverHp += UpdateHp;
                SetHpText(m_currentTarget.CurrentHp, m_currentTarget.MaxHp);
                m_bar?.SetMaxValue(m_currentTarget.CurrentHp);
                m_bar?.SetBarValue(m_currentTarget.MaxHp, forceInstantChange: true);
            }
        }
        void SetHpText(int current, int max) 
        {
            if (m_hpText == null) return;
            m_hpText.text = $"{current} / {max}";
        }
        void UpdateHp(int damageVal, int newHP) 
        {
            m_bar?.SetBarValue(newHP);
            SetHpText(newHP, (int)m_bar.Max);
        }
    }
}


