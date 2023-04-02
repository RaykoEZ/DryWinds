using System.Collections;
using UnityEngine;

namespace Curry.Explore
{

    public class SweepScanner : AdventCard, ICooldown
    {
        [SerializeField] CooldownModule m_cooldown = default;
        [SerializeField] Defog m_defog = default;
        public bool IsOnCooldown => m_cooldown.IsOnCooldown;
        public int CooldownTime { get => m_cooldown.CooldownTime; set => m_cooldown.CooldownTime = value; }
        public int Current { get => m_cooldown.Current; set => m_cooldown.Current = value; }
        public override IEnumerator ActivateEffect(IPlayer user)
        {
            m_defog.ApplyEffect(user, user);
            yield return new WaitForEndOfFrame();
        }
        public void Tick(int dt, out bool isOnCoolDown)
        {
            m_cooldown.Tick(dt, out isOnCoolDown);
        }
        public void TrggerCooldown()
        {
            m_cooldown.TrggerCooldown();
        }
    }
}
