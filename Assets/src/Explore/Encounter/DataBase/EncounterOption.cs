using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    // Conditions to meet to unlock an encounter
    [Serializable]
    public class EncounterUnlockCondition 
    {
        [SerializeField] List<GameStateCondition> m_unlockConditions = default;
        public bool AreConditionsMet(GameStateContext context) 
        {
            bool ret = true;
            foreach(var condition in m_unlockConditions) 
            {
                ret &= condition.IsConditionMet(context);
            }
            return ret;
        }
    }
    public abstract class EncounterOption : ScriptableObject, IEncounterModule
    {
        [TextArea]
        [SerializeField] string m_detail = default;
        /// <summary>
        /// Only one of the condition item in this list need to be met to unlock
        /// List contains all valid unlock conditions, only need to met one of of any
        /// </summary>
        [SerializeField] List<EncounterUnlockCondition> m_validUnlockConditions = default;
        public string DetailText => m_detail;
        public abstract EncounterContent GetOutcomeContent();
        // Implement conditions to lock/unlock an option
        public bool IsOptionAvailable(GameStateContext context) 
        {
            bool ret = true;
            foreach (var condition in m_validUnlockConditions)
            {
                ret |= condition.AreConditionsMet(context);
            }
            return ret;
        }
        public static IEnumerator Activate(GameStateContext context, List<BaseEffectResource> effects)
        {
            foreach (BaseEffectResource effect in effects)
            {
                effect?.Activate(context);
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }
    }
}