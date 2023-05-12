using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "EncOpt_", menuName = "Curry/Encounter/Options/New Encounter Effect for Encounter Option(s)", order = 3)]
    public class EncounterEffect : ScriptableObject, IEncounterModule
    {
        [SerializeField] List<BaseEffectResource> m_effects = default;
        public virtual List<BaseEffectResource> SerializeProperties => m_effects;
        public virtual IEnumerator Activate(GameStateContext context) 
        {
            foreach (BaseEffectResource effect in m_effects) 
            {
                effect?.Activate(context);
                yield return new WaitForEndOfFrame();
            }
            yield return null; 
        }
    }
}