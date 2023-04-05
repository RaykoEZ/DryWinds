using UnityEngine;
using Curry.Explore;
namespace Curry.UI
{
    public class CharacterHpBar : MonoBehaviour 
    {
        [SerializeField] TacticalCharacter m_target = default;
        [SerializeField] ResourceBar m_bar = default;
        ICharacter m_currentTarget;
        void Start()
        {
            SetDisplayTarget(m_target);
        }
        public void SetDisplayTarget(ICharacter target) 
        {
            if (target != null)
            {
                m_currentTarget = target;
                m_currentTarget.TakeDamage += UpdateHp;
                m_currentTarget.RecoverHp += UpdateHp;
                m_bar?.SetMaxValue(m_currentTarget.CurrentHp);
                m_bar?.SetBarValue(m_currentTarget.MaxHp, forceInstantChange: true);
            }
        }
        void UpdateHp(int damageVal, int newHP) 
        {
            m_bar?.SetBarValue(newHP);
        }
    }

}


