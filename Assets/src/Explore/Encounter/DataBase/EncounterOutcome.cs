using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public abstract class EncounterOutcome : ScriptableObject, IEncounterModule
    {
        [TextArea]
        [SerializeField] string m_detail = default; 
        public string DetailText => m_detail;
        public abstract EncounterContent GetOutcomeContent();
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