using UnityEngine;
using Curry.Explore;
namespace Curry.UI
{
    public class PlayerHpBar : MonoBehaviour 
    {
        [SerializeField] Adventurer m_player = default;
        [SerializeField] ResourceBar m_bar = default;
        void Start()
        {
            m_player.TakeDamage += UpdateHp;
            m_bar?.SetMaxValue(m_player.CurrentHp);
            m_bar?.SetBarValue(m_player.MaxHp, forceInstantChange: true);
        }
        void UpdateHp(int damageVal, int newHP) 
        {
            m_bar?.SetBarValue(newHP);
        }
    }

}


