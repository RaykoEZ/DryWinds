using Curry.Explore;
using Curry.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.UI
{
    public class TimeDealerManager : MonoBehaviour
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] List<EncounterOptionDisplay> m_optionDisplays = default;
        [SerializeField] EncounterEntry m_sacrificeOptions = default;
        GameStateContext m_contextRef;
        public void Begin(GameStateContext context) 
        {
            m_contextRef = context;
            List<EncounterOptionAttribute> optionContent =
                SamplingUtil.SampleFromList(m_sacrificeOptions.Detail.Options, m_optionDisplays.Count);
            EncounterOptionDisplay currentOption;
            for (int i = 0; i < m_optionDisplays.Count; i++)
            {
                currentOption = m_optionDisplays[i];
                currentOption.Init(optionContent[i], context);
                currentOption.OnChosen += OnSacrificeChosen;
            }
            m_anim.SetBool("active", true);
        }
        void OnSacrificeChosen(EncounterOptionAttribute chosen) 
        {
            foreach (EncounterOptionDisplay option in m_optionDisplays)
            {
                option.OnChosen -= OnSacrificeChosen;
            }
            StartCoroutine(OnChosen_Internal(chosen.OptionDetail));
        }
        IEnumerator OnChosen_Internal(EncounterOption outcome)
        {
            var content = outcome.GetOutcomeContent();
            // call option effect when we finish dialogue
            yield return StartCoroutine(EncounterOption.Activate(m_contextRef, content.Effects));
            yield return new WaitForEndOfFrame();
            m_anim.SetBool("active", false);
        }
    }
}