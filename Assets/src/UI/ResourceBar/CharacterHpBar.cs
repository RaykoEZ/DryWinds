using UnityEngine;
using Curry.Explore;
using TMPro;
using UnityEngine.PlayerLoop;

namespace Curry.UI
{
    public class CharacterHpBar : MonoBehaviour 
    {
        [SerializeField] TacticalCharacter m_initTarget = default;
        [SerializeField] ResourceBar m_bar = default;
        [SerializeField] TextMeshProUGUI m_hpText = default;
        TacticalCharacter m_currentTarget;
        void Start()
        {
            SetDisplayTarget(m_initTarget);
        }
        public void SetDisplayTarget(TacticalCharacter target) 
        {
            if (m_currentTarget != null) 
            {
                m_currentTarget.CurrentStats.OnStatUpdated -= UpdateHpStats;
                m_currentTarget.TakeDamage -= UpdateHp;
                m_currentTarget.RecoverHp -= UpdateHp;
            }

            if (target != null)
            {
                m_currentTarget = target;
                m_currentTarget.CurrentStats.OnStatUpdated += UpdateHpStats;
                m_currentTarget.TakeDamage += UpdateHp;
                m_currentTarget.RecoverHp += UpdateHp;
                SetHpText(m_currentTarget.CurrentHp, m_currentTarget.MaxHp);
                m_bar?.SetMaxValue(m_currentTarget.MaxHp);
                m_bar?.SetBarValue(m_currentTarget.CurrentHp, forceInstantChange: true);
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
        void UpdateHpStats(TacticalStats newStats)
        {
            if (newStats.MaxHp != m_bar.Max) 
            {
                m_bar?.SetMaxValue(newStats.MaxHp);
            }
            if (newStats.Hp != m_bar.Current) 
            {
                UpdateHp(0, newStats.Hp);
                SetHpText(newStats.Hp, newStats.MaxHp);
            }
        }
    }
}


