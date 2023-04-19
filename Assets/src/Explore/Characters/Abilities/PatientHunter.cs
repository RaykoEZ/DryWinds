using Curry.Util;
using System;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public class PatientHunter : BaseAbility, IAbility, IStackableEffect
    {
        [SerializeField] Prepared m_prep = default;
        [SerializeField] BaseAbility m_abilityRangeRef = default;
        bool m_activated = false;
        Prepared m_instace;
        public override RangeMap Range => m_abilityRangeRef.Range;
        public override AbilityContent GetContent()
        {
            var ret = base.GetContent();
            ret.Name = "Patient Hunter";
            ret.Description = $"Reaction: Gain [Prepared] stack when target is inside detection range " +
                $"(max. {m_prep.MaxStack} times), " +
                $"Stacks reset upon first attack.";
            return ret;
        }
        public void AddStack(int addVal = 1) 
        {
            m_instace?.AddStack(addVal);
        }
        public void SubtractStack(int subVal = 1)
        {
            m_instace?.SubtractStack(subVal);
        }
        public void ResetStack() 
        {
            m_instace?.ResetStack();
        }
        // Activate this once on spawn
        public void Activate(IModifiable applyTo)
        {
            if (!m_activated) 
            {
                m_instace = new Prepared(m_prep);
                applyTo?.CurrentStats.ApplyModifier(m_instace);
                m_activated = true;
            }
            else 
            {
                Debug.Log($"{nameof(PatientHunter)} is already active");
            }
        }
    }
}
